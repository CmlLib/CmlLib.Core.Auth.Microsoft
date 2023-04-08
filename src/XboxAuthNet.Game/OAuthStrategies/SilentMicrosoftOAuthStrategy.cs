using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuthStrategies
{
    public class SilentMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly ISessionSource<MicrosoftOAuthResponse> _oauthTokenSource;
        private readonly MicrosoftOAuthCodeApiClient _oauthHandler;

        public SilentMicrosoftOAuthStrategy(
            MicrosoftOAuthCodeApiClient oauthFlow, 
            ISessionSource<MicrosoftOAuthResponse> oauthTokenSource) =>
            (_oauthTokenSource, _oauthHandler) = (oauthTokenSource, oauthFlow);

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var token = await _oauthTokenSource.GetAsync();
            if (string.IsNullOrEmpty(token?.RefreshToken))
                throw new MicrosoftOAuthException("no refresh token", 0);

            // TODO: validate token

            token = await _oauthHandler.RefreshToken(token.RefreshToken, CancellationToken.None);
            return token;
        }
    }
}