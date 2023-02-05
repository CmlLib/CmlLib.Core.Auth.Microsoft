using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.Cache;

namespace CmlLib.Core.Auth.Microsoft.Executors
{
    public interface IXboxGameAuthenticationExecutor
    {
        Task<XboxGameSession> Authenticate(
            IXboxGameAuthenticator xboxGameAuthenticator,
            IXboxAuthStrategy xboxAuthStrategy,
            ICacheStorage<XboxGameSession> cacheStorage);
    }
}