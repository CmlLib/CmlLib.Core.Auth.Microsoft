using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    // For backward compatibility, this class just wrap up JavaEditionLoginHandler

    [Obsolete("Use JavaEditionLoginHandler")]
    public class LoginHandler
    {
        private readonly JavaEditionLoginHandler _loginHandler;
        private MicrosoftOAuth _oauth;
        private MicrosoftOAuthWebUILoginHandler _webUILoginHandler;

        private MicrosoftOAuthCode? _authCode;

        public LoginHandler()
        {
            this._loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .With((builder, context) =>
                {
                    if (string.IsNullOrEmpty(context.ClientId))
                        throw new InvalidOperationException("context.ClientId was null");

                    this._oauth = new MicrosoftOAuth(context.ClientId!, XboxAuth.XboxScope, context.HttpClient);
                    this._webUILoginHandler = new MicrosoftOAuthWebUILoginHandler(this._oauth);
                    builder.WithMicrosoftOAuthApi(new MicrosoftOAuthApi(this._oauth));
                })
                .Build();

            if (this._oauth == null)
                throw new InvalidOperationException("_oauth was null");
            if (this._webUILoginHandler == null)
                throw new InvalidOperationException("_webUILoginHandler was null");
        }

        public LoginHandler(Action<JavaEditionLoginHandlerBuilder> builder)
        {
            var builderObj = new LoginHandlerBuilder().ForJavaEdition();
            builder.Invoke(builderObj);
            var parameters = builderObj.BuildParameters();

            var oauth = parameters.MicrosoftOAuthApi as MicrosoftOAuth;
            if (oauth == null)
                throw new InvalidOperationException("Legacy LoginHandler only can handle MicrosoftOAuth. Use JavaEditionLoginHandlerBuilder for others.");

            this._oauth = oauth;
            this._webUILoginHandler = new MicrosoftOAuthWebUILoginHandler(_oauth);
            this._loginHandler = builderObj.Build();
        }

        public async Task<MSession> LoginFromCache()
        {
            var sessionCache = await _loginHandler.LoginFromCache(CancellationToken.None);
            return sessionCache.GameSession;
        }

        public Task<JavaEditionSessionCache> LoginFromCache(JavaEditionSessionCache sessionCache)
        {
            return _loginHandler.LoginFromCache(sessionCache, CancellationToken.None);
        }

        public async Task<MSession> LoginFromOAuth()
        {
            if (_authCode == null)
                throw new InvalidOperationException("authCode was null");

            var token = await _oauth.GetTokens(_authCode);
            var sessionCache = await _loginHandler.LoginFromOAuth(token, CancellationToken.None);
            return sessionCache.GameSession;
        }

        public virtual async Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var sessionCache = await _loginHandler.LoginFromOAuth(msToken, CancellationToken.None);
            return sessionCache.GameSession;
        }

        public async Task ClearCache()
        {
            await _loginHandler.ClearCache();
        }

        public virtual string CreateOAuthUrl()
        {
            return _webUILoginHandler.CreateOAuthUrl();
        }

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            var result = _webUILoginHandler.CheckOAuthCodeResult(uri);
            this._authCode = authCode = result.OAuthCode ?? throw new InvalidOperationException("OAuthCode was null");
            return result.IsSuccess;
        }
    }
}
