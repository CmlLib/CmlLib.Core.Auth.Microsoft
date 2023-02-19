using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class BasicXboxAuthStrategy : IXboxAuthStrategy
    {
        protected HttpClient HttpClient { get; private set; }
        private IMicrosoftOAuthStrategy _oAuthStrategy;

        public BasicXboxAuthStrategy(
            HttpClient httpClient,
            IMicrosoftOAuthStrategy oAuthStrategy) =>
            (HttpClient, _oAuthStrategy) = (httpClient, oAuthStrategy);

        public async Task<XboxAuthTokens> Authenticate()
        {
            var oAuth = await _oAuthStrategy.Authenticate();
            if (string.IsNullOrEmpty(oAuth.AccessToken))
                throw new MicrosoftOAuthException("AccessToken was empty", 0);

            var tokens = await AuthenticateFromOAuthResult(oAuth);

            return tokens;
        }

        protected virtual async Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth)
        {
            var xboxApi = new XboxAuth(HttpClient);
            var userToken = await xboxApi.ExchangeRpsTicketForUserToken(oAuth.AccessToken!);
            
            if (string.IsNullOrEmpty(userToken.Token))
                throw new XboxAuthException("UserToken was empty", 0);

            var xsts = await xboxApi.ExchangeTokensForXstsIdentity(userToken.Token, null, null, null, null);

            var tokens = new XboxAuthTokens
            {
                UserToken = userToken,
                XstsToken = xsts
            };
            return tokens;
        }
    }
}