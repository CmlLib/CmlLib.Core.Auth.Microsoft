using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandler
    {
        private readonly IXboxLiveApi xboxLiveApi;
        private readonly IMojangXboxApi mojangXboxApi;

        private readonly ICacheManager<SessionCache>? cacheManager;

        public LoginHandler() : this(builder => { })
        {
            
        }

        public LoginHandler(Action<LoginHandlerBuilder> builder)
        {
            var builderObj = new LoginHandlerBuilder();
            builder.Invoke(builderObj);

            this.cacheManager = builderObj.CacheManager.Value;
            this.xboxLiveApi = builderObj.XboxLiveApi.Value;
            this.mojangXboxApi = builderObj.MojangXboxApi.Value;
        }

        public string RelyingParty { get; set; } = MojangXboxApi.RelyingParty;
        public bool CheckGameOwnership { get; set; } = false;
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
                sessionCache.MojangSession = null;
                sessionCache.XboxAuthSession = null;
                sessionCache.GameSession = null;
                sessionCache.MicrosoftOAuthSession = null;
            }

            saveSessionCache();
        }

        public virtual async Task<MSession> LoginFromCache()
        {
            readSessionCache();

            var mcToken = sessionCache?.MojangSession;
            var msToken = sessionCache?.MicrosoftOAuthSession;
            var xboxToken = sessionCache?.XboxAuthSession;

            if (mcToken == null || DateTime.Now > mcToken.ExpiresOn) // invalid mc session
            {
                if (string.IsNullOrEmpty(msToken?.RefreshToken)) // failed to refresh ms
                    throw new MicrosoftOAuthException("no refresh token", 0);

                msToken = await xboxLiveApi.RefreshTokens(msToken?.RefreshToken!); // not null
                
                // success to refresh ms
                var xsts = await LoginXbox(msToken);
                mcToken = await LoginMinecraft(xsts);
            } 

            return await getGameSession(msToken, xboxToken, mcToken);
        }

        public virtual string CreateOAuthUrl()
        {
            return xboxLiveApi.CreateOAuthUrl();
        }

        public virtual bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthAuthCode authCode)
        {
            return xboxLiveApi.CheckOAuthCodeResult(uri, out authCode);
        }

        public async Task<MSession> LoginFromOAuth()
        {
            var msToken = await xboxLiveApi.GetTokens();
            return await LoginFromOAuth(msToken);
        }

        public virtual async Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var xboxToken = await LoginXbox(msToken);
            var mcToken = await LoginMinecraft(xboxToken);
            return await getGameSession(msToken, xboxToken, mcToken);
        }

        private async Task<MSession> getGameSession(MicrosoftOAuthResponse? msToken, XboxAuthResponse? xboxToken, MojangXboxLoginResponse mcToken)
        {
            if (sessionCache == null)
                sessionCache = new SessionCache();

            if (sessionCache.GameSession == null)
            {
                sessionCache.GameSession = await CreateMinecraftSession(mcToken);
                sessionCache.GameSession.AccessToken = mcToken.AccessToken;

                //if (!string.IsNullOrEmpty(xboxToken?.UserXUID)) // XUID is not necessary
                //    sessionCache.GameSession.XUID = xboxToken.UserXUID;
            }

            sessionCache.XboxAuthSession = xboxToken;
            sessionCache.MojangSession = mcToken;
            sessionCache.MicrosoftOAuthSession = msToken;

            saveSessionCache();
            return sessionCache.GameSession;
        }

        public virtual async Task<XboxAuthResponse> LoginXbox(MicrosoftOAuthResponse? msToken)
        {
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (msToken.AccessToken == null)
                throw new ArgumentNullException(nameof(msToken.AccessToken));

            var xsts = await xboxLiveApi.GetXSTS(msToken.AccessToken, null, null, RelyingParty);
            return xsts;
        }

        public virtual async Task<MojangXboxLoginResponse> LoginMinecraft(XboxAuthResponse xsts)
        {
            if (string.IsNullOrEmpty(xsts.UserHash))
                throw new ArgumentException("xsts.UserHash was null");
            if (string.IsNullOrEmpty(xsts.Token))
                throw new ArgumentException("xsts.Token was null");

            var mcToken = await mojangXboxApi.LoginWithXbox(xsts.UserHash!, xsts.Token!); // not null
            return mcToken;
        }

        public virtual async Task<MSession> CreateMinecraftSession(MojangXboxLoginResponse xboxToken)
        {
            // 6. get minecraft profile (username, uuid)

            if (xboxToken == null)
                throw new ArgumentNullException(nameof(xboxToken));
            if (xboxToken.AccessToken == null)
                throw new ArgumentNullException(nameof(xboxToken.AccessToken));

            if (CheckGameOwnership && !await mojangXboxApi.CheckGameOwnership(xboxToken.AccessToken))
                throw new MinecraftAuthException("mojang_nogame");

            MSession session;
            try
            {
                // throw 404 exception if profile is not exists
                session = await mojangXboxApi.GetProfileUsingToken(xboxToken.AccessToken);

                if (!session.CheckIsValid())
                    throw new MinecraftAuthException("mojang_noprofile");

                return session;
            }
            catch (MinecraftAuthException ex)
            {
                throw new MinecraftAuthException("mojang_noprofile", ex);
            }
        }
    }
}
