using System;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class BasicXboxAuthStrategy : IXboxAuthStrategy
    {
        protected HttpClient HttpClient { get; private set; }

        public BasicXboxAuthStrategy(HttpClient httpClient) =>
            (HttpClient) = (httpClient);

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

            return tokens;
        }
    }
}