using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Domain
{
    public class DomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
        public string OperationType { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
    }
}
