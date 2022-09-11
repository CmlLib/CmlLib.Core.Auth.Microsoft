using CmlLib.Core.Auth.Microsoft.OAuth;
using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockOAuthApi : IMicrosoftOAuthApi
    {
        public bool OAuthCodeResult { get; set; } = true;

        public MicrosoftOAuthResponse? GetOrRefreshTokensReturn = new MicrosoftOAuthResponse
        {
            AccessToken = "MockOAuthApi_GetOrRefreshTokens_AccessToken",
            RefreshToken = "MockOAuthApi_GetOrRefreshTokens_RefreshToken",
        };

        public Task<MicrosoftOAuthResponse> GetOrRefreshTokens(MicrosoftOAuthResponse? refreshToken, CancellationToken cancellationToken)
        {
            if (refreshToken != null && refreshToken.ExpireIn < 10)
                throw new MicrosoftOAuthException("token was expired", 0);

            return Task.FromResult(GetOrRefreshTokensReturn)!;
        }

        public MicrosoftOAuthResponse? RequestNewTokensReturn = new MicrosoftOAuthResponse
        {
            AccessToken = "MockOAuthApi_RequestNewTokens_AccessToken",
            RefreshToken = "MockOAuthApi_RequestNewTokens_RefreshToken",
        };

        public Task<MicrosoftOAuthResponse> RequestNewTokens(CancellationToken cancellationToken)
        {
            return Task.FromResult(RequestNewTokensReturn)!;
        }

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            if (OAuthCodeResult)
            {
                authCode = new MicrosoftOAuthCode
                {
                    Code = "Code",
                };
                return true;
            }
            else
            {
                authCode = new MicrosoftOAuthCode
                {
                    Error = "Error",
                    ErrorDescription = "ErrorDescription"
                };
                return false;
            }
        }

        public string CreateOAuthUrl()
        {
            return "CreateOAuthUrl";
        }

        public Task InvalidateTokens()
        {
            return Task.CompletedTask;
        }
    }
}
