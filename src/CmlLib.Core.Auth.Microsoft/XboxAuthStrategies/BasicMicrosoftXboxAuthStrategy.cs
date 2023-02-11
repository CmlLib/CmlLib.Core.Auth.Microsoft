using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class BasicXboxAuthStrategy : IXboxAuthStrategy
    {
        private readonly ISessionSource<XboxAuthTokens> _xboxTokenSource;
        private readonly HttpClient _httpClient;
        private readonly IMicrosoftOAuthStrategy _oAuthStrategy;

        public BasicXboxAuthStrategy(
            HttpClient httpClient,
            IMicrosoftOAuthStrategy oAuthStrategy,
            ISessionSource<XboxAuthTokens> xboxTokenSource) =>
            (_httpClient, _oAuthStrategy, _xboxTokenSource) = (httpClient, oAuthStrategy, xboxTokenSource);

        public async Task<XboxAuthTokens> Authenticate()
        {
            var oAuth = await _oAuthStrategy.Authenticate();
            
            var xboxApi = new XboxAuth(_httpClient);
            var userToken = await xboxApi.ExchangeRpsTicketForUserToken(oAuth.AccessToken);
            var xsts = await xboxApi.ExchangeTokensForXstsIdentity(userToken.Token, null, null, null, null);

            var tokens = new XboxAuthTokens();
            tokens.UserToken = userToken;
            tokens.XstsToken = xsts;

            await _xboxTokenSource.SetAsync(tokens);
            return tokens;
        }
    }
}