using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using XboxAuthNet.OAuth.Models;

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
            var oAuthResponse = await oAuth(oAuthStrategy, sessionStorage);
            var xboxTokens = await xboxAuthStrategy.Authenticate(oAuthResponse);
            var gameSession = await xboxGameAuthenticator.Authenticate(xboxTokens);

            await sessionStorage.SetAsync("O", oAuthResponse);
            await sessionStorage.SetAsync("X", xboxTokens);
            await sessionStorage.SetAsync("G", gameSession);
            return gameSession;
        }

        private async Task<MicrosoftOAuthResponse> oAuth(IMicrosoftOAuthStrategy oAuthStrategy, ISessionStorage sessionStorage)
        {
            var oAuthResponse = await sessionStorage.GetAsync<MicrosoftOAuthResponse>("O");

            if (oAuthResponse == null)
                oAuthResponse = await oAuthStrategy.Authenticate();
            else
                oAuthResponse = await oAuthStrategy.Authenticate(oAuthResponse);

            return oAuthResponse;
        }
    }
}