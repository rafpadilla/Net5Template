using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Bus.RabbitMQ
{
    public interface IMessagePublish
    {
        public Task Publish<T>(T message) where T : class;
    }
}
