using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Application.HostedServices
{
    public abstract class BaseJobHostedService<T>
    {
        protected readonly ILogger<T> _logger;
        protected readonly IServiceScopeFactory _scopeFactory;

        public BaseJobHostedService(ILogger<T> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
    }
}
