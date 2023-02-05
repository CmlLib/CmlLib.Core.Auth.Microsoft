using System.Threading;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Cache;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class SilentMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly ICacheStorage<MicrosoftOAuthResponse> _oauthTokenSource;
        private readonly MicrosoftOAuthCodeApiClient _oauthHandler;

        public SilentMicrosoftOAuthStrategy(MicrosoftOAuthCodeApiClient oauthFlow, ICacheStorage<MicrosoftOAuthResponse> oauthTokenSource)
        {
            this._oauthTokenSource = oauthTokenSource;
            this._oauthHandler = oauthFlow;
        }

        public Task<MicrosoftOAuthResponse> Authenticate()
        {
            var token = _oauthTokenSource.Get();

            if (string.IsNullOrEmpty(token?.RefreshToken))
                throw new MicrosoftOAuthException("no refresh token", 0);

            // TODO: validate token

            return _oauthHandler.RefreshToken(token.RefreshToken, CancellationToken.None);
        }
    }
}