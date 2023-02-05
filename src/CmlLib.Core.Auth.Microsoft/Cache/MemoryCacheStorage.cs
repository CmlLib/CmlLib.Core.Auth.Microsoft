
namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class MemoryCacheStorage<T> : ICacheStorage<T> where T : class
    {
        T? cache;

        public T? Get()
        {
            return cache;
        }

        public void Set(T? value)
        {
            cache = value;
        }

        public void Clear()
        {
            cache = null;
        }
    }
}