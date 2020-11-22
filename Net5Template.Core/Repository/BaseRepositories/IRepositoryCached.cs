using Net5Template.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Repository
{
    public interface IRepositoryCached<TEntity> : IRepositoryCached<TEntity, Guid>
        where TEntity : class, IEntity, ICacheableEntity
    {
    }
    public interface IRepositoryCached<TEntity, TKeyEntity> : IRepository<TEntity, TKeyEntity>
        where TEntity : class, IEntity<TKeyEntity>, ICacheableEntity
        where TKeyEntity : struct
    {
        Task<IEnumerable<TEntity>> GetCached();
    }
}
