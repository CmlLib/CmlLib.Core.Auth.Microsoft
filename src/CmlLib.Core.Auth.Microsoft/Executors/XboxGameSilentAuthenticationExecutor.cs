using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Executors
{
    public class XboxGameSilentAuthenticationExecutor : IXboxGameAuthenticationExecutor
    {
        private readonly IXboxGameAuthenticationExecutor _executor;

        public XboxGameSilentAuthenticationExecutor(IXboxGameAuthenticationExecutor executor)
        {
            this._executor = executor;
        }

        public async Task<XboxGameSession> Authenticate(
            IXboxGameAuthenticator xboxGameAuthenticator,
            IXboxAuthStrategy xboxAuthStrategy,
            ICacheStorage<XboxGameSession> cacheStorage)
        {
            var cachedSession = cacheStorage.Get();

            if (cachedSession != null && cachedSession.Validate())
            {
                return cachedSession;
            }
            else
            {
                return await _executor.Authenticate(xboxGameAuthenticator, xboxAuthStrategy, cacheStorage);
            }
        }
    }
}