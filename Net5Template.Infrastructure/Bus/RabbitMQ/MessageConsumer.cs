using Net5Template.Core.Bus.RabbitMQ;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Bus.RabbitMQ
{
    public abstract class MessageConsumer<TMessage> : IMessageConsumer<TMessage>, IConsumer<TMessage> where TMessage : class
    {
        public abstract Task Consume(IMessageContext<TMessage> context);

        public Task Consume(ConsumeContext<TMessage> context)
        {
            return Consume(new MessageContext<TMessage>(context));
        }
    }
}
