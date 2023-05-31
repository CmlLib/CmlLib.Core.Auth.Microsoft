namespace XboxAuthNet.Game.SessionStorages;

public class SessionFromStorage<T> : ISessionSource<T>
{
    private readonly string _keyName;

    public SessionFromStorage(string keyName) =>
        _keyName = keyName;

    public T? Get(ISessionStorage sessionStorage) =>
        sessionStorage.GetOrDefault<T?>(_keyName, default);

    public void Set(ISessionStorage sessionStorage, T? obj) =>
        sessionStorage.Set(_keyName, obj);

    public void Clear(ISessionStorage sessionStorage) =>
        Set(sessionStorage, default);
}