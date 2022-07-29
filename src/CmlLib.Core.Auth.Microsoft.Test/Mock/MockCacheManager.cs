using CmlLib.Core.Auth.Microsoft.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockCacheManager<T> : ICacheManager<T>
    {
        public T Cache { get; set; }

        public T GetDefaultObject()
        {
            return default(T);
        }

        public T ReadCache()
        {
            return Cache;
        }

        public void SaveCache(T obj)
        {
            Cache = obj;
        }
    }
}
