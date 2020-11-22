using Net5Template.Core.Repository;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Net5Template.Core.Domain;
using Net5Template.Infrastructure.DataContext;

namespace Net5Template.Infrastructure.Persistence.Repository
{
    public class RepositoryCached<T> : RepositoryCached<T, Guid>, IRepositoryCached<T>
        where T : class, IEntity, ICacheableEntity
    {
        public RepositoryCached(IDataContext context, IMemoryCache memoryCache, IServiceProvider serviceProvider)
            : base(context, memoryCache, serviceProvider)
        {
        }
    }
    public class RepositoryCached<T, TKeyEntity> : Repository<T, TKeyEntity>, IRepositoryCached<T, TKeyEntity>
        where T : class, IEntity<TKeyEntity>, ICacheableEntity
        where TKeyEntity : struct
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;

        public RepositoryCached(IDataContext context, IMemoryCache memoryCache, IServiceProvider serviceProvider)
            : base(context)
        {
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<T>> GetCached()
        {
            //Logger.LogInformation($"Try get cached entity: {nameof(T)}");
            if (!_memoryCache.TryGetValue(typeof(T).Name, out IEnumerable<T> cacheValue))
            {
                var repo = _serviceProvider.GetService<IRepositoryCached<T, TKeyEntity>>();
                cacheValue = await repo.GetAll(pageSize: int.MaxValue);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                _memoryCache.Set(typeof(T).Name, cacheValue, cacheEntryOptions);

                //Logger.Information($"Entity: {nameof(T)} successfully cached");
            }
            return cacheValue;
        }
    }
}
