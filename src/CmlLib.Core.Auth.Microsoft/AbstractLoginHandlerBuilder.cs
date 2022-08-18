using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System.IO;
using System.Net.Http;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public abstract class AbstractLoginHandlerBuilder<TBuilder, TSession> 
        where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
        where TSession : SessionCacheBase
    {
        protected readonly HttpClient HttpClient;

        public AbstractLoginHandlerBuilder(string clientId, HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this.MicrosoftOAuthApi = new MicrosoftOAuthApi(new MicrosoftOAuth(clientId, XboxAuth.XboxScope, httpClient));

            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
            this.CacheManager = new JsonFileCacheManager<TSession>(defaultPath);
        }

        public IMicrosoftOAuthApi MicrosoftOAuthApi { get; private set; }
        public ICacheManager<TSession> CacheManager { get; private set; }

        public TBuilder WithMicrosoftOAuthApi(IMicrosoftOAuthApi oauthApi)
        {
            this.MicrosoftOAuthApi = oauthApi;
            return (TBuilder)this;
        }

        public TBuilder WithCacheManager(ICacheManager<TSession> cacheManager)
        {
            this.CacheManager = cacheManager;
            return (TBuilder)this;
        }
    }
}
