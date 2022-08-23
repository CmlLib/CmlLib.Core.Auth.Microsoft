using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.OAuth;
using System.Threading;
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
        public async Task<T> LoginFromCache(CancellationToken cancellationToken = default)
        {
            var sessionCache = await readSessionCache();
            sessionCache = await LoginFromCache(sessionCache, cancellationToken);

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
        public virtual async Task<T> LoginFromCache(T? sessionCacheBase, CancellationToken cancellationToken = default)
        {
            // if current cached minecraft token is invalid,
            // it try to refresh microsoft token, xbox token, and minecraft token
            if (sessionCacheBase == null || !sessionCacheBase.CheckValidation())
            {
                var msToken = await _oauth.GetOrRefreshTokens(sessionCacheBase!.MicrosoftOAuthToken!, cancellationToken);
                // success to refresh ms
                return await GetAllTokens(msToken, cancellationToken);
            }
            else
            {
                return sessionCacheBase;
            }
        }


        public async Task<T> LoginFromOAuth(CancellationToken cancellationToken = default)
        {
            var token = await _oauth.RequestNewTokens(cancellationToken);
            return await LoginFromOAuth(token, cancellationToken);
        }

        /// <summary>
        /// Get new Minecraft session using Microsoft OAuth token
        /// </summary>
        /// <returns>New valid session</returns>
        /// <exception cref="MicrosoftOAuthException"></exception>
        /// <exception cref="XboxAuthException"></exception>
        /// <exception cref="MinecraftAuthException"></exception>
        public async Task<T> LoginFromOAuth(MicrosoftOAuthResponse msToken, CancellationToken cancellationToken = default)
        {
            var sessionCache = await GetAllTokens(msToken, cancellationToken);
            await saveSessionCache(sessionCache);
            return sessionCache;
        }

        protected abstract Task<T> GetAllTokens(MicrosoftOAuthResponse msToken, CancellationToken cancellationToken = default);

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

        public async Task ClearCache()
        {
            await CacheManager.ClearCache();
            await _oauth.InvalidateTokens();
        }
    }
}
