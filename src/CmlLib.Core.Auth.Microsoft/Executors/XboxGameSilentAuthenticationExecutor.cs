using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
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
            ISessionStorage sessionStorage)
        {
            var cachedSession = await sessionStorage.GetAsync<XboxGameSession>("G");

            if (cachedSession != null && cachedSession.Validate())
            {
                return cachedSession;
            }
            else
            {
                return await _executor.Authenticate(xboxGameAuthenticator, xboxAuthStrategy, sessionStorage);
            }
        }
    }
}