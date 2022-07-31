using CmlLib.Core.Auth.Microsoft.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft
{
    // For backward compatibility, this class just wrap up JavaEditionLoginHandler

    [Obsolete("Use LoginHandlerBuilder")]
    public class LoginHandler
    {
        private readonly JavaEditionLoginHandler _loginHandler;

        [Obsolete("Use LoginHandlerBuilder.Create().ForJavaEdition().Build()")]
        public LoginHandler()
        {
            this._loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition()
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
            var sessionCache = await _loginHandler.LoginFromOAuth();
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
            return _loginHandler.CreateOAuthUrl();
        }

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            return _loginHandler.CheckOAuthCodeResult(uri, out authCode);
        }
    }
}
