using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public LoginHandler(MicrosoftOAuth oauth)
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");

            this.cacheManager = new JsonFileCacheManager<SessionCache>(defaultPath);
            this.oauth = oauth;
        }

        public LoginHandler(ICacheManager<SessionCache> cacheManager)
        {
            this.cacheManager = cacheManager;
            this.oauth = new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope);
        }

        public LoginHandler(MicrosoftOAuth oauth, ICacheManager<SessionCache> cacheManager)
        {
            this.cacheManager = cacheManager;
            this.oauth = oauth;
        }

        private MicrosoftOAuth oauth;
        private readonly ICacheManager<SessionCache> cacheManager;

        private SessionCache sessionCache;

        private void readSessionCache()
        {
            sessionCache = cacheManager.ReadCache();
        }

        private void saveSessionCache()
        {
            cacheManager.SaveCache(sessionCache);
        }

        public MSession LoginFromCache()
        {
            readSessionCache();

            var mcToken = sessionCache?.XboxSession;
            var msToken = sessionCache?.MicrosoftOAuthSession;

            if (mcToken == null || DateTime.Now > mcToken.ExpiresOn) // invalid mc session
            {
                if (!oauth.TryGetTokens(out msToken, msToken?.RefreshToken)) // failed to refresh ms
                    return null;
                
                // success to refresh ms
                mcToken = mcLogin(msToken);
            }

            return getGameSession(msToken, mcToken);
        }

        public bool CheckOAuthLoginSuccess(string url)
        {
            return oauth.CheckLoginSuccess(url);
        }

        public MSession LoginFromOAuth()
        {
            var result = oauth.TryGetTokens(out MicrosoftOAuthResponse msToken); // get token
            if (!result)
                throw new MicrosoftOAuthException(msToken);

            var mcToken = mcLogin(msToken);
            return getGameSession(msToken, mcToken);
        }

        public string CreateOAuthUrl()
        {
            return oauth.CreateUrl();
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

        private MSession getGameSession(MicrosoftOAuthResponse msToken, AuthenticationResponse mcToken)
        {
            if (sessionCache == null)
                sessionCache = new SessionCache();

            if (sessionCache.GameSession == null)
                sessionCache.GameSession = getSession(mcToken);

            sessionCache.XboxSession = mcToken;
            sessionCache.MicrosoftOAuthSession = msToken;

            saveSessionCache();
            return sessionCache.GameSession;
        }

        private AuthenticationResponse mcLogin(MicrosoftOAuthResponse msToken)
        {
            if (msToken == null)
                throw new ArgumentNullException("msToken");

            var xbox = new XboxAuth();
            var rps = xbox.ExchangeRpsTicketForUserToken(msToken.AccessToken);
            var xsts = xbox.ExchangeTokensForXSTSIdentity(rps.Token, null, null, XboxMinecraftLogin.RelyingParty, null);

            if (!xsts.IsSuccess)
            {
                throw createXboxException(xsts);
            }

            var mclogin = new XboxMinecraftLogin();
            var mcToken = mclogin.LoginWithXbox(xsts.UserHash, xsts.Token);
            return mcToken;
        }

        private Exception createXboxException(XboxAuthResponse xsts)
        {
            var msg = "";
            if (xsts.Error == XboxAuthResponse.ChildError || xsts.Error == "2148916236")
                msg = "xbox_error_child";
            else if (xsts.Error == XboxAuthResponse.NoXboxAccountError)
                msg = "xbox_error_noaccount";

            string errorCode;
            try
            {
                var errorInt = long.Parse(xsts.Error.Trim());
                errorCode = errorInt.ToString("x");
            }
            catch
            {
                errorCode = xsts.Error;
            }

            if (string.IsNullOrEmpty(msg))
                msg = errorCode;

            //return new XboxAuthException(msg, errorCode, xsts.Message);
            return new XboxAuthException(msg, null);
        }

        private MSession getSession(AuthenticationResponse mcToken)
        {
            // 6. get minecraft profile (username, uuid)

            if (mcToken == null)
                throw new ArgumentNullException(nameof(mcToken));

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
