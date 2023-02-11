using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public interface ISessionStorage
    {
        ValueTask<T?> GetAsync<T>(string key) where T : class;
        ValueTask SetAsync<T>(string key, T? obj) where T : class;
    }
}