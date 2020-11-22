using Net5Template.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Repository
{
    public interface IRepositoryValueObject<TEntity> where TEntity : IValueObject
    {
    }
}
