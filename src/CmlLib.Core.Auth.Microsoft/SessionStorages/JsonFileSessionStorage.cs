using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public class JsonFileSessionStorage : ISessionStorage
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        private JsonObject? innerStorage;

        public JsonFileSessionStorage(string filePath) : this(filePath, JsonSerializerOptions.Default) {}

        public JsonFileSessionStorage(string filePath, JsonSerializerOptions jsonSerializerOptions)
        {
            this._filePath = filePath;
            this._jsonOptions = jsonSerializerOptions;
        }

        public ValueTask<T?> GetAsync<T>(string key)
        {
            T? result;

            var json = getJson();
            var node = json[key];

            if (node == null)
                result = default(T);
            else
                result = node.Deserialize<T>();

            return new ValueTask<T?>(result);
        }

        public ValueTask SetAsync<T>(string key, T? obj)
        {
            var json = getJson();

            if (obj != null)
                json[key] = JsonSerializer.SerializeToNode<T>(obj);
            else
                json.Remove(key);
            
            saveJson();
            return new ValueTask();
        }

        public ValueTask Clear()
        {
            getJson().Clear();
            saveJson();
            return new ValueTask();
        }

        private JsonObject getJson()
        {
            if (innerStorage == null)
                loadJson();
            return innerStorage!;
        }

        private void loadJson()
        {
            innerStorage = null;

            if (File.Exists(_filePath))
            {
                try
                {
                    using var fs = File.OpenRead(_filePath);
                    innerStorage = JsonNode.Parse(fs) as JsonObject;
                }
                catch (JsonException) // reset storage if json file is corrupted
                {
                    innerStorage = new JsonObject();
                }
            }

            if (innerStorage == null)
                innerStorage = new JsonObject();
        }

        private void saveJson()
        {
            var dirPath = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using var fs = File.Create(_filePath);
            using var jsonWriter = new Utf8JsonWriter(fs);
            getJson().WriteTo(jsonWriter, _jsonOptions);
        }
    }
}