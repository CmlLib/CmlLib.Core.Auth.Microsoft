using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.SessionStorages
{
    public class JsonFileSessionStorage : ISessionStorage
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly KeyModeStorage _keyModeStorage = new();

    private JsonSessionStorage? _innerStorage;
    public IEnumerable<string> Keys => _innerStorage?.Keys ?? Enumerable.Empty<string>();

    public JsonFileSessionStorage(string filePath) : this(filePath, JsonSerializerOptions.Default) {}

    public JsonFileSessionStorage(string filePath, JsonSerializerOptions jsonSerializerOptions)
    {
        this._filePath = filePath;
        this._jsonOptions = jsonSerializerOptions;
    }

    public T Get<T>(string key)
    {
        return getStorage().Get<T>(key);
    }

    public T GetOrDefault<T>(string key, T defaultValue)
    {
        if (TryGetValue<T>(key, out var value))
            return value;
        else
            return defaultValue;
    }

    public bool TryGetValue<T>(string key, out T value)
        => getStorage().TryGetValue(key, out value);

    public void Set<T>(string key, T obj)
    {
        getStorage().Set<T>(key, obj);
        saveStorage();
    }

    public bool ContainsKey<T>(string key)
    {
        return getStorage().ContainsKey<T>(key);
    }

    public bool ContainsKey(string key)
    {
        return getStorage().ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return getStorage().Remove(key);
    }

    public void Clear()
    {
        getStorage().Clear();
        saveStorage();
    }

    private JsonSessionStorage getStorage()
    {
        if (_innerStorage == null)
            loadJson();
        return _innerStorage!;
    }

    private void loadJson()
    {
        JsonObject? jsonObject = null;
        _innerStorage = null;

        if (File.Exists(_filePath))
        {
            try
            {
                using var fs = File.OpenRead(_filePath);
                jsonObject = JsonNode.Parse(fs) as JsonObject;
            }
            catch (JsonException) // reset storage if json file is corrupted
            {
                jsonObject = new JsonObject();
            }
        }

        if (jsonObject == null)
            jsonObject = new JsonObject();

        _innerStorage = new JsonSessionStorage(jsonObject, _jsonOptions);
    }

    private void saveStorage()
    {
        var dirPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dirPath))
            Directory.CreateDirectory(dirPath);

            var json = getStorage().ToJsonObjectForStoring();
            using var fs = File.Create(_filePath);
            using var jsonWriter = new Utf8JsonWriter(fs);
            json.WriteTo(jsonWriter, _jsonOptions);
        }

        public SessionStorageKeyMode GetKeyMode(string key) =>
            _keyModeStorage.Get(key);

        public void SetKeyMode(string key, SessionStorageKeyMode mode) =>
            _keyModeStorage.Set(key, mode);
    }
}