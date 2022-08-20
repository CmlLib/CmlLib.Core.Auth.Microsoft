using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class JsonFileCacheManager<T> : ICacheManager<T> where T : class
    {
        public string CacheFilePath { get; private set; }

        public JsonFileCacheManager(string filepath)
        {
            this.CacheFilePath = filepath;
        }

        private T? GetDefaultObject() => default(T);

        public virtual Task<T?> ReadCache()
        {
            if (!File.Exists(CacheFilePath))
                return Task.FromResult(GetDefaultObject());

            try
            {
                string filecontent = File.ReadAllText(CacheFilePath);
                return Task.FromResult(JsonSerializer.Deserialize<T>(filecontent));
            }
            catch
            {
                return Task.FromResult(GetDefaultObject());
            }
        }

        public virtual Task SaveCache(T? obj)
        {
            var dirPath = Path.GetDirectoryName(CacheFilePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);
            File.WriteAllText(CacheFilePath, JsonSerializer.Serialize(obj));

            return Task.CompletedTask;
        }

        public virtual Task ClearCache()
        {
            if (File.Exists(CacheFilePath))
                File.Delete(CacheFilePath);

            return Task.CompletedTask;
        }
    }
}
