using System;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public class MicrosoftOAuthWebUILoginHandler : IWebUILoginHandler
    {
        private readonly MicrosoftOAuth _oAuth;

        public MicrosoftOAuthWebUILoginHandler(MicrosoftOAuth oauth)
        {
            this._oAuth = oauth;
        }

        public MicrosoftOAuthCodeCheckResult CheckOAuthCodeResult(Uri uri)
        {
            var checkResult = _oAuth.CheckLoginSuccess(uri.ToString(), out var authCode);
            return new MicrosoftOAuthCodeCheckResult(checkResult, authCode);
        }

        public string CreateOAuthUrl()
        {
            return _oAuth.CreateUrlForOAuth();
        }
    }
}
