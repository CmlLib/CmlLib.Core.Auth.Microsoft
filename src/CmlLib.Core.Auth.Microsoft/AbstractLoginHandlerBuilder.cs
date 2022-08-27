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
        public AbstractLoginHandlerBuilder(LoginHandlerBuilderContext context)
        {
            this.Context = context;
            this.Parameters = new LoginHandlerParameters();
        }

        private LoginHandlerParameters Parameters { get; set; }
        protected LoginHandlerBuilderContext Context { get; private set; }
        public ICacheManager<TSession>? CacheManager { get; private set; }

        public TBuilder With(Action<TBuilder, LoginHandlerBuilderContext> action)
        {
            action.Invoke((TBuilder)this, Context);
            return (TBuilder)this;
        }

        public TBuilder WithMicrosoftOAuthApi(IMicrosoftOAuthApi oauthApi)
        {
            if (oauthApi == null)
                throw new ArgumentNullException(nameof(oauthApi));

            Parameters.MicrosoftOAuthApi = oauthApi;
            return (TBuilder)this;
        }

        public TBuilder WithXboxLiveApi(IXboxLiveApi xboxApi)
        {
            if (xboxApi == null)
                throw new ArgumentNullException(nameof(xboxApi));

            Parameters.XboxLiveApi = xboxApi;
            return (TBuilder)this;
        }

        public TBuilder WithRelyingParty(string relyingParty)
        {
            if (string.IsNullOrEmpty(relyingParty))
                throw new ArgumentNullException(nameof(relyingParty));

            Parameters.RelyingParty = relyingParty;
            return (TBuilder)this;
        }

        public TBuilder WithCacheManager(ICacheManager<TSession> cacheManager)
        {
            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            this.CacheManager = cacheManager;
            return (TBuilder)this;
        }

        protected abstract AbstractLoginHandler<TSession> BuildInternal(LoginHandlerParameters parameters);

        public LoginHandlerParameters BuildParameters()
        {
            if (Parameters.MicrosoftOAuthApi == null)
            {
                if (string.IsNullOrEmpty(Context.ClientId))
                    throw new InvalidOperationException("MicrosoftOAuthApi was null");
                if (Context.HttpClient == null)
                    throw new InvalidOperationException("MicrosoftOAuthApi was null");

                Parameters.MicrosoftOAuthApi = MicrosoftOAuthApiBuilder.Create(Context.ClientId!)
                    .WithHttpClient(Context.HttpClient)
                    .WithScope(XboxAuth.XboxScope)
                    .Build();
            }

            if (Parameters.XboxLiveApi == null)
            {
                if (Context.HttpClient == null)
                    throw new InvalidOperationException("XboxLiveApi was null");

                Parameters.XboxLiveApi = new XboxAuthNetApi(new XboxAuth(Context.HttpClient));
            }

            if (this.CacheManager == null)
            {
                if (string.IsNullOrEmpty(Context.CachePath))
                    throw new InvalidOperationException("CacheManager was null");
                this.CacheManager = new JsonFileCacheManager<TSession>(Context.CachePath!);
            }

            return Parameters;
        }

        public AbstractLoginHandler<TSession> Build()
        {
            return BuildInternal(BuildParameters());
        }
    }
}
