using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Bus.RabbitMQ
{
    public interface IMessageConsumer<TMessage> where TMessage : class
    {
        Task Consume(IMessageContext<TMessage> context);
    }
}
