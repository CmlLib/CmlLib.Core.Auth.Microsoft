using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public interface ISessionStorage
    {
        ValueTask<T?> GetAsync<T>(string key);
        ValueTask SetAsync<T>(string key, T? obj);
        ValueTask Clear();
    }
}