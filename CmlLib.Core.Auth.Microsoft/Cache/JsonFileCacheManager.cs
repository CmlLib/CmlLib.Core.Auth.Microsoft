using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class JsonFileCacheManager<T> : ICacheManager<T>
    {
        public string CacheFilePath { get; private set; }

        public JsonFileCacheManager(string filepath)
        {
            this.CacheFilePath = filepath;
        }

        public virtual T GetDefaultObject() => default(T);

        public virtual T ReadCache()
        {
            if (!File.Exists(CacheFilePath))
                return GetDefaultObject();

            try
            {
                string filecontent = File.ReadAllText(CacheFilePath);
                return JsonConvert.DeserializeObject<T>(filecontent);
            }
            catch
            {
                return GetDefaultObject();
            }
        }

        public virtual void SaveCache(T obj)
        {
            try
            {
                File.WriteAllText(CacheFilePath, JsonConvert.SerializeObject(obj));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
