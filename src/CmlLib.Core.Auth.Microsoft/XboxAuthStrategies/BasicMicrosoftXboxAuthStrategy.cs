using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class BasicXboxAuthStrategy : IXboxAuthStrategy
    {
        private readonly HttpClient _httpClient;
        private readonly IMicrosoftOAuthStrategy _oAuthStrategy;

        public BasicXboxAuthStrategy(
            HttpClient httpClient,
            IMicrosoftOAuthStrategy oAuthStrategy)
        {
            this._httpClient = httpClient;
            this._oAuthStrategy = oAuthStrategy;
        }

        public async Task<XboxAuthTokens> Authenticate()
        {
            var oAuth = await _oAuthStrategy.Authenticate();
            
            var xboxApi = new XboxAuth(_httpClient);
            var userToken = await xboxApi.ExchangeRpsTicketForUserToken(oAuth.AccessToken);
            var xsts = await xboxApi.ExchangeTokensForXstsIdentity(userToken.Token, null, null, null, null);

            var tokens = new XboxAuthTokens();
            tokens.UserToken = userToken;
            tokens.XstsToken = xsts;
            return tokens;
        }
    }
}