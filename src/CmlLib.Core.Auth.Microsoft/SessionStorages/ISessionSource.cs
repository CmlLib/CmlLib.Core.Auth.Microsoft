using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public interface ISessionSource<T> where T : class
    {
        ValueTask<T?> GetAsync();
        ValueTask SetAsync(T? obj);
    }
}