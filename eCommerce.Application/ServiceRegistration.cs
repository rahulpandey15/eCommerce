using eCommerce.Application.Contracts;
using eCommerce.Application.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Application
{
    public static class ServiceRegistration
    {

        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }


    }
}
