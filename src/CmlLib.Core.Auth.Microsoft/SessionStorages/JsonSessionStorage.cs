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
        private Dictionary<string, JsonElement>? documentMap;

        public JsonFileSessionStorage(string filepath)
         : this(filepath, JsonSerializerOptions.Default)
        {

        }

        public JsonFileSessionStorage(string filepath, JsonSerializerOptions options)
        {
            this._filePath = filepath;
            this._jsonOptions = options;
        }

        public async ValueTask<T?> GetAsync<T>(string key)
        {
            var value = await getData<T>(key);
            return value;
        }

        private async ValueTask<T?> getData<T>(string key)
        {
            var map = await getMap();
            if (map.TryGetValue(key, out var element))
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
            await setData<T>(key, obj);
            await saveToJson();
        }

        private async ValueTask setData<T>(string key, T? obj)
        {
            var map = await getMap();
            if (obj == null)
            {
                map.Remove(key);
            }
            else
            {
                map[key] = JsonSerializer.SerializeToElement<T>(obj, _jsonOptions);
            }
        }

        private async Task saveToJson()
        {
            var dirPath = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);
                
            var map = await getMap();
            await using var fileStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(fileStream, map, _jsonOptions);
        }

        private async ValueTask<Dictionary<string, JsonElement>> getMap()
        {
            if (documentMap == null)
            {
                await loadJsonFile();
                mapElements();
            }

            return documentMap;
        }

        private async Task loadJsonFile()
        {
            if (jsonDocument != null)
                jsonDocument.Dispose();

            if (File.Exists(_filePath))
            {
                await using var fileStream = File.OpenRead(_filePath);
                jsonDocument = await JsonDocument.ParseAsync(fileStream);
            }
            else
            {
                jsonDocument = JsonDocument.Parse("{}");
            }
        }

        private void mapElements()
        {
            if (jsonDocument == null)
                throw new InvalidOperationException("loadFromJson first");

            if (documentMap == null)    
                documentMap = new Dictionary<string, JsonElement>();
            else
                documentMap.Clear();
            
            var root = jsonDocument.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return;
            
            foreach (var kv in root.EnumerateObject())
            {
                documentMap[kv.Name] = kv.Value;
            }
        }

        public void Dispose()
        {
            if (jsonDocument != null)
                jsonDocument.Dispose();
        }
    }
}