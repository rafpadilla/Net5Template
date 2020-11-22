using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Domain
{
    public interface IDataCacheService
    {
        Task<IEnumerable<T>> GetCachedItem<T>() where T : class, IEntity, ICacheableEntity;
        Task<T> GetCachedItemAsync<T>(CacheKeys key) where T : class;
    }
}
