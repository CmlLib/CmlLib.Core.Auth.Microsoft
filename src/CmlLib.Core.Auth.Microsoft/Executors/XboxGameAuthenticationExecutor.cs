using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Executors
{
    public class XboxGameAuthenticationExecutor : IXboxGameAuthenticationExecutor
    {
        public async Task<XboxGameSession> Authenticate(
            IXboxGameAuthenticator xboxGameAuthenticator,
            IXboxAuthStrategy xboxAuthStrategy,
            ICacheStorage<XboxGameSession> cacheStorage)
        {
            var xboxTokens = await xboxAuthStrategy.Authenticate();
            var gameSession = await xboxGameAuthenticator.Authenticate(xboxTokens);
            cacheStorage.Set(gameSession);

            return gameSession;
        }
    }
}