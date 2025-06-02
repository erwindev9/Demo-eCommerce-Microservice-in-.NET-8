using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //Add Database connecttivity
            //Add Authentication schema
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:Filename"]!);

            //Create dependency injection 
            services.AddScoped<IOrder, OrderRepository>();
            return services;

        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as : 
            //Global exception => handle external errors 
            // ListenToApiGAteway => block all outsiders api calls

            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
