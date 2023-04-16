namespace XboxAuthNet.Game.SessionStorages
{
    public interface ISessionStorage
    {
        T? Get<T>(string key);
        void Set<T>(string key, T? obj);
        void Clear();
    }
}