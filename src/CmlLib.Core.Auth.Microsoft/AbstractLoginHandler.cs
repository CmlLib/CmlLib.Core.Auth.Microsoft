using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public abstract class AbstractLoginHandler<T> where T : SessionCacheBase
    {
        private readonly IXboxLiveApi _xboxLiveApi;
        private readonly ICacheManager<T>? _cacheManager;

        public AbstractLoginHandler(IXboxLiveApi xboxLiveApi, ICacheManager<T>? cacheManager)
        {
            this._xboxLiveApi = xboxLiveApi;
            this._cacheManager = cacheManager;
        }

        /// <summary>
        /// Get minecraft session from cached tokens. This method also try to refresh token if cached token is expired. 
        /// Cached tokens is retrieved by internal cache manager.
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public async Task<T> LoginFromCache()
        {
            var sessionCache = readSessionCache();
            sessionCache = await LoginFromCache(sessionCache);

            saveSessionCache(sessionCache);
            return sessionCache;
        }

        /// <summary>
        /// Get minecraft session from cached tokens. This method also try to refresh token if cached token is expired. 
        /// </summary>
        /// <param name="sessionCache">cached session</param>
        /// <returns>valid sessions</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public virtual async Task<T> LoginFromCache(T sessionCacheBase)
        {
            // if current cached minecraft token is invalid,
            // it try to refresh microsoft token, xbox token, and minecraft token
            if (sessionCacheBase == null || !sessionCacheBase.CheckValidation())
            {
                if (string.IsNullOrEmpty(sessionCacheBase?.MicrosoftOAuthToken?.RefreshToken))
                    throw new MicrosoftOAuthException("no refresh token", 0);

                // RefreshTokens method throws exception when server fails to refresh token
                var msToken = await _xboxLiveApi.RefreshTokens(sessionCacheBase?.MicrosoftOAuthToken?.RefreshToken!);

                // success to refresh ms
                return await GetAllTokens(msToken);
            }
            else
            {
                return sessionCacheBase;
            }
        }

        /// <summary>
        /// Get new Minecraft session
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public async Task<T> LoginFromOAuth()
        {
            // if CheckOAuthCodeResult returns true,
            // the xboxLiveApi holds OAuth code and can get valid Microsoft OAuth token

            var msToken = await GetMicrosoftOAuthToken();
            return await LoginFromOAuth(msToken);
        }

        /// <summary>
        /// Get new Minecraft session using Microsoft OAuth token
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public async Task<T> LoginFromOAuth(MicrosoftOAuthResponse msToken)
        {
            var sessionCache = await GetAllTokens(msToken);
            saveSessionCache(sessionCache);
            return sessionCache;
        }

        public abstract Task<T> GetAllTokens(MicrosoftOAuthResponse msToken);

        // xboxLiveApis

        protected async Task<XboxAuthResponse> GetXsts(MicrosoftOAuthResponse msToken, string? deviceToken, string? titleToken, string? relyingParty)
        {
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (msToken.AccessToken == null)
                throw new ArgumentNullException(nameof(msToken.AccessToken));

            var xsts = await _xboxLiveApi.GetXSTS(msToken.AccessToken, deviceToken, titleToken, relyingParty);
            return xsts;
        }

        protected async Task<MicrosoftOAuthResponse> GetMicrosoftOAuthToken()
        {
            return await _xboxLiveApi.GetTokens();
        }

        public string CreateOAuthUrl()
        {
            return _xboxLiveApi.CreateOAuthUrl();
        }

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            return _xboxLiveApi.CheckOAuthCodeResult(uri, out authCode);
        }

        // managing caches

        protected T readSessionCache()
        {
            return _cacheManager?.ReadCache();
        }

        protected void saveSessionCache(T sessionCache)
        {
            _cacheManager?.SaveCache(sessionCache);
        }

        public void ClearCache()
        {
            saveSessionCache(null);
        }
    }
}
