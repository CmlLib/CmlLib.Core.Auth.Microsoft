using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class JsonFileCacheManager<T> : ICacheManager<T> where T : class
    {
        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public string CacheFilePath { get; private set; }

        public JsonFileCacheManager(string filepath)
        {
            this.CacheFilePath = filepath;
        }

        private T? GetDefaultObject() => default(T);

        public async virtual Task<T?> ReadCache()
        {
            if (!File.Exists(CacheFilePath))
                return GetDefaultObject();

            try
            {
                using var file = File.OpenRead(CacheFilePath);
                return await JsonSerializer.DeserializeAsync<T>(file);
            }
            catch
            {
                return GetDefaultObject();
            }
        }

        public async virtual Task SaveCache(T? obj)
        {
            var dirPath = Path.GetDirectoryName(CacheFilePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            using var file = File.Create(CacheFilePath);
            await JsonSerializer.SerializeAsync(file, obj, options: jsonOptions);
        }

        public virtual Task ClearCache()
        {
            if (File.Exists(CacheFilePath))
                File.Delete(CacheFilePath);

            return Task.CompletedTask;
        }
    }
}
