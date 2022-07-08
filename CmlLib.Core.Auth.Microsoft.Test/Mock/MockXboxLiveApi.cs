using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockXboxLiveApi : IXboxLiveApi
    {
        public bool OAuthCodeResult { get; set; } = true;

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

        public Task<MicrosoftOAuthResponse> GetTokens()
        {
            return Task.FromResult(new MicrosoftOAuthResponse
            {
                AccessToken = "MockXboxLiveApi_AccessToken",
                RefreshToken = "MockXboxLiveApi_RefreshToken",
            });
        }

        public Task<XboxAuthResponse> GetXSTS(string token, string? deviceToken, string? titleToken, string? xstsRelyingParty)
        {
            return Task.FromResult(new XboxAuthResponse
            {
                Token = "MockXboxLiveApi_Token",
                UserHash = "MockXboxLiveApi_UserHash"
            });
        }

        public Task<MicrosoftOAuthResponse> RefreshTokens(string token)
        {
            return GetTokens();
        }
    }
}
