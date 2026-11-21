using eCommerce.Application;
using eCommerce.Application.Common.Messaging;
using eCommerce.Infrastructure;
using eCommerce.Infrastructure.Messaging.AzureServiceBus;
using eCommerce.Infrastructure.Messaging.Outbox;
using eCommerce.Messaging.Worker.Consumers;
using eCommerce.Messaging.Worker.HostedServices;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSerilog((services, configuration) => configuration
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddHttpContextAccessor();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "127.0.0.1:6379";
    options.InstanceName = "redis-instance";
});
builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DatabaseConnection")!,
    addHangfire: false);

builder.Services.Configure<ServiceBusOptions>(
    builder.Configuration.GetSection("AzureServiceBus"));
builder.Services.Configure<OutboxProcessingOptions>(
    builder.Configuration.GetSection("Messaging:Outbox"));
builder.Services.AddSingleton<ServiceBusClientProvider>();
builder.Services.AddSingleton<ServiceBusMessageSerializer>();
builder.Services.AddSingleton<AzureServiceBusEventPublisher>();
builder.Services.AddSingleton<IEventPublisher>(
    services => services.GetRequiredService<AzureServiceBusEventPublisher>());
builder.Services.AddScoped<OutboxMessageStore>();

builder.Services.AddTransient<IOrderCreatedAction, EmailNotificationAction>();
builder.Services.AddTransient<IOrderCreatedAction, InventoryManagementAction>();
builder.Services.AddTransient<IOrderCreatedAction, OrderAuditAction>();

//builder.Services.AddHostedService<OutboxPublisherService>();
builder.Services.AddHostedService<OrderEventProcessor>();
var host = builder.Build();
host.Run();
