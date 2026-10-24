using Microsoft.Extensions.Logging;

namespace eCommerce.Application.Jobs
{
    public class EnqueuedJob
    {
        private readonly ILogger<OrderProcessingJob> _logger;

        public EnqueuedJob(ILogger<OrderProcessingJob> logger)
        {
            this._logger = logger;
        }

        public async Task EnqueueJob()
        {
            Guid guid = Guid.NewGuid();

            _logger.LogInformation($"Executing EnqueueJob job for OrderId {guid}");

            await Task.Delay(5000); // simulating an email sending logic
        }

    }
}
