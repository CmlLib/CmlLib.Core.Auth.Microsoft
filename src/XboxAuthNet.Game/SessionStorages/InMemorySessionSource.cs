namespace XboxAuthNet.Game.SessionStorages;

public class InMemorySessionSource<T> : ISessionSource<T>
{
    T? innerObj;
    SessionStorageKeyMode keyMode = SessionStorageKeyMode.Default;

    public InMemorySessionSource(T obj)
    {
        innerObj = obj;
    }

    public void Clear(ISessionStorage sessionStorage) => 
        innerObj = default!;

    public T? Get(ISessionStorage sessionStorage) => innerObj;

    public SessionStorageKeyMode GetKeyMode(ISessionStorage sessionStorage) => keyMode;

    public void Set(ISessionStorage sessionStorage, T? obj) =>
        innerObj = obj;

    public void SetKeyMode(ISessionStorage sessionStorage, SessionStorageKeyMode mode) => keyMode = mode;
}