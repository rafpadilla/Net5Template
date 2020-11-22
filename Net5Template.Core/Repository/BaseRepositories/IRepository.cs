using Net5Template.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Repository
{
    public interface IRepository<TEntity> : IRepository<TEntity, Guid>
        where TEntity : class, IEntity
    {
    }
    public interface IRepository<TEntity, TKeyEntity>
        where TEntity : class, IEntity<TKeyEntity>
        where TKeyEntity : struct
    {
        Task<IEnumerable<TEntity>> GetAll(int pageIndex = 0, int pageSize = 20);
        Task<int> GetAllCount();
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task Update(TEntity entity);
        Task UpdateRange(IEnumerable<TEntity> entities);
        Task Remove(TEntity entity);
        Task RemoveRange(IEnumerable<TEntity> entities);
        Task<TEntity> GetById(TKeyEntity Id);
    }
}
