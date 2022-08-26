using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
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
            this.parameters = new LoginHandlerParameters();
        }

        private readonly LoginHandlerParameters parameters;
        public ICacheManager<TSession>? CacheManager { get; private set; }

        public TBuilder WithMicrosoftOAuthApi(IMicrosoftOAuthApi oauthApi)
        {
            if (oauthApi == null)
                throw new ArgumentNullException(nameof(oauthApi));

            parameters.MicrosoftOAuthApi = oauthApi;
            return (TBuilder)this;
        }

        public TBuilder WithXboxLiveApi(IXboxLiveApi xboxApi)
        {
            if (xboxApi == null)
                throw new ArgumentNullException(nameof(xboxApi));

            parameters.XboxLiveApi = xboxApi;
            return (TBuilder)this;
        }

        public TBuilder WithRelyingParty(string relyingParty)
        {
            if (string.IsNullOrEmpty(relyingParty))
                throw new ArgumentNullException(nameof(relyingParty));

            parameters.RelyingParty = relyingParty;
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

        protected abstract AbstractLoginHandler<TSession> BuildInternal(LoginHandlerParameters parameters);

        public LoginHandlerParameters BuildParameters()
        {
            if (parameters.MicrosoftOAuthApi == null)
            {
                if (!IsDefaultClientIdAvailable)
                    throw new InvalidOperationException("MicrosoftOAuthApi was not set.");

                parameters.MicrosoftOAuthApi = MicrosoftOAuthApiBuilder.Create(GetDefaultClientId())
                    .WithHttpClient(this.HttpClient)
                    .WithScope(XboxAuth.XboxScope)
                    .Build();
            }

            if (parameters.XboxLiveApi == null)
            {
                parameters.XboxLiveApi = new XboxAuthNetApi(new XboxAuth(this.HttpClient));
            }

            if (this.CacheManager == null)
            {
                var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_xsession.json");
                this.CacheManager = new JsonFileCacheManager<TSession>(defaultPath);
            }

            return parameters;
        }

        public AbstractLoginHandler<TSession> Build()
        {
            return BuildInternal(BuildParameters());
        }
    }
}
