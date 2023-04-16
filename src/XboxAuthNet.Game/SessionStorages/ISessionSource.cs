namespace XboxAuthNet.Game.SessionStorages
{
    public interface ISessionSource<T>
    {
        T? Get();
        void Set(T? obj);
        void Clear();
    }
}