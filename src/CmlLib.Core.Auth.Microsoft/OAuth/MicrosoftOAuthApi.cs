using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    // use XboxAuthNet library
    public class MicrosoftOAuthApi : IMicrosoftOAuthApi
    {
        protected readonly MicrosoftOAuth _oAuth;

        public MicrosoftOAuthApi(MicrosoftOAuth oa)
        {
            this._oAuth = oa;
        }

        public Task<MicrosoftOAuthResponse> GetOrRefreshTokens(MicrosoftOAuthResponse? token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token?.RefreshToken))
                throw new MicrosoftOAuthException("no refresh token", 0);

            // TODO: Validate token
            return _oAuth.RefreshToken(token?.RefreshToken!);
        }

        public virtual Task<MicrosoftOAuthResponse> RequestNewTokens(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task InvalidateTokens()
        {
            return Task.CompletedTask;
        }
    }
}
