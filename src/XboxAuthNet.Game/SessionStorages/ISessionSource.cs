namespace XboxAuthNet.Game.SessionStorages;

public interface ISessionSource<T>
{
    T? Get(ISessionStorage sessionStorage);
    void Set(ISessionStorage sessionStorage, T? obj);
    void Clear(ISessionStorage sessionStorage);
    SessionStorageKeyMode GetKeyMode(ISessionStorage sessionStorage);
    void SetKeyMode(ISessionStorage sessionStorage, SessionStorageKeyMode mode);
}