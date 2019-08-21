using HealthCheck.Configuration;
using HealthCheck.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCheck.Hosting
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HealthCheckOptions _options;

        public HealthCheckMiddleware(RequestDelegate next, IOptionsMonitor<HealthCheckOptions> options)
        {
            _next = next;
            _options = options.CurrentValue;
        }

        public async Task Invoke(HttpContext context, IEnumerable<IHealthCheckService> healthCheckServices, ILogger<HealthCheckMiddleware> logger)
        {
            if (string.Equals(context.Request.Path.Value, _options.EndpointPath, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    foreach (var checker in healthCheckServices)
                    {
                        if (!await checker.Check())
                        {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync($"NOK");
                        }
                    }
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync($"OK");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error checking the application.");
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync($"NOK");
                }

                logger.LogDebug($"Request path {context.Request.Path} matches endpoint", context.Request.Path);

                return;
            }
            else
            {
                await _next(context);
            }
        }
    }
}
