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
        }

        public IMicrosoftOAuthApi? MicrosoftOAuthApi { get; private set; }
        public ICacheManager<TSession>? CacheManager { get; private set; }

        public TBuilder WithMicrosoftOAuthApi(IMicrosoftOAuthApi oauthApi)
        {
            if (oauthApi == null)
                throw new ArgumentNullException(nameof(oauthApi));

            this.MicrosoftOAuthApi = oauthApi;
            return (TBuilder)this;
        }

        public TBuilder WithCacheManager(ICacheManager<TSession> cacheManager)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            this.CacheManager = cacheManager;
            return (TBuilder)this;
        }

        public virtual bool IsDefaultClientIdAvailable => false;
        public string DefaultClientId
        {
            get
            {
                if (!IsDefaultClientIdAvailable)
                    throw new InvalidOperationException("This builder doesn't have a default client id. Please specify client id.");
                return GetDefaultClientId();
            }
        }

        protected virtual string GetDefaultClientId() => throw new NotImplementedException();

        protected abstract AbstractLoginHandler<TSession> BuildInternal();

        public AbstractLoginHandler<TSession> Build()
        {
            if (this.MicrosoftOAuthApi == null)
            {
                if (!IsDefaultClientIdAvailable)
                    throw new InvalidOperationException("MicrosoftOAuthApi was not set.");

                this.MicrosoftOAuthApi = MicrosoftOAuthApiBuilder.Create(GetDefaultClientId())
                    .WithHttpClient(this.HttpClient)
                    .WithScope(XboxAuth.XboxScope)
                    .Build();
            }

            if (this.CacheManager == null)
            {
                var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
                this.CacheManager = new JsonFileCacheManager<TSession>(defaultPath);
            }

            return BuildInternal();
        }
    }
}
