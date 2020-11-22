using Net5Template.Core.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.Controllers
{
    public class Net5TemplateControllerBase : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly ICommandBus _commandBus;
        protected readonly IQueryBus _queryBus;

        public Net5TemplateControllerBase(ILogger logger, ICommandBus commandBus, IQueryBus queryBus)
        {
            _logger = logger;
            _commandBus = commandBus;
            _queryBus = queryBus;
        }
    }
}
