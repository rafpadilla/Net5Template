using Net5Template.Core.Bus.RabbitMQ;
using Net5Template.Core.MessageContracts;
using Net5Template.Infrastructure.Bus.RabbitMQ;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5Template.WebAPI.EventConsumers
{
    public class RecipeWasChangedConsumer : MessageConsumer<RecipeWasChangedV1>
    {
        private readonly ILogger<RecipeWasChangedConsumer> _logger;

        public RecipeWasChangedConsumer(ILogger<RecipeWasChangedConsumer> logger)
        {
            _logger = logger;
        }

        public override Task Consume(IMessageContext<RecipeWasChangedV1> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.RecipeName);
            return Task.CompletedTask;
        }
    }
}
