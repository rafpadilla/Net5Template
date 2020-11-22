using Net5Template.Core.Domain;
using Net5Template.Core.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Caching
{
    public class DataCacheService : IDataCacheService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;

        public DataCacheService(IServiceProvider serviceProvider, IMemoryCache memoryCache)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
        }
        private T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        public async Task<IEnumerable<T>> GetCachedItem<T>()
            where T : class, IEntity, ICacheableEntity
        {
            if (!_memoryCache.TryGetValue(typeof(T).Name, out IEnumerable<T> value))
            {
                var repo = _serviceProvider.GetService<IRepositoryCached<T>>();
                value = await repo.GetCached();
            }
            return value;
        }
        public async Task<T> GetCachedItemAsync<T>(CacheKeys key) where T : class
        {
            if (!_memoryCache.TryGetValue(key.ToString(), out T value))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));
                value = await GetCachedItemQueryAsync<T>(key);
                if (value != null)
                    _memoryCache.Set(key.ToString(), value, cacheEntryOptions);
            }
            return value;
        }

        private Task<T> GetCachedItemQueryAsync<T>(CacheKeys key) where T : class
        {
            switch (key)
            {
                //case CacheKeys.AllLogs:
                //    return await Resolve<ILogRepository>().GetAll();
                default:
                    return null;
            }
        }
    }
}
