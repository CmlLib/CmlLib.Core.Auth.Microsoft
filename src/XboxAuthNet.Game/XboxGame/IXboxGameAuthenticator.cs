using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.XboxGame
{
    public interface IXboxGameAuthenticator<T>
    {
        Task<T> Authenticate(IXboxAuthStrategy xboxAuthStrategy);
    }
}