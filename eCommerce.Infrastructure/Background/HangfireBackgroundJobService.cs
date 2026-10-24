using eCommerce.Application.Contracts;
using System.Linq.Expressions;
using Hangfire;

namespace eCommerce.Infrastructure.Background
{
    public class HangfireBackgroundJobService : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _client;
        private readonly IRecurringJobManager _recurring;

        public HangfireBackgroundJobService(
            IBackgroundJobClient client,
            IRecurringJobManager recurring)
        {
            _client = client;
            _recurring = recurring;
        }


        public void AddOrUpdateRecurring<T>(
            string jobId, 
            Expression<Func<T, Task>> job, 
            string cron)
        {
            _recurring.AddOrUpdate(jobId, job, cron, new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Utc
            });
        }

        public string Enqueue<T>(Expression<Func<T, Task>> job) => _client.Enqueue(job);

        public string Schedule<T>(Expression<Func<T, Task>> job, TimeSpan delay) => _client.Schedule(job, delay);
    }
}
