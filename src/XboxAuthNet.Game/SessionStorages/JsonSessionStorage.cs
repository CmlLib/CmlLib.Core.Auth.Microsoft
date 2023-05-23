using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.SessionStorages
{
    public class JsonSessionStorage : ISessionStorage
    {
        public static JsonSessionStorage CreateEmpty(JsonSerializerOptions? jsonOptions = default)
        {
            return new JsonSessionStorage(new JsonObject(), jsonOptions);
        }

        private HashSet<string> _keys = new();
        private readonly InMemorySessionStorage _cache = new();
        private readonly JsonObject _jsonObject;
        private readonly JsonSerializerOptions? _jsonOptions;

        public IEnumerable<string> Keys => _keys;

        public JsonSessionStorage(JsonObject json, JsonSerializerOptions? jsonSerializerOptions = default)
        {
            this._jsonOptions = jsonSerializerOptions;
            this._jsonObject = json;

            foreach (var kv in json)
            {
                _keys.Add(kv.Key);
            }
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            if (_cache.TryGetValue(key, out value))
            {
                return true;
            }
            else
            { 
                return cacheJson(key, out value);
            }
        }

        public T? Get<T>(string key)
        {
            var result = TryGetValue<T>(key, out var value);
            if (!result)
                throw new KeyNotFoundException(key);
            return value;
        }

        public T? GetOrDefault<T>(string key, T? defaultValue)
        {
            if (TryGetValue<T>(key, out var value))
                return value;
            else
                return defaultValue;
        }

        private bool cacheJson<T>(string key, out T? value)
        {
            var result = getFromJson(key, out value);
            if (result)
                _cache.Set(key, value);
            return result;
        }

        private bool getFromJson<T>(string key, out T? value)
        {
            if (_jsonObject.ContainsKey(key))
            {
                value = _jsonObject[key].Deserialize<T?>(_jsonOptions);
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void Set<T>(string key, T obj)
        {
            _cache.Set(key, obj);
            _keys.Add(key);
        }

        public bool ContainsKey(string key) => _cache.ContainsKey(key);

        public bool ContainsKey<T>(string key)
        {
            if (_cache.ContainsKey<T>(key))
            {
                return true;
            }
            else
            {
                if (_jsonObject.ContainsKey(key))
                {
                    cacheJson<T>(key, out var _);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Remove(string key)
        {
            _keys.Remove(key);
            return _cache.Remove(key);
        }

        public void Clear()
        {
            _keys.Clear();
            _cache.Clear();
        }

        public JsonObject ToJsonObject()
        {
            var json = new JsonObject();
            foreach (var key in Keys)
            {
                var value = JsonSerializer.SerializeToNode(Get<object>(key));
                json.Add(key, value);
            }
            return json;
        }
    }
}