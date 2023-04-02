using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Requests;

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

        public async Task<XboxAuthTokens> Authenticate(string relyingParty)
        {
            var oAuth = await _oAuthStrategy.Authenticate();
            if (string.IsNullOrEmpty(oAuth.AccessToken))
                throw new MicrosoftOAuthException("AccessToken was empty", 0);

            var tokens = await AuthenticateFromOAuthResult(oAuth, relyingParty);

            return tokens;
        }

        protected virtual async Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth, string relyingParty)
        {
            var xboxAuthClient = new XboxAuthClient(HttpClient);
            var userToken = await xboxAuthClient.RequestUserToken(oAuth.AccessToken!);
            
            if (string.IsNullOrEmpty(userToken.Token))
                throw new XboxAuthException("UserToken was empty", 0);

            var xsts = await xboxAuthClient.RequestXsts(new XboxXstsRequest
            {
                UserToken = userToken.Token,
                RelyingParty = relyingParty
            });

            var tokens = new XboxAuthTokens
            {
                UserToken = userToken,
                XstsToken = xsts
            };
            return tokens;
        }
    }
}