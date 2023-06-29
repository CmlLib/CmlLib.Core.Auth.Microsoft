using System.Collections.Generic;

namespace XboxAuthNet.Game.SessionStorages
{
    public class InMemorySessionStorage : ISessionStorage
    {
        private readonly KeyModeStorage _keyModeStorage = new();
        private readonly Dictionary<string, object?> _storage = new();

        public IEnumerable<string> Keys => _storage.Keys;
        public int Count => _storage.Count;

        public T? Get<T>(string key) => 
            get<T>(key);

        private T? get<T>(string key)
            => (T?)_storage[key];

        public T? GetOrDefault<T>(string key, T? defaultValue)
        {
            if (TryGetValue<T>(key, out var value))
                return value;
            else
                return defaultValue;
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            var result = _storage.TryGetValue(key, out object? objValue);
            if (!result)
            {
                value = default;
                return false;
            }
            else if (objValue is not T)
            {
                value = default;
                return false;
            }
            else
            {
                value = (T)objValue;
                return true;
            }
        }

        public void Set<T>(string key, T obj) => 
            set(key, obj);

        private void set<T>(string key, T obj) =>
            _storage[key] = obj;

        public bool ContainsKey(string key) =>
            _storage.ContainsKey(key);

        public bool ContainsKey<T>(string key)
        {
            var contains = _storage.TryGetValue(key, out var value);
            return contains && value is T;
        }

        public bool Remove(string key) =>
            _storage.Remove(key);

        public void Clear() =>
            _storage.Clear();

        public SessionStorageKeyMode GetKeyMode(string key) => 
            _keyModeStorage.Get(key);

        public void SetKeyMode(string key, SessionStorageKeyMode mode) => 
            _keyModeStorage.Set(key, mode);
    }
}