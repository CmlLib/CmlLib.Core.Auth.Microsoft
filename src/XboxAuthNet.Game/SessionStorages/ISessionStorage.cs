using System.Threading.Tasks;

namespace XboxAuthNet.Game.SessionStorages
{
    public interface ISessionStorage
    {
        ValueTask<T?> GetAsync<T>(string key);
        ValueTask SetAsync<T>(string key, T? obj);
        ValueTask Clear();
    }
}