using eCommerce.Application.Contracts;
using eCommerce.Application.Jobs;
using eCommerce.Domain.Contracts;
using eCommerce.Infrastructure.Background;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Infrastructure.Persistence.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;


namespace eCommerce.Infrastructure
{
    public static class ServiceRegistration
    {

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


            services.AddHangfireInfrastructure(connectionString);

            return services;
        }



        public static IServiceCollection AddHangfireInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddHangfire(config => config
                     .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                     .UseSimpleAssemblyNameTypeSerializer()
                     .UseRecommendedSerializerSettings()
                     .UseSqlServerStorage(connectionString, new Hangfire.SqlServer.SqlServerStorageOptions
                     {
                         CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                         SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                         QueuePollInterval = TimeSpan.Zero,
                         UseRecommendedIsolationLevel = true,
                         PrepareSchemaIfNecessary = true, // creates (Hangfire table),
                         DisableGlobalLocks = true
                        
                     }));

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = Math.Min(Environment.ProcessorCount * 2, 10);
                options.Queues = new[] { "default" };
            });

            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();

            services.AddScoped<OrderProcessingJob>();

            return services;
        }
    }
}
