using System;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class BasicXboxAuthStrategy : IXboxAuthStrategy
    {
        private readonly ISessionSource<XboxAuthTokens> _xboxTokenSource;
        protected HttpClient HttpClient { get; private set; }

        public BasicXboxAuthStrategy(
            HttpClient httpClient,
            ISessionSource<XboxAuthTokens> xboxTokenSource) =>
            (HttpClient, _xboxTokenSource) = (httpClient, xboxTokenSource);

        public async Task<XboxAuthTokens> Authenticate(MicrosoftOAuthResponse oAuthResponse)
        {
            if (string.IsNullOrEmpty(oAuthResponse.AccessToken))
                throw new ArgumentException("AccessToken was empty");

            var xboxApi = new XboxAuth(HttpClient);
            var userToken = await xboxApi.ExchangeRpsTicketForUserToken(oAuthResponse.AccessToken);
            
            if (string.IsNullOrEmpty(userToken.Token))
                throw new XboxAuthException("UserToken was empty", 0);

            var xsts = await xboxApi.ExchangeTokensForXstsIdentity(userToken.Token, null, null, null, null);

            var tokens = new XboxAuthTokens
            {
                UserToken = userToken,
                XstsToken = xsts
            };

            await _xboxTokenSource.SetAsync(tokens);
            return tokens;
        }
    }
}