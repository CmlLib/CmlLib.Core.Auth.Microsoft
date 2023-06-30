namespace XboxAuthNet.Game.SessionStorages;

public interface ISessionStorage
{
    IEnumerable<string> Keys { get; }
    T Get<T>(string key);
    T GetOrDefault<T>(string key, T defaultValue);
    bool TryGetValue<T>(string key, out T value);
    void Set<T>(string key, T obj);
    SessionStorageKeyMode GetKeyMode(string key);
    void SetKeyMode(string key, SessionStorageKeyMode mode);
    bool Remove(string key);
    bool ContainsKey(string key);
    bool ContainsKey<T>(string key);
}