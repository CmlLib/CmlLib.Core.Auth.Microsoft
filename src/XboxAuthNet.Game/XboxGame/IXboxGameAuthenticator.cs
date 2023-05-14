using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.XboxGame
{
    public interface IXboxGameAuthenticator<T>
        where T : ISession
    {
        Task<T> Authenticate(IXboxAuthStrategy xboxAuthStrategy);
    }
}