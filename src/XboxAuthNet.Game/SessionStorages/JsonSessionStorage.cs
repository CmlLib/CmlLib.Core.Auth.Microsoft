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

        private readonly JsonSerializerOptions? _jsonOptions;
        public JsonObject JsonObject { get; private set; }

        public JsonSessionStorage(JsonObject json, JsonSerializerOptions? jsonSerializerOptions = default)
        {
            JsonObject = json;
            this._jsonOptions = jsonSerializerOptions;
        }

        public T? Get<T>(string key)
        {
            var node = JsonObject[key];

            if (node == null)
                return default(T);
            else
                return node.Deserialize<T>(_jsonOptions);
        }

        public void Set<T>(string key, T? obj)
        {
            if (obj != null)
                JsonObject[key] = JsonSerializer.SerializeToNode<T>(obj, _jsonOptions);
            else
                JsonObject.Remove(key);
        }

        public void Clear()
        {
            JsonObject.Clear();
        }
    }
}