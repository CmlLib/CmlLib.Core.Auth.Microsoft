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

        public LoginHandler(ICacheManager<SessionCache>? cacheManager)
        {
            this.cacheManager = cacheManager;
            this.OAuth = new MicrosoftOAuth(DefaultClientId, XboxAuth.XboxScope);
        }

        public LoginHandler(MicrosoftOAuth oAuth, ICacheManager<SessionCache>? cacheManager)
        {
            this.cacheManager = cacheManager;
            this.OAuth = oAuth;
        }

        public MicrosoftOAuth OAuth { get; private set; }
        private readonly ICacheManager<SessionCache>? cacheManager;

        private SessionCache? sessionCache;

        private void readSessionCache()
        {
            sessionCache = cacheManager?.ReadCache();
        }

        private void saveSessionCache()
        {
            cacheManager?.SaveCache(sessionCache ?? new SessionCache());
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
                var xsts = LoginXbox(msToken);
                mcToken = LoginMinecraft(xsts.UserHash!, xsts.Token!); // LoginXbox method checks if UserHash and Token is null
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
            if (msToken == null || !result)
                throw new MicrosoftOAuthException(msToken);

            return LoginFromOAuth(msToken);
        }

        public MSession LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var xboxToken = LoginXbox(msToken);
            var mcToken = LoginMinecraft(xboxToken.UserHash!, xboxToken.Token!); // LoginXbox method checks if UserHash and Token is null
            return getGameSession(msToken, mcToken);
        }

        public string CreateOAuthUrl()
        {
            return OAuth.CreateUrl();
        }

        private MSession getGameSession(MicrosoftOAuthResponse? msToken, AuthenticationResponse mcToken)
        {
            if (sessionCache == null)
                sessionCache = new SessionCache();
            
            sessionCache.GameSession ??= CreateMinecraftSession(mcToken);
            sessionCache.GameSession.AccessToken = mcToken.AccessToken;
            sessionCache.XboxSession = mcToken;
            sessionCache.MicrosoftOAuthSession = msToken;

            saveSessionCache();
            return sessionCache.GameSession;
        }

        public XboxAuthResponse LoginXbox(MicrosoftOAuthResponse? msToken)
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

            return xsts;
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

            string errorCode;
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

        public AuthenticationResponse LoginMinecraft(string userHash, string xsts)
        {
            var mcLogin = new XboxMinecraftLogin();
            var mcToken = mcLogin.LoginWithXbox(userHash, xsts);
            return mcToken;
        }

        public MSession CreateMinecraftSession(AuthenticationResponse xboxToken)
        {
            // 6. get minecraft profile (username, uuid)

            if (xboxToken == null)
                throw new ArgumentNullException(nameof(xboxToken));
            if (xboxToken.AccessToken == null)
                throw new ArgumentNullException(nameof(xboxToken.AccessToken));

            if (!MojangAPI.CheckGameOwnership(xboxToken.AccessToken))
                throw new InvalidOperationException("mojang_nogame");

            var profile = MojangAPI.GetProfileUsingToken(xboxToken.AccessToken);
            return new MSession
            {
                AccessToken = xboxToken.AccessToken,
                UUID = profile.UUID,
                Username = profile.Name,
                UserType = "msa"
            };
        }
    }
}
