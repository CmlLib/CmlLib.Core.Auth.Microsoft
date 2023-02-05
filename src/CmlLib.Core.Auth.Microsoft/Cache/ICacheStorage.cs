namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public interface ICacheStorage<T> where T : class
    {
        T? Get();
        void Set(T? value);
        void Clear();
    }
}