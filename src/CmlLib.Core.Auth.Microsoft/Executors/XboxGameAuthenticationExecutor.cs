using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Executors
{
    public class XboxGameAuthenticationExecutor : IXboxGameAuthenticationExecutor
    {
        public async Task<XboxGameSession> Authenticate(
            IMicrosoftOAuthStrategy oAuthStrategy,
            IXboxAuthStrategy xboxAuthStrategy,
            IXboxGameAuthenticator xboxGameAuthenticator,
            ISessionStorage sessionStorage)
        {
            var oAuthResponse = await oAuthStrategy.Authenticate();
            var xboxTokens = await xboxAuthStrategy.Authenticate(oAuthResponse);
            var gameSession = await xboxGameAuthenticator.Authenticate(xboxTokens);
            await sessionStorage.SetAsync("G", gameSession);

            return gameSession;
        }
    }
}