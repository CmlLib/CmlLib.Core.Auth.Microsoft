using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.SessionStorages
{
    public class JsonFileSessionStorage : ISessionStorage
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        private JsonObject? _object;
        private JsonSessionStorage? _innerStorage;

        public JsonFileSessionStorage(string filePath) : this(filePath, JsonSerializerOptions.Default) {}

        public JsonFileSessionStorage(string filePath, JsonSerializerOptions jsonSerializerOptions)
        {
            this._filePath = filePath;
            this._jsonOptions = jsonSerializerOptions;
        }

        public T? Get<T>(string key)
        {
            return getStorage().Get<T>(key);
        }

        public void Set<T>(string key, T? obj)
        {
            getStorage().Set<T>(key, obj);
            saveStorage();
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
            _object = null;
            _innerStorage = null;

            if (File.Exists(_filePath))
            {
                try
                {
                    using var fs = File.OpenRead(_filePath);
                    _object = JsonNode.Parse(fs) as JsonObject;
                }
                catch (JsonException) // reset storage if json file is corrupted
                {
                    _object = new JsonObject();
                }
            }

            if (_object == null)
                _object = new JsonObject();

            _innerStorage = new JsonSessionStorage(_object, _jsonOptions);
        }

        private void saveStorage()
        {
            if (_object == null)
                return;

            var dirPath = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using var fs = File.Create(_filePath);
            using var jsonWriter = new Utf8JsonWriter(fs);
            _object.WriteTo(jsonWriter, _jsonOptions);
        }
    }
}