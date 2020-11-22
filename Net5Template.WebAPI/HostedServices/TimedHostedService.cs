using Net5Template.Application.HostedServices;
using Net5Template.Core.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.HostedServices
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int _executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private readonly int _secs;
        //private readonly bool _enableGeoIPCrawl;
        private readonly bool _runHostedServices;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _secs = configuration.GetValue<int>("AppSettings:SystemJobIntervalSecs");
            //_enableGeoIPCrawl = configuration.GetValue("AppSettings:TimedHostedServices:EnableGeoIPCrawl", true);
            _runHostedServices = configuration.GetValue("AppSettings:RunHostedServices", true);
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_secs));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);

            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
            try
            {
                if (_runHostedServices)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetService<IBackgroundTaskQueue>();
                    var systemJobHostedService = scope.ServiceProvider.GetService<ISystemJobHostedService>();

                    service.QueueBackgroundWorkItem(cancel => systemJobHostedService.DoSomeStuff(cancel));
                    //if (EnableMailingProcess)
                    //    service.QueueBackgroundWorkItem(cancel => mailingJobAppService.MailingProcess(cancel));
                    //if (EnableGeoIPCrawl)
                    //    service.QueueBackgroundWorkItem(cancel => geoIPJobAppService.GeoIPMappingProcess(cancel));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

            _logger.LogInformation("Timed Hosted Service is working.");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose() => _timer?.Dispose();
    }
}
