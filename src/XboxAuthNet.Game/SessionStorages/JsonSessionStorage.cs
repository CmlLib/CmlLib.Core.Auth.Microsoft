using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.SessionStorages;

public class JsonSessionStorage : ISessionStorage
{
    public static JsonSessionStorage CreateEmpty(JsonSerializerOptions? jsonOptions = default)
    {
        return new JsonSessionStorage(new JsonObject(), jsonOptions);
    }

    private HashSet<string> _keys = new();
    private readonly KeyModeStorage _keyModeStorage = new();
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

    public bool TryGetValue<T>(string key, out T value)
    {
        if (_cache.TryGetValue(key, out value))
        {
            return true;
        }
        else if (Keys.Contains(key))
        { 
            return cacheJson(key, out value);
        }
        else
        {
            return false;
        }
    }

    public T Get<T>(string key)
    {
        var result = TryGetValue<T>(key, out var value);
        if (!result)
            throw new KeyNotFoundException(key);
        return value;
    }

    public T GetOrDefault<T>(string key, T defaultValue)
    {
        if (TryGetValue<T>(key, out var value))
            return value;
        else
            return defaultValue;
    }

    private bool cacheJson<T>(string key, out T value)
    {
        var result = getFromJson(key, out value);
        if (result)
            _cache.Set(key, value);
        return result;
    }

    private bool getFromJson<T>(string key, out T value)
    {
        if (_jsonObject.ContainsKey(key))
        {
            var jsonValue = _jsonObject[key].Deserialize<T>(_jsonOptions);
            if (jsonValue == null)
            {
                value = default!;
                return false;
            }
            else
            {
                value = jsonValue;
                return true;
            }
        }
        else
        {
            value = default!;
            return false;
        }
    }

    public void Set<T>(string key, T obj)
    {
        _cache.Set(key, obj);
        if (EqualityComparer<T>.Default.Equals(obj, default))
            _keys.Remove(key);
        else
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

    public JsonObject ToJsonObject() =>
        ToJsonObjectByKeys(Keys);

    public JsonObject ToJsonObjectForStoring() => 
        ToJsonObjectByKeys(this.GetKeysForStoring());

    public JsonObject ToJsonObjectByKeys(IEnumerable<string> keys)
    {
        var json = new JsonObject();
        foreach (var key in keys)
        {
            var result = TryGetValue<object>(key, out var value);
            if (!result)
                continue;
            var node = JsonSerializer.SerializeToNode(value);
            json.Add(key, node);
        }
        return json;
    }

    public SessionStorageKeyMode GetKeyMode(string key) => 
        _keyModeStorage.Get(key);

    public void SetKeyMode(string key, SessionStorageKeyMode mode) =>
        _keyModeStorage.Set(key, mode);
}