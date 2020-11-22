using Net5Template.Core.Bus.RabbitMQ;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Bus.RabbitMQ
{
    public class MessagePublish : IMessagePublish
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagePublish(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public Task Publish<T>(T message) where T : class
        {
            return _publishEndpoint.Publish<T>(message);
        }
    }
}
