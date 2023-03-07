using System.Threading.Tasks;
using System.Collections.Generic;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public class InMemorySessionStorage : ISessionStorage
    {
        private readonly Dictionary<string, object?> _storage = new Dictionary<string, object?>();

        public IEnumerable<string> Keys => _storage.Keys;

        public ValueTask<T?> GetAsync<T>(string key)
        {
            var obj = get<T>(key);
            return new ValueTask<T?>(obj);
        }

        private T? get<T>(string key)
        {
            if (_storage.TryGetValue(key, out var obj))
                return (T?)obj;
            else
                throw new KeyNotFoundException();
        }

        public ValueTask SetAsync<T>(string key, T? obj)
        {
            set<T>(key, obj);
            return new ValueTask();
        }

        private void set<T>(string key, T? obj)
        {
            _storage[key] = obj;
        }
    }
}