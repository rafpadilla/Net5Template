//using Net5Template.Core.Repository;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using System.Linq;
//using Net5Template.Core.Domain;
//using Net5Template.Infrastructure.DataContext;
//using AutoMapper;

//namespace Net5Template.Infrastructure.Persistence.Repository
//{
//    public class RepositoryCachedMongo<T> : RepositoryMongo<T>, IRepositoryCached<T>
//        where T : class, IEntity, ICacheableEntity
//    {
//        private readonly IMemoryCache _memoryCache;
//        private readonly IServiceProvider _serviceProvider;

//        public RepositoryCachedMongo(IDataContextMongo context, IMemoryCache memoryCache, IServiceProvider serviceProvider, IMapper mapper)
//            : base(context, mapper)
//        {
//            _memoryCache = memoryCache;
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<IEnumerable<T>> GetCached()
//        {
//            //Logger.LogInformation($"Try get cached entity: {nameof(T)}");
//            if (!_memoryCache.TryGetValue(typeof(T).Name, out IEnumerable<T> cacheValue))
//            {
//                var repo = _serviceProvider.GetService<IRepositoryCached<T>>();
//                cacheValue = await repo.GetAll();

//                var cacheEntryOptions = new MemoryCacheEntryOptions()
//                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));

//                _memoryCache.Set(typeof(T).Name, cacheValue, cacheEntryOptions);

//                //Logger.Information($"Entity: {nameof(T)} successfully cached");
//            }
//            return cacheValue;
//        }
//    }
//}
