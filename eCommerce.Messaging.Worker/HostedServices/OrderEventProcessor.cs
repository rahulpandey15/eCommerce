using Azure.Messaging.ServiceBus;
using eCommerce.Application.Common.Messaging.Events;
using eCommerce.Infrastructure.Messaging.AzureServiceBus;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Messaging.Worker.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace eCommerce.Messaging.Worker.HostedServices
{
    public sealed class OrderEventProcessor(
        ServiceBusClientProvider clientProvider,
        ServiceBusMessageSerializer serializer,
        IServiceScopeFactory scopeFactory,
        IOptions<ServiceBusOptions> options,
        ILogger<OrderEventProcessor> logger) : IHostedService, IAsyncDisposable
    {
        private readonly List<ServiceBusProcessor> _processors = [];
        private readonly ServiceBusOptions _options = options.Value;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
            {
                logger.LogWarning("Messaging worker is disabled because AzureServiceBus:Enabled is false.");
                return;
            }

            var processor = clientProvider.GetClient().CreateProcessor(
                _options.QueueName,
                new ServiceBusProcessorOptions
                {
                    AutoCompleteMessages = false,
                    MaxConcurrentCalls = _options.MaxConcurrentCalls,
                    PrefetchCount = _options.PrefetchCount,
                    MaxAutoLockRenewalDuration = _options.MaxAutoLockRenewalDuration
                });

            processor.ProcessMessageAsync += ProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessErrorAsync;

            _processors.Add(processor);

            await processor.StartProcessingAsync(cancellationToken);
            logger.LogInformation(
                "Started Service Bus processor for queue {QueueName}",
                _options.QueueName);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var processor in _processors)
            {
                await processor.StopProcessingAsync(cancellationToken);
            }
        }

        private async Task ProcessMessageAsync(
            ProcessMessageEventArgs args)

        {
            var message = args.Message;
            ServiceBusEvent<OrderCreatedPayload> integrationEvent;
            try
            {
                integrationEvent = serializer.DeserializeEnvelope<OrderCreatedPayload>(message.Body);
                if (!string.Equals(integrationEvent.EventType, "OrderCreated", StringComparison.Ordinal))
                {
                    await args.DeadLetterMessageAsync(
                        message,
                        "UnsupportedEventType",
                        $"Event type '{integrationEvent.EventType}' is not supported.");
                    return;
                }
            }
            catch (JsonException exception)
            {
                logger.LogError(
                    exception,
                    "Dead-lettering malformed message {MessageId} from queue {QueueName}",
                    message.MessageId,
                    _options.QueueName);

                await args.DeadLetterMessageAsync(message, "MalformedMessage", exception.Message);
                return;
            }

            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var actions = scope.ServiceProvider
                    .GetRequiredService<IEnumerable<IOrderCreatedAction>>()
                    .ToArray();

                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                const string consumerName = "order-event-processor";
                var alreadyProcessed = await dbContext.ProcessedIntegrationEvents.AnyAsync(
                    item => item.EventId == integrationEvent.EventId && item.ConsumerName == consumerName);

                if (alreadyProcessed)
                {
                    await transaction.CommitAsync();
                    await args.CompleteMessageAsync(message);
                    logger.LogInformation(
                        "Completed duplicate message {MessageId} for EventId {EventId} from queue {QueueName}",
                        message.MessageId,
                        integrationEvent.EventId,
                        _options.QueueName);
                    return;
                }

                foreach (var action in actions)
                {
                    await action.HandleAsync(integrationEvent, CancellationToken.None);
                }

                dbContext.ProcessedIntegrationEvents.Add(new ProcessedIntegrationEvent
                {
                    EventId = integrationEvent.EventId,
                    ConsumerName = consumerName,
                    ProcessedAtUtc = DateTimeOffset.UtcNow
                });
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                await args.CompleteMessageAsync(message);

                logger.LogInformation(
                    "Processed message {MessageId} for EventId {EventId}, OrderId {OrderId}, DeliveryCount {DeliveryCount}, Queue {QueueName}",
                    message.MessageId,
                    integrationEvent.EventId,
                    integrationEvent.Payload.OrderId,
                    message.DeliveryCount,
                    _options.QueueName);
            }
            catch (ServiceBusException exception) when (exception.Reason == ServiceBusFailureReason.MessageLockLost)
            {
                logger.LogWarning(
                    exception,
                    "Message lock expired for MessageId {MessageId}, EventId {EventId}, Queue {QueueName}",
                    message.MessageId,
                    integrationEvent.EventId,
                    _options.QueueName);
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Transient processing failure for MessageId {MessageId}, EventId {EventId}, OrderId {OrderId}, DeliveryCount {DeliveryCount}, Queue {QueueName}",
                    message.MessageId,
                    integrationEvent.EventId,
                    integrationEvent.Payload.OrderId,
                    message.DeliveryCount,
                    _options.QueueName);
                await args.AbandonMessageAsync(message);
            }
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogError(
                args.Exception,
                "Service Bus processor error for Entity {EntityPath}, ErrorSource {ErrorSource}",
                args.EntityPath,
                args.ErrorSource);
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var processor in _processors)
            {
                await processor.DisposeAsync();
            }
        }
    }
}
