using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.XboxAuthStrategies
{
    public abstract class MicrosoftXboxAuthStrategy : IXboxAuthStrategy
    {
        protected HttpClient HttpClient { get; private set; }
        private IMicrosoftOAuthStrategy _oAuthStrategy;

        public MicrosoftXboxAuthStrategy(
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

        protected abstract Task<XboxAuthTokens> AuthenticateFromOAuthResult(MicrosoftOAuthResponse oAuth, string relyingParty);
    }
}