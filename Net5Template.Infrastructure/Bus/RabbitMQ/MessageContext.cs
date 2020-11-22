using Net5Template.Core.Bus.RabbitMQ;
using MassTransit;
using MassTransit.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Bus.RabbitMQ
{
    public class MessageContext<TMessage> : IMessageContext<TMessage> where TMessage : class
    {
        private readonly ConsumeContextScope<TMessage> _context;
        internal MessageContext(MassTransit.MessageContext ctx)
        {
            _context = ctx as ConsumeContextScope<TMessage>;
            Message = _context.Message;
            Host = _context.Host;
            Headers = _context.Headers;
            SentTime = _context.SentTime;
            FaultAddress = _context.FaultAddress;
            ResponseAddress = _context.ResponseAddress;
            DestinationAddress = _context.DestinationAddress;
            SourceAddress = _context.SourceAddress;
            ExpirationTime = _context.ExpirationTime;
            InitiatorId = _context.InitiatorId;
            ConversationId = _context.ConversationId;
            CorrelationId = _context.CorrelationId;
            RequestId = _context.RequestId;
            MessageId = _context.MessageId;
        }

        public TMessage Message { get; set; }

        public HostInfo Host { get; set; }
        public Headers Headers { get; set; }
        public DateTime? SentTime { get; set; }
        public Uri FaultAddress { get; set; }
        public Uri ResponseAddress { get; set; }
        public Uri DestinationAddress { get; set; }
        public Uri SourceAddress { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public Guid? InitiatorId { get; set; }
        public Guid? ConversationId { get; set; }
        public Guid? CorrelationId { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? MessageId { get; set; }

        public Task NotifyConsumed(TimeSpan duration, string consumerType)
        {
            return _context.NotifyConsumed(duration, consumerType);
        }

        public Task NotifyFaulted(TimeSpan duration, string consumerType, Exception exception)
        {
            return _context.NotifyFaulted(duration, consumerType, exception);
        }
    }
}
