using System.Threading.Tasks;
using System.Collections.Generic;

namespace XboxAuthNet.Game.SessionStorages
{
    public class InMemorySessionStorage : ISessionStorage
    {
        private readonly Dictionary<string, object?> _storage = new Dictionary<string, object?>();

        public T? Get<T>(string key) => 
            get<T>(key);

        private T? get<T>(string key)
        {
            if (_storage.TryGetValue(key, out var obj))
                return (T?)obj;
            else
                throw new KeyNotFoundException();
        }

        public void Set<T>(string key, T? obj) => 
            set<T>(key, obj);

        private void set<T>(string key, T? obj) =>
            _storage[key] = obj;

        public void Clear() =>
            _storage.Clear();
    }
}