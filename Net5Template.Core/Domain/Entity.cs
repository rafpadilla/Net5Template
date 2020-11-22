using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Domain
{
    public abstract class Entity : Entity<Guid>, IEntity
    {
    }
    public abstract class Entity<T> : IEntity<T> where T : struct
    {
        public virtual T Id { get; set; }

        public List<DomainEvent> DomainEvents = new List<DomainEvent>();
    }
}
