using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public interface ISessionSource<T>
    {
        ValueTask<T> GetAsync();
        ValueTask SetAsync(T obj);
    }
}