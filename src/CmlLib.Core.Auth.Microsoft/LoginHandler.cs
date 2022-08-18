using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    // For backward compatibility, this class just wrap up JavaEditionLoginHandler

    [Obsolete("Use LoginHandlerBuilder")]
    public class LoginHandler
    {
        private readonly JavaEditionLoginHandler _loginHandler;
        private readonly MicrosoftOAuth _oauth;
        private readonly MicrosoftOAuthWebUILoginHandler _webUILoginHandler;

        private MicrosoftOAuthCode? authCode;

        [Obsolete("Use LoginHandlerBuilder.Create().ForJavaEdition().Build()")]
        public LoginHandler()
        {
            this._oauth = new MicrosoftOAuth(
                JavaEditionLoginHandlerBuilder.DefaultClientId, 
                XboxAuth.XboxScope, 
                HttpHelper.DefaultHttpClient.Value);
            this._webUILoginHandler = new MicrosoftOAuthWebUILoginHandler(this._oauth);
            this._loginHandler = new JavaEditionLoginHandlerBuilder()
                .WithMicrosoftOAuthApi(new MicrosoftOAuthApi(this._oauth))
                .Build();
        }

        public async Task<MSession> LoginFromCache()
        {
            var sessionCache = await _loginHandler.LoginFromCache();
            return sessionCache.GameSession;
        }

        public Task<JavaEditionSessionCache> LoginFromCache(JavaEditionSessionCache sessionCache)
        {
            return _loginHandler.LoginFromCache(sessionCache);
        }

        public async Task<MSession> LoginFromOAuth()
        {
            if (authCode == null)
                throw new InvalidOperationException();

            var token = await _oauth.GetTokens(authCode);
            var sessionCache = await _loginHandler.LoginFromOAuth(token);
            return sessionCache.GameSession;
        }

        public virtual async Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var sessionCache = await _loginHandler.LoginFromOAuth(msToken);
            return sessionCache.GameSession;
        }

        public void ClearCache()
        {
            _loginHandler.ClearCache();
        }

        public virtual string CreateOAuthUrl()
        {
            return _webUILoginHandler.CreateOAuthUrl();
        }

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            var result = _webUILoginHandler.CheckOAuthCodeResult(uri);
            authCode = result.OAuthCode;
            return result.IsSuccess;
        }
    }
}
