using Net5Template.Core.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Application.HostedServices
{
    public interface ISystemJobHostedService
    {
        Task DoSomeStuff(CancellationToken cancellationToken);
    }
    public class SystemJobHostedService : BaseJobHostedService<SystemJobHostedService>, ISystemJobHostedService
    {
        public SystemJobHostedService(ILogger<SystemJobHostedService> logger, IServiceScopeFactory scopeFactory)
               : base(logger, scopeFactory)
        {
        }

        public async Task DoSomeStuff(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SystemJobHostedService -> Doing SomeStuff");
            //obligatorio crear un nuevo scope y solicitar instancias de los servicios que se necesitarán (esto es un hilo distinto del contexto que llama)
            using var scope = _scopeFactory.CreateScope();
            var commandBus = scope.ServiceProvider.GetService<ICommandBus>();

            // Do any stuff
            await commandBus.Send(new Application.Services.Users.Commands.UserLoginCommand(string.Empty, string.Empty, string.Empty));
        }
    }
}
