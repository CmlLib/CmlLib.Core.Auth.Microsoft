namespace CmlLib.Core.Auth.Microsoft
{
    public interface ICacheManager<T>
    {
        T GetDefaultObject();
        T ReadCache();
        void SaveCache(T obj);
    }
}
