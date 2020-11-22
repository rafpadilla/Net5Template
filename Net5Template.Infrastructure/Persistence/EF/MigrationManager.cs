using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Persistence.EF
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host, IWebHostEnvironment env, IConfiguration configuration, ILogger logger, string[] args)
        {
            logger.LogInformation("Check if --apply-migrations param or environment APPLY_MIGRATIONS is present for apply migrations on startup");
            if (args != null && args.Contains("--apply-migrations") || Environment.GetEnvironmentVariable("APPLY_MIGRATIONS") == "true")
            {
                logger.LogInformation("param --apply-migrations or environment APPLY_MIGRATIONS was found, applying migration");

                if (/*_environment.IsDevelopment() &&*/configuration["AppSettings:UseDatabase"].Equals("sqlserver"))
                {
                    using var scope = host.Services.CreateScope();
                    try
                    {
                        if (configuration.GetValue<bool>("AppSettings:UseEFIdentity"))
                        {
                            using var appContext = scope.ServiceProvider.GetRequiredService<Net5TemplateIdentityContext>();
                            appContext.Database.Migrate();
                        }
                        else
                        {
                            using var appContext = scope.ServiceProvider.GetRequiredService<Net5TemplateContext>();
                            appContext.Database.Migrate();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred configuring the DB.");
                    }
                }
            }
            else
                logger.LogInformation("param --apply-migrations or environment APPLY_MIGRATIONS was not found");

            return host;
        }
        public static IHost AddDevelopmentUsers(this IHost host, IWebHostEnvironment env, IConfiguration configuration, ILogger logger, string[] args)
        {
            return host;
        }
    }
}
