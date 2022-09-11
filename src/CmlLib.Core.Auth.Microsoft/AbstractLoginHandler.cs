using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft
{
    public abstract class AbstractLoginHandler<T> where T : SessionCacheBase
    {
        private readonly IMicrosoftOAuthApi _oauth;
        private readonly IXboxLiveApi _xbox;

        protected string RelyingParty { get; }
        protected ICacheManager<T> CacheManager { get; }

        public AbstractLoginHandler(
            LoginHandlerParameters parameters,
            ICacheManager<T> cacheManager)
        {
            parameters.Validate();

            this._oauth = parameters.MicrosoftOAuthApi!;
            this.CacheManager = cacheManager;
            this._xbox = parameters.XboxLiveApi!;
            this.RelyingParty = parameters.RelyingParty!;
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
                var msToken = await _oauth.GetOrRefreshTokens(sessionCacheBase?.MicrosoftOAuthToken, cancellationToken);

                if (msToken == null || string.IsNullOrEmpty(msToken.AccessToken))
                    throw new MicrosoftOAuthException("MicrosoftOAuth returned null AccessToken", 200);

                // success to refresh ms
                return await GetAllTokens(msToken, sessionCacheBase?.XboxTokens, cancellationToken);
            }
            else
            {
                return sessionCacheBase;
            }
        }


        public async Task<T> LoginFromOAuth(CancellationToken cancellationToken = default)
        {
            var token = await _oauth.RequestNewTokens(cancellationToken);

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
                throw new MicrosoftOAuthException("MicrosoftOAuth returned null AccessToken", 200);

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
            if (msToken == null)
                throw new ArgumentNullException(nameof(msToken));
            if (string.IsNullOrEmpty(msToken.AccessToken))
                throw new ArgumentException("Empty msToken");

            var sessionCache = await GetAllTokens(msToken, null, cancellationToken);
            await saveSessionCache(sessionCache);
            return sessionCache;
        }

        private async Task<T> GetAllTokens(MicrosoftOAuthResponse msToken, XboxAuthTokens? xboxTokens, CancellationToken cancellationToken = default)
        { 
            xboxTokens = await _xbox.GetTokens(msToken.AccessToken!, xboxTokens, this.RelyingParty);
            if (string.IsNullOrEmpty(xboxTokens?.XstsToken?.Token))
                throw new XboxAuthException("XboxLive returned empty xsts token", 200);
            if (string.IsNullOrEmpty(xboxTokens?.XstsToken?.UserHash))
                throw new XboxAuthException("XboxLive returned empty userHash", 200);

            var session = await GetAllTokens(xboxTokens?.XstsToken!, cancellationToken);
            session.MicrosoftOAuthToken = msToken;
            session.XboxTokens = xboxTokens;
            return session;
        }

        protected abstract Task<T> GetAllTokens(XboxAuthResponse xsts, CancellationToken cancellationToken = default);

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
