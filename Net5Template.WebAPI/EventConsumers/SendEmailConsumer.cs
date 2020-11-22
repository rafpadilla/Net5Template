using Net5Template.Core.Bus.RabbitMQ;
using Net5Template.Core.MessageContracts;
using Net5Template.Infrastructure.Bus.RabbitMQ;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Net5Template.Core.Bus;
using Net5Template.Application.Services.Email.Commands;

namespace Net5Template.WebAPI.EventConsumers
{
    public class SendEmailConsumer : MessageConsumer<SendEmailV1>
    {
        private readonly ILogger<SendEmailConsumer> _logger;
        private readonly ICommandBus _commandBus;

        public SendEmailConsumer(ILogger<SendEmailConsumer> logger, ICommandBus commandBus)
        {
            _logger = logger;
            _commandBus = commandBus;
        }

        public override Task Consume(IMessageContext<SendEmailV1> context)
        {
            _logger.LogInformation($"SendEmail to: {context.Message.ToEmail}");
            return _commandBus.Send(new SendEmailCommand(context.Message.ToEmail, context.Message.ToName, context.Message.Subject, context.Message.HtmlBody));
        }
    }
}
