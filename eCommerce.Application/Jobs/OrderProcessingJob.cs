using Microsoft.Extensions.Logging;

namespace eCommerce.Application.Jobs
{
    public class OrderProcessingJob
    {
        private readonly ILogger<OrderProcessingJob> _logger;

        public OrderProcessingJob(ILogger<OrderProcessingJob> logger)
        {
            this._logger = logger;
        }

        public async Task SendOrderConfirmationAsync()
        {
            Guid guid = Guid.NewGuid();

            _logger.LogInformation($"Executing SendOrderConfirmationAsync job for OrderId {guid}");

            await Task.Delay(5000); // simulating an email sending logic
        }

    }
}
