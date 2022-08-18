using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using System;
using System.IO;
using System.Net.Http;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft
{
    public abstract class AbstractLoginHandlerBuilder<TBuilder, TSession> 
        where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
        where TSession : SessionCacheBase
    {
        public HttpClient HttpClient { get; private set; }

        public AbstractLoginHandlerBuilder(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            this.MicrosoftOAuthApi = MicrosoftOAuthApiBuilder.Create(GetDefaultClientId())
                .WithHttpClient(httpClient)
                .WithScope(XboxAuth.XboxScope)
                .Build();

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

        public abstract string GetDefaultClientId();
    }
}
