using System;
using System.IO;
using CmlLib.Core.Mojang;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandler
    {
        public static readonly string DefaultClientId = "00000000402B5328";

        public LoginHandler() :
            this(new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope))
        {

        }

        public LoginHandler(MicrosoftOAuth oAuth)
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");

            this.cacheManager = new JsonFileCacheManager<SessionCache>(defaultPath);
            this.OAuth = oAuth;
        }

        public LoginHandler(ICacheManager<SessionCache> cacheManager)
        {
            this.cacheManager = cacheManager;
            this.OAuth = new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope);
        }

        public LoginHandler(MicrosoftOAuth oAuth, ICacheManager<SessionCache> cacheManager)
        {
            this.cacheManager = cacheManager;
            this.OAuth = oAuth;
        }

        public MicrosoftOAuth OAuth { get; private set; }
        private readonly ICacheManager<SessionCache> cacheManager;

        private SessionCache? sessionCache;

        private void readSessionCache()
        {
            sessionCache = cacheManager.ReadCache();
        }

        private void saveSessionCache()
        {
            cacheManager.SaveCache(sessionCache ?? new SessionCache());
        }

        public MSession? LoginFromCache()
        {
            readSessionCache();

            var mcToken = sessionCache?.XboxSession;
            var msToken = sessionCache?.MicrosoftOAuthSession;

            if (mcToken == null || DateTime.Now > mcToken.ExpiresOn) // invalid mc session
            {
                if (!OAuth.TryGetTokens(out msToken, msToken?.RefreshToken)) // failed to refresh ms
                    return null;
                
                // success to refresh ms
                mcToken = mcLogin(msToken);
            }

            return getGameSession(msToken, mcToken);
        }

        public bool CheckOAuthLoginSuccess(string url)
        {
            return OAuth.CheckLoginSuccess(url);
        }

        public MSession LoginFromOAuth()
        {
            var result = OAuth.TryGetTokens(out MicrosoftOAuthResponse? msToken); // get token
            if (!result)
                throw new MicrosoftOAuthException(msToken);

            var mcToken = mcLogin(msToken);
            return getGameSession(msToken, mcToken);
        }

        public string CreateOAuthUrl()
        {
            return OAuth.CreateUrl();
        }

        public void ClearCache()
        {
            if (sessionCache != null)
            {
                sessionCache.XboxSession = null;
                sessionCache.GameSession = null;
                sessionCache.MicrosoftOAuthSession = null;
            }

            saveSessionCache();
        }

        private MSession getGameSession(MicrosoftOAuthResponse? msToken, AuthenticationResponse mcToken)
        {
            if (sessionCache == null)
                sessionCache = new SessionCache();
            
            sessionCache.GameSession ??= getSession(mcToken);
            sessionCache.XboxSession = mcToken;
            sessionCache.MicrosoftOAuthSession = msToken;

            saveSessionCache();
            return sessionCache.GameSession;
        }

        private AuthenticationResponse mcLogin(MicrosoftOAuthResponse? msToken)
        {
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (!msToken.IsSuccess)
                throw new ArgumentException("msToken was failed");
            if (msToken.AccessToken == null)
                throw new ArgumentNullException(nameof(msToken.AccessToken));

            var xbox = new XboxAuth();
            var rps = xbox.ExchangeRpsTicketForUserToken(msToken.AccessToken);

            if (!rps.IsSuccess || string.IsNullOrEmpty(rps.Token))
                throw new XboxAuthException($"ExchangeRpsTicketForUserToken\n{rps.Error}\n{rps.Message}", null);
            
            var xsts = xbox.ExchangeTokensForXstsIdentity(
                rps.Token, 
                null, 
                null, 
                XboxMinecraftLogin.RelyingParty, 
                null);

            if (!xsts.IsSuccess || string.IsNullOrEmpty(xsts.UserHash) || string.IsNullOrEmpty(xsts.Token))
            {
                throw createXboxException(xsts);
            }

            var mcLogin = new XboxMinecraftLogin();
            var mcToken = mcLogin.LoginWithXbox(xsts.UserHash, xsts.Token);
            return mcToken;
        }

        private Exception createXboxException(XboxAuthResponse xsts)
        {
            string msg = "";
            if (xsts.Error == XboxAuthResponse.ChildError || xsts.Error == "2148916236")
                msg = "xbox_error_child";
            else if (xsts.Error == XboxAuthResponse.NoXboxAccountError)
                msg = "xbox_error_noaccount";
            else if (string.IsNullOrEmpty(xsts.UserHash))
                msg = "empty_userhash";
            else if (string.IsNullOrEmpty(xsts.Token))
                msg = "empty_token";

            string errorCode = "";
            try
            {
                var errorCodeStr = xsts.Error?.Trim();
                if (string.IsNullOrEmpty(errorCodeStr))
                {
                    errorCode = "no_error_msg";
                }
                else
                {
                    var errorInt = long.Parse(errorCodeStr);
                    errorCode = errorInt.ToString("x");
                }
            }
            catch
            {
                errorCode = xsts.Error ?? "no_error_msg";
            }

            if (string.IsNullOrEmpty(msg))
                msg = errorCode;

            return new XboxAuthException(msg, errorCode, xsts.Message ?? "no_error_msg");
            //return new XboxAuthException(msg, null);
        }

        private MSession getSession(AuthenticationResponse mcToken)
        {
            // 6. get minecraft profile (username, uuid)

            if (mcToken == null)
                throw new ArgumentNullException(nameof(mcToken));
            if (mcToken.AccessToken == null)
                throw new ArgumentNullException(nameof(mcToken.AccessToken));

            if (!MojangAPI.CheckGameOwnership(mcToken.AccessToken))
                throw new InvalidOperationException("mojang_nogame");

            var profile = MojangAPI.GetProfileUsingToken(mcToken.AccessToken);
            return new MSession
            {
                AccessToken = mcToken.AccessToken,
                UUID = profile.UUID,
                Username = profile.Name
            };
        }
    }
}
