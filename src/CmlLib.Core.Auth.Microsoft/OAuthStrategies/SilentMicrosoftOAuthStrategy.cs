using System.Threading;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class SilentMicrosoftOAuthStrategy : IMicrosoftOAuthStrategy
    {
        private readonly MicrosoftOAuthCodeApiClient _oauthHandler;

        public SilentMicrosoftOAuthStrategy(MicrosoftOAuthCodeApiClient oauthFlow) =>
            _oauthHandler = oauthFlow;

        public Task<MicrosoftOAuthResponse> Authenticate()
        {
            throw new MicrosoftOAuthException("no refresh token", 0);
        }

        public async Task<MicrosoftOAuthResponse> Authenticate(MicrosoftOAuthResponse cachedResponse)
        {
            if (string.IsNullOrEmpty(cachedResponse?.RefreshToken))
                throw new MicrosoftOAuthException("no refresh token", 0);

            // TODO: validate token

            var refreshed = await _oauthHandler.RefreshToken(cachedResponse.RefreshToken, CancellationToken.None);
            return refreshed;
        }
    }
}