using FNS.HealthCheck.Hosting;
using FNS.HealthCheck.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FNS.HealthCheck.Configuration
{
    public static class HealthCheckApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app)
        {
            app.UseMiddleware<HealthCheckMiddleware>();

            return app;
        }

        public static void AddSqlConnectionHealthCheck(this IServiceCollection services)
        {
            services.AddTransient<IHealthCheckService, SqlConnectionHealthCheck>();            
        }

        public static void AddNoOpHealthCheck(this IServiceCollection services)
        {
            services.AddTransient<IHealthCheckService, NoOpHealthCheck>();
        }
    }
}
