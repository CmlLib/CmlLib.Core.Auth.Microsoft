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
        private readonly IMicrosoftOAuthStrategy _oAuthStrategy;

        public BasicXboxAuthStrategy(
            HttpClient httpClient,
            IMicrosoftOAuthStrategy oAuthStrategy,
            ISessionSource<XboxAuthTokens> xboxTokenSource) =>
            (HttpClient, _oAuthStrategy, _xboxTokenSource) = (httpClient, oAuthStrategy, xboxTokenSource);

        public async Task<XboxAuthTokens> Authenticate()
        {
            var oAuth = await _oAuthStrategy.Authenticate();
            if (string.IsNullOrEmpty(oAuth.AccessToken))
                throw new MicrosoftOAuthException("AccessToken was empty", 0);

            var tokens = await AuthenticateFromOAuthResult(oAuth);

            await _xboxTokenSource.SetAsync(tokens);
            return tokens;
        }

        protected virtual async Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth)
        {
            if (string.IsNullOrEmpty(oAuth.AccessToken))
                throw new ArgumentException("AccessToken was empty");

            var xboxApi = new XboxAuth(HttpClient);
            var userToken = await xboxApi.ExchangeRpsTicketForUserToken(oAuth.AccessToken);
            
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