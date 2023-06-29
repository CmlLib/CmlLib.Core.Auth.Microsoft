namespace XboxAuthNet.Game.SessionStorages;

public class KeyModeStorage
{
    private readonly Dictionary<string, SessionStorageKeyMode> _storage = new();

    public SessionStorageKeyMode Get(string key)
    {
        var result = _storage.TryGetValue(key, out var keyMode);
        if (result)
            return keyMode;
        else
            return SessionStorageKeyMode.Default;
    }

    public void Set(string key, SessionStorageKeyMode value) 
    {
        _storage[key] = value;
    }
}
