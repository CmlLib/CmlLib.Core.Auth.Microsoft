using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public interface ICacheManager<T> where T : class
    {
        Task<T?> ReadCache();
        Task SaveCache(T? obj);
        Task ClearCache();
    }
}
