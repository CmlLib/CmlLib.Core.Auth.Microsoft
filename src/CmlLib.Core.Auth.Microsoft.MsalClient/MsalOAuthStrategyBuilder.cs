using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalOAuthStrategyBuilder<T> : MethodChaining<T>
    {
        private readonly IPublicClientApplication _app;
        private Func<IMicrosoftOAuthStrategy>? strategyGenerator;

        public MsalOAuthStrategyBuilder(T returning, IPublicClientApplication app)
         : base(returning)
        {
            _app = app;
        }

        public T UseInteractiveStrategy()
        {
            UseStrategy(() =>
            {
                var strategy = new MsalInteractiveStrategy(_app);
                return strategy;
            });
            return GetThis();
        }

        public T UseEmbeddedWebViewStrategy()
        {
            UseStrategy(() => 
            {
                var strategy = new MsalInteractiveStrategy(_app);
                strategy.UseDefaultWebViewOption = false;
                strategy.UseEmbeddedWebView = true;
                return strategy;
            });
            return GetThis();
        }

        public T UseSystemBrowserStrategy()
        {
            UseStrategy(() => 
            {
                var strategy = new MsalInteractiveStrategy(_app);
                strategy.UseDefaultWebViewOption = false;
                strategy.UseEmbeddedWebView = false;
                return strategy;
            });
            return GetThis();
        }

        public T UseDeviceCodeStrategy(Func<DeviceCodeResult, Task> deviceResultCallback)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalDeviceCodeStrategy(_app, deviceResultCallback);
                return strategy;
            });
            return GetThis();
        }

        public T FromAuthenticationResult(AuthenticationResult result)
        {
            UseStrategy(() => 
            {
                var strategy = new MsalOAuthStrategyFromResult(result);
                return strategy;
            });
            return GetThis();
        }

        public T FromParameterBuilder<TBuilder>(AbstractAcquireTokenParameterBuilder<TBuilder> builder)
            where TBuilder : AbstractAcquireTokenParameterBuilder<TBuilder>
        {
            UseStrategy(() => 
            {
                var strategy = new MsalOAuthStrategyFromBuilder<TBuilder>(builder);
                return strategy;
            });
            return GetThis();
        }

        public T UseStrategy(Func<IMicrosoftOAuthStrategy> strategy)
        {
            strategyGenerator = strategy;
            return GetThis();
        }

        private IMicrosoftOAuthStrategy withCachingIfRequired(IMicrosoftOAuthStrategy strategy)
        {
            
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