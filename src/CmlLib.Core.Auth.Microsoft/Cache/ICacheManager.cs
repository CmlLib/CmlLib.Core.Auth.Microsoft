namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public interface ICacheManager<T> where T : class
    {
        T? ReadCache();
        void SaveCache(T? obj);
    }
}
