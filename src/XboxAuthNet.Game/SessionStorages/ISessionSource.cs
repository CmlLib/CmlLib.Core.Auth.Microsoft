using System.Threading.Tasks;

namespace XboxAuthNet.Game.SessionStorages
{
    public interface ISessionSource<T>
    {
        ValueTask<T?> GetAsync();
        ValueTask SetAsync(T? obj);
        ValueTask Clear();
    }
}