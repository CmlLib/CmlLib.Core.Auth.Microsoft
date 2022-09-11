using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class InMemoryCacheManger<T> : ICacheManager<T> where T : class
    {
        private T? cache;

        public Task ClearCache()
        {
            cache = null;
            return Task.CompletedTask;
        }

        public Task<T?> ReadCache()
        {
            return Task.FromResult(cache);
        }

        public Task SaveCache(T? obj)
        {
            cache = obj;
            return Task.CompletedTask;
        }
    }
}
