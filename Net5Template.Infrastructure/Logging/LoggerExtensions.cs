using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net5Template.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        public static void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        public static IHostBuilder UseSerilog(this IHostBuilder builder, ILogger logger = null, bool dispose = false, LoggerProviderCollection providers = null)
        {
            return SerilogHostBuilderExtensions.UseSerilog(builder, logger, dispose, providers);
        }
        public static IHostBuilder UseSerilog(this IHostBuilder builder, Action<HostBuilderContext, LoggerConfiguration> configureLogger, bool preserveStaticLogger = false, bool writeToProviders = false)
        {
            return SerilogHostBuilderExtensions.UseSerilog(builder, configureLogger, preserveStaticLogger, writeToProviders);
        }

        public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app, string messageTemplate)
        {
            return SerilogApplicationBuilderExtensions.UseSerilogRequestLogging(app, messageTemplate);
        }
        public static IApplicationBuilder UseSerilogRequestLogging(this IApplicationBuilder app, Action<RequestLoggingOptions> configureOptions = null)
        {
            return SerilogApplicationBuilderExtensions.UseSerilogRequestLogging(app, configureOptions);
        }
    }
}
