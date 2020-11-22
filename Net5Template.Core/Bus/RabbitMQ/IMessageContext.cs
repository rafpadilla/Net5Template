using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Bus.RabbitMQ
{
    public interface IMessageContext<out T> where T : class
    {
        T Message { get; }
        Task NotifyConsumed(TimeSpan duration, string consumerType);
        Task NotifyFaulted(TimeSpan duration, string consumerType, Exception exception);
    }
}
