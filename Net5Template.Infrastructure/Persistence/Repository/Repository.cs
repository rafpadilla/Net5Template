using Net5Template.Core.Domain;
using Net5Template.Core.Repository;
using Net5Template.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Persistence.Repository
{
    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
        where TEntity : class, IEntity
    {
        public Repository(IDataContext context)
            : base(context)
        {
        }
    }
    public class Repository<TEntity, TKeyEntity> : IRepository<TEntity, TKeyEntity>
    where TEntity : class, IEntity<TKeyEntity>
    where TKeyEntity : struct
    {
        protected readonly IDataContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(IDataContext context)
        {
            _context = context;
            if (context is DbContext dbContext)
            {
                _dbSet = dbContext.Set<TEntity>();
            }
        }
        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll(int pageIndex = 0, int pageSize = 20)
        {
            return await _dbSet.AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        public virtual async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
        public virtual async Task Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
        public virtual Task<TEntity> GetById(TKeyEntity Id)
        {
            return _dbSet.FirstOrDefaultAsync(a => a.Id.Equals(Id));
        }
        public Task<int> GetAllCount()
        {
            return _dbSet.AsNoTracking().CountAsync();
        }
    }
}
