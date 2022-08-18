using System;
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

        public Task<MicrosoftOAuthResponse> GetOrRefreshTokens(MicrosoftOAuthResponse token)
        {
            if (string.IsNullOrEmpty(token.RefreshToken))
                throw new ArgumentException("token has empty RefreshToken");

            // TODO: Validate token
            return _oAuth.RefreshToken(token.RefreshToken!);
        }

        public virtual Task<MicrosoftOAuthResponse> RequestNewTokens()
        {
            throw new NotImplementedException();
        }
    }
}
