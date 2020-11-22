using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Health
{
    public static class HealthCheckService
    {
        public static IHealthChecksBuilder AddMyHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {

            return services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("sqlserver"));
        }

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder application)
        {

            var healthCheckOptions = new HealthCheckOptions
            {
                ResponseWriter = async (a, b) =>
                {
                    a.Response.ContentType = MediaTypeNames.Application.Json;
                    var result = JsonConvert.SerializeObject(
                       new
                       {
                           checks = b.Entries.Select(e =>
                           new
                           {
                               description = e.Key,
                               status = e.Value.Status.ToString(),
                               responseTime = e.Value.Duration.TotalMilliseconds
                           }),
                           totalResponseTime = b.TotalDuration.TotalMilliseconds,
                           status = b.Entries.Any(a => a.Value.Status != HealthStatus.Healthy) ?
                                b.Entries.Any(a => a.Value.Status == HealthStatus.Unhealthy) ? HealthStatus.Unhealthy.ToString() : HealthStatus.Degraded.ToString()
                            : HealthStatus.Healthy.ToString()
                       });
                    await a.Response.WriteAsync(result);
                }
            };

            return application.UseHealthChecks("/health", healthCheckOptions);
        }
    }
}
