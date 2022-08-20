using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.OAuth;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public abstract class AbstractLoginHandler<T> where T : SessionCacheBase
    {
        private readonly IMicrosoftOAuthApi _oauth;
        protected ICacheManager<T> CacheManager { get; }

        public AbstractLoginHandler(
            IMicrosoftOAuthApi oauthApi, 
            ICacheManager<T> cacheManager)
        {
            this._oauth = oauthApi;
            this.CacheManager = cacheManager;
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
            var sessionCache = await readSessionCache();
            sessionCache = await LoginFromCache(sessionCache);

            await saveSessionCache(sessionCache);
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
        public virtual async Task<T> LoginFromCache(T? sessionCacheBase)
        {
            // if current cached minecraft token is invalid,
            // it try to refresh microsoft token, xbox token, and minecraft token
            if (sessionCacheBase == null || !sessionCacheBase.CheckValidation())
            {
                if (string.IsNullOrEmpty(sessionCacheBase?.MicrosoftOAuthToken?.RefreshToken))
                    throw new MicrosoftOAuthException("no refresh token", 0);

                var msToken = await _oauth.GetOrRefreshTokens(sessionCacheBase!.MicrosoftOAuthToken!);
                // success to refresh ms
                return await GetAllTokens(msToken);
            }
            else
            {
                return sessionCacheBase;
            }
        }


        public async Task<T> LoginFromOAuth()
        {
            var token = await _oauth.RequestNewTokens();
            return await LoginFromOAuth(token);
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
            await saveSessionCache(sessionCache);
            return sessionCache;
        }

        public abstract Task<T> GetAllTokens(MicrosoftOAuthResponse msToken);

        // managing caches

        protected async Task<T?> readSessionCache()
        {
            if (CacheManager == null)
                return null;
            return await CacheManager.ReadCache();
        }

        protected async Task saveSessionCache(T? sessionCache)
        {
            if (CacheManager == null)
                return;
            await CacheManager.SaveCache(sessionCache);
        }

        public async void ClearCache()
        {
            await CacheManager.ClearCache();
        }
    }
}
