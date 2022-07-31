using CmlLib.Core.Auth.Microsoft.Cache;
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
        private readonly HttpClient HttpClient;

        public AbstractLoginHandlerBuilder(string clientId, HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this.XboxLiveApi = new XboxAuthNetApi(
                new MicrosoftOAuth(clientId, XboxAuth.XboxScope, httpClient),
                new XboxAuth(httpClient));

            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
            this.CacheManager = new JsonFileCacheManager<TSession>(defaultPath);
        }

        protected IXboxLiveApi XboxLiveApi { get; private set; }
        protected ICacheManager<TSession> CacheManager { get; private set; }

        public TBuilder SetMicrosoftOAuthHandler(string id, string scope)
        {
            this.XboxLiveApi = new XboxAuthNetApi(
                new MicrosoftOAuth(id, scope, HttpClient),
                new XboxAuth(HttpClient));
            return (TBuilder)this;
        }

        public TBuilder SetMicrosoftOAuthHandler(MicrosoftOAuth oAuth)
        {
            this.XboxLiveApi = new XboxAuthNetApi(
                oAuth, 
                new XboxAuth(HttpClient));
            return (TBuilder)this;
        }

        public TBuilder SetXboxLiveApi(IXboxLiveApi xboxApi)
        {
            this.XboxLiveApi = xboxApi;
            return (TBuilder)this;
        }

        public TBuilder SetCacheManager(ICacheManager<TSession> cacheManager)
        {
            this.CacheManager = cacheManager;
            return (TBuilder)this;
        }

        public TBuilder SetJsonCacheManager(string path)
        {
            return SetCacheManager(new JsonFileCacheManager<TSession>(path));
        }
    }
}
