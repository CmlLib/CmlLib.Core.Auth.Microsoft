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

            this.xboxLiveApi = builderObj.XboxLiveApi.Value;
            this.mojangXboxApi = builderObj.MojangXboxApi.Value;
            this.cacheManager = builderObj.CacheManager.Value;
            this.RelyingParty = builderObj.XboxRelyingParty;
        }

        public string RelyingParty { get; set; }
        public bool CheckGameOwnership { get; set; } = false;

        /// <summary>
        /// Get minecraft session from cached tokens. This method also try to refresh token if cached token is expired. 
        /// Cached tokens is retrieved by internal cache manager.
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public async Task<MSession> LoginFromCache()
        {
            var sessionCache = readSessionCache() ?? new SessionCache();
            sessionCache = await LoginFromCache(sessionCache);
            saveSessionCache(sessionCache);
            return sessionCache.GameSession!;
        }

        /// <summary>
        /// Get minecraft session from cached tokens. This method also try to refresh token if cached token is expired. 
        /// </summary>
        /// <param name="sessionCache">cached session</param>
        /// <returns>valid sessions</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public virtual async Task<SessionCache> LoginFromCache(SessionCache sessionCache)
        {
            // if current cached minecraft token is invalid,
            // it try to refresh microsoft token, xbox token, and minecraft token
            if (sessionCache.MojangXboxToken == null || !sessionCache.MojangXboxToken.CheckValidation())
            {
                if (string.IsNullOrEmpty(sessionCache.MicrosoftOAuthToken?.RefreshToken))
                    throw new MicrosoftOAuthException("no refresh token", 0);

                // RefreshTokens method throws exception when server fails to refresh token
                var msToken = await xboxLiveApi.RefreshTokens(sessionCache.MicrosoftOAuthToken?.RefreshToken!);

                // success to refresh ms
                return await GetAllTokens(msToken);
            }
            else
            {
                // always refresh game session
                sessionCache.GameSession = await GetMSession(sessionCache.MojangXboxToken, sessionCache.GameSession);
                return sessionCache;
            }
        }

        /// <summary>
        /// Get new Minecraft session
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public async Task<MSession> LoginFromOAuth()
        {
            // if CheckOAuthCodeResult returns true,
            // the xboxLiveApi holds OAuth code and can get valid Microsoft OAuth token

            var msToken = await xboxLiveApi.GetTokens();
            return await LoginFromOAuth(msToken);
        }

        /// <summary>
        /// Get new Minecraft session using Microsoft OAuth token
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public virtual async Task<MSession> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var sessionCache = await GetAllTokens(msToken);
            saveSessionCache(sessionCache);
            return sessionCache.GameSession!;
        }

        protected SessionCache? readSessionCache()
        {
            return cacheManager?.ReadCache();
        }

        protected void saveSessionCache(SessionCache? sessionCache)
        {
            cacheManager?.SaveCache(sessionCache ?? new SessionCache());
        }

        public void ClearCache()
        {
            saveSessionCache(null);
        }

        protected async Task<SessionCache> GetAllTokens(MicrosoftOAuthResponse msToken)
        {
            var xsts = await GetXsts(msToken, null, null, this.RelyingParty);
            var mojangToken = await GetMojangXboxToken(xsts);
            var msession = await GetMSession(mojangToken, new MSession());

            return new SessionCache
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mojangToken,
                GameSession = msession
            };
        }

        protected virtual async Task<XboxAuthResponse> GetXsts(MicrosoftOAuthResponse msToken, string? deviceToken, string? titleToken, string? relyingParty)
        {
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (msToken.AccessToken == null)
                throw new ArgumentNullException(nameof(msToken.AccessToken));

            var xsts = await xboxLiveApi.GetXSTS(msToken.AccessToken, deviceToken, titleToken, relyingParty);
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

            // update XUID (for CmlLib.Core 3.4.0)
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

        public virtual string CreateOAuthUrl()
        {
            return xboxLiveApi.CreateOAuthUrl();
        }

        public virtual bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            return xboxLiveApi.CheckOAuthCodeResult(uri, out authCode);
        }
    }
}
