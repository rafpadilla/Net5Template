using Net5Template.WebAPI.HostedServices;
using Net5Template.Infrastructure.Logging;
using Net5Template.Infrastructure.Persistence.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
          .AddEnvironmentVariables()
          .Build();

        public static void Main(string[] args)
        {
            Infrastructure.Logging.LoggerExtensions.ConfigureLogger(Configuration);

            var host = CreateHostBuilder(args).Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            var _env = host.Services.GetRequiredService<IWebHostEnvironment>();

            logger.LogInformation($"Running Environment: {_env.EnvironmentName}");

            logger.LogInformation("Starting web host");

            host.MigrateDatabase(_env, Configuration, logger, args)//migrate on start app on development
                .AddDevelopmentUsers(_env, Configuration, logger, args)//en desarrollog (debug) verificar si existen los ususarios y a�adirlos en tal caso
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog()
                .ConfigureServices(services =>
                {
                    //https://docs.microsoft.com/es-es/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio

                    //execute a task every x time (configurable)
                    #region snippet1
                    services.AddHostedService<TimedHostedService>();
                    #endregion

                    //use a scoped service in a background service (can use DI), stops where cancellation token calls
                    //#region snippet2
                    //services.AddHostedService<ConsumeScopedServiceHostedService>();
                    //services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    //#endregion

                    //Add tasks to queued background, throug DI (IBackgroundTaskQueue)
                    #region snippet3
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
                    #endregion
                });
    }
}
