using System;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public class MicrosoftOAuthWebUILoginHandler : IWebUILoginHandler
    {
        private readonly MicrosoftOAuth _oAuth;
        private readonly MicrosoftOAuthParameters _parameters;

        public MicrosoftOAuthWebUILoginHandler(MicrosoftOAuth oauth, MicrosoftOAuthParameters param)
        {
            this._oAuth = oauth;
            this._parameters = param;
        }

        public MicrosoftOAuthCodeCheckResult CheckOAuthCodeResult(Uri uri)
        {
            _oAuth.CheckLoginSuccess(uri.ToString(), out var authCode);
            return new MicrosoftOAuthCodeCheckResult(!authCode.IsEmpty, authCode);
        }

        public string CreateOAuthUrl()
        {
            return _oAuth.CreateUrlForOAuth(this._parameters);
        }
    }
}
