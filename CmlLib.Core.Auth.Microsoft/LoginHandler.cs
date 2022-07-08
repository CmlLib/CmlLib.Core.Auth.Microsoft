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

        private SessionCache? readSessionCache()
        {
            return cacheManager?.ReadCache();
        }

        private void saveSessionCache(SessionCache? sessionCache)
        {
            cacheManager?.SaveCache(sessionCache ?? new SessionCache());
        }

        public void ClearCache()
        {
            saveSessionCache(null);
        }

        public virtual async Task<MSession> LoginFromCache()
        {
            var sessionCache = readSessionCache() ?? new SessionCache();

            // if current cached minecraft token is invalid,
            // it try to refresh microsoft token, xbox token, and minecraft token
            if (sessionCache.MojangXboxToken == null || !sessionCache.MojangXboxToken.CheckValidation())
            {
                if (string.IsNullOrEmpty(sessionCache.MicrosoftOAuthToken?.RefreshToken))
                    throw new MicrosoftOAuthException("no refresh token", 0);

                // RefreshTokens method throws exception when server fails to refresh token
                sessionCache.MicrosoftOAuthToken = await xboxLiveApi.RefreshTokens(sessionCache.MicrosoftOAuthToken?.RefreshToken!); 
                
                // success to refresh ms
                sessionCache.XstsToken = await GetXsts(sessionCache.MicrosoftOAuthToken);
                sessionCache.MojangXboxToken = await GetMojangXboxToken(sessionCache.XstsToken);
                sessionCache.GameSession = null; // clear GameSession to refresh
            } 

            // always refresh game session
            sessionCache.GameSession = await GetMSession(sessionCache.MojangXboxToken, sessionCache.GameSession);

            saveSessionCache(sessionCache);
            return sessionCache.GameSession;
        }

        public virtual string CreateOAuthUrl()
        {
            return xboxLiveApi.CreateOAuthUrl();
        }

        public virtual bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            return xboxLiveApi.CheckOAuthCodeResult(uri, out authCode);
        }

        public async Task<MSession> LoginFromOAuth()
        {
            // if CheckOAuthCodeResult returns true,
            // the xboxLiveApi holds OAuth code and can get valid Microsoft OAuth token

            var msToken = await xboxLiveApi.GetTokens();
            return await LoginFromOAuth(msToken);
        }

        public virtual async Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var xsts = await GetXsts(msToken);
            var mojangToken = await GetMojangXboxToken(xsts);
            var msession = await GetMSession(mojangToken, new MSession());

            saveSessionCache(new SessionCache
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mojangToken,
                GameSession = msession
            });
            return msession;
        }

        protected virtual async Task<XboxAuthResponse> GetXsts(MicrosoftOAuthResponse msToken)
        {
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (msToken.AccessToken == null)
                throw new ArgumentNullException(nameof(msToken.AccessToken));

            var xsts = await xboxLiveApi.GetXSTS(msToken.AccessToken, null, null, RelyingParty);
            return xsts;
        }

        protected virtual async Task<MojangXboxLoginResponse> GetMojangXboxToken(XboxAuthResponse xsts)
        {
            if (string.IsNullOrEmpty(xsts.UserHash))
                throw new ArgumentException("xsts.UserHash was null");
            if (string.IsNullOrEmpty(xsts.Token))
                throw new ArgumentException("xsts.Token was null");

            var mcToken = await mojangXboxApi.LoginWithXbox(xsts.UserHash!, xsts.Token!); // not null
            return mcToken;
        }

        protected virtual async Task<MSession> GetMSession(MojangXboxLoginResponse mcToken, MSession? cachedSession)
        {
            if (cachedSession == null)
                cachedSession = new MSession();

            // update Username, UUID
            if (string.IsNullOrEmpty(cachedSession.Username) ||
                string.IsNullOrEmpty(cachedSession.UUID))
            {
                cachedSession = await createMinecraftSession(mcToken);
            }

            // update XUID
            //if (string.IsNullOrEmpty(sessionCache.GameSession.UserXUID))
            //{
            //    var payload = mcToken.DecodeAccesTokenPayload();
            //    sessionCache.GameSession.UserXUID = payload.Xuid;
            //}

            // update AccessToken
            cachedSession.AccessToken = mcToken.AccessToken;
            return cachedSession;
        }

        private async Task<MSession> createMinecraftSession(MojangXboxLoginResponse mcToken)
        {
            if (mcToken == null)
                throw new ArgumentNullException(nameof(mcToken));
            if (mcToken.AccessToken == null)
                throw new ArgumentNullException(nameof(mcToken.AccessToken));

            if (CheckGameOwnership && !await mojangXboxApi.CheckGameOwnership(mcToken.AccessToken))
                throw new MinecraftAuthException("mojang_nogame");

            MSession session;
            try
            {
                // throw 404 exception if profile is not exists
                session = await mojangXboxApi.GetProfileUsingToken(mcToken.AccessToken);

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
