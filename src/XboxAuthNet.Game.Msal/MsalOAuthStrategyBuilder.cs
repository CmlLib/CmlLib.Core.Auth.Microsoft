using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Msal.OAuth;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal
{
    public class MsalOAuthStrategyBuilder<T> : MethodChaining<T>
    {
        public bool UseCaching { get; set; } = true;
        public ISessionSource<MicrosoftOAuthResponse>? SessionSource { get; set; }

        private Func<IMicrosoftOAuthStrategy>? strategyGenerator;

        public MsalOAuthStrategyBuilder(T returning)
         : base(returning)
        {

        }

        public T UseSilentStrategy(IPublicClientApplication app) => UseSilentStrategy(app, null);

        public T UseSilentStrategy(IPublicClientApplication app, string? loginHint)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalSilentStrategy(app, loginHint);
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseInteractiveStrategy(IPublicClientApplication app)
        {
            UseStrategy(() =>
            {
                var strategy = new MsalInteractiveStrategy(app);
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseEmbeddedWebViewStrategy(IPublicClientApplication app)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalInteractiveStrategy(app);
                strategy.UseDefaultWebViewOption = false;
                strategy.UseEmbeddedWebView = true;
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseSystemBrowserStrategy(IPublicClientApplication app)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalInteractiveStrategy(app);
                strategy.UseDefaultWebViewOption = false;
                strategy.UseEmbeddedWebView = false;
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseDeviceCodeStrategy(IPublicClientApplication app, Func<DeviceCodeResult, Task> deviceResultCallback)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalDeviceCodeStrategy(app, deviceResultCallback);
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T FromAuthenticationResult(AuthenticationResult result)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalOAuthStrategyFromResult(result);
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T FromParameterBuilder<TBuilder>(AbstractAcquireTokenParameterBuilder<TBuilder> builder)
            where TBuilder : AbstractAcquireTokenParameterBuilder<TBuilder>
        {
            UseStrategy(() => 
            {
                var strategy = new MsalOAuthStrategyFromBuilder<TBuilder>(builder);
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseStrategy(Func<IMicrosoftOAuthStrategy> strategy)
        {
            strategyGenerator = strategy;
            return GetThis();
        }

        public T WithCaching() => WithCaching(true);

        public T WithCaching(bool value)
        {
            UseCaching = value;
            return GetThis();
        }

        public T WithSessionSource(ISessionSource<MicrosoftOAuthResponse> sessionSource)
        {
            this.SessionSource = sessionSource;
            return GetThis();
        }

        private IMicrosoftOAuthStrategy withCachingIfRequired(IMicrosoftOAuthStrategy strategy)
        {
            if (UseCaching)
            {
                return CreateCachingStrategy(strategy);
            }
            else
                return strategy;
        }

        public IMicrosoftOAuthStrategy CreateCachingStrategy(IMicrosoftOAuthStrategy innerStrategy)        
        {     
            return new CachingMsalOAuthStrategy(innerStrategy, GetOrCreateSessionSource());
        }

        private ISessionSource<MicrosoftOAuthResponse> GetOrCreateSessionSource()
        {
            if (SessionSource == null)
                return new InMemorySessionSource<MicrosoftOAuthResponse>();
            else
                return SessionSource;
        }

        public IMicrosoftOAuthStrategy Build()
        {
            if (strategyGenerator == null)  
                throw new InvalidOperationException("Set Strategy first");

            var strategy = strategyGenerator.Invoke();
            return strategy;
        }
    }
}