
using eCommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration config,string fileName) where TContext : DbContext
        {
            //Add Generic Database Context
            services.AddDbContext<TContext>(options => options.UseSqlServer(
                config.GetConnectionString("eCommerceConnection"), sqlserverOption => sqlserverOption.EnableRetryOnFailure()    
            ));

            //Configure serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLinse}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //Add Jwt authentication scheme
            JWTAuthenticationScheme.AddJwtAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //use global exception
            app.UseMiddleware<GlobalException>();

            //register middleware to block all outsiders api calls
            //app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
