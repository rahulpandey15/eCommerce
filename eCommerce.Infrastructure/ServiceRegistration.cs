using eCommerce.Domain.Contracts;
using eCommerce.Infrastructure.Persistence.Entities;
using eCommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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



            services.AddScoped<IUserRepository, UserRepository>();  // DI register

            return services;
        }


    }
}
