using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public class JsonFileSessionStorage : ISessionStorage, IDisposable
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;
        private JsonDocument? jsonDocument;

        // Prevent to store null. Casting from null to primitive type is not allowed.
        private readonly Dictionary<string, JsonElement> _map; 
        private bool isLoaded = false;

        public JsonFileSessionStorage(string filepath)
         : this(filepath, JsonSerializerOptions.Default)
        {

        }

        public JsonFileSessionStorage(string filepath, JsonSerializerOptions options)
        {
            this._filePath = filepath;
            this._jsonOptions = options;
            this._map = new Dictionary<string, JsonElement>();
        }

        public async ValueTask<T?> GetAsync<T>(string key)
        {
            if (!isLoaded)
            {
                await loadFromJson();
                mapElements();
            }

            var value = getData<T>(key);
            return value;
        }

        private async Task loadFromJson()
        {
            if (!File.Exists(_filePath))
                return;

            if (jsonDocument != null)
                jsonDocument.Dispose();

            await using var fileStream = File.OpenRead(_filePath);
            jsonDocument = await JsonDocument.ParseAsync(fileStream);
        }

        private void mapElements()
        {
            if (jsonDocument == null)
                throw new InvalidOperationException("loadFromJson first");

            _map.Clear();
            
            var root = jsonDocument.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return;
            
            
            foreach (var kv in root.EnumerateObject())
            {
                _map[kv.Name] = kv.Value;
            }
        }

        private T? getData<T>(string key)
        {
            if (_map.TryGetValue(key, out var element))
            {
                return element.Deserialize<T>();
            }
            else
            {
                return default(T);
            }
        }

        public async ValueTask SetAsync<T>(string key, T? obj)
        {
            setData<T>(key, obj);
            await saveToJson();
        }

        private void setData<T>(string key, T? obj)
        {
            if (obj == null)
            {
                _map.Remove(key);
            }
            else
            {
                _map[key] = JsonSerializer.SerializeToElement<T>(obj, _jsonOptions);
            }
        }

        private async Task saveToJson()
        {
            await using var fileStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(fileStream, _map, _jsonOptions);
        }

        public void Dispose()
        {
            if (jsonDocument != null)
                jsonDocument.Dispose();
        }
    }
}