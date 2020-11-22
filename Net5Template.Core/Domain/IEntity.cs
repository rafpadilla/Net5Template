using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Domain
{
    public interface IEntity : IEntity<Guid>
    {
    }
    public interface IEntity<T> where T : struct
    {
        T Id { get; set; }
    }
}
