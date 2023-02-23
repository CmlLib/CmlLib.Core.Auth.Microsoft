using System;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftOAuthStrategyFactory
    {
        private readonly MicrosoftOAuthCodeApiClient _oAuthClient;
        public ISessionSource<MicrosoftOAuthResponse>? SessionSource { get; set; }

        public MicrosoftOAuthStrategyFactory(MicrosoftOAuthCodeApiClient apiClient)
             => _oAuthClient = apiClient;

        public IMicrosoftOAuthStrategy CreateSilentStrategy()
        {
            return new SilentMicrosoftOAuthStrategy(_oAuthClient, getOrCreateSessionSource());
        }

        public IMicrosoftOAuthStrategy CreateInteractiveStrategy(
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker, 
            MicrosoftOAuthParameters parameters)
        {
            var builder = new MicrosoftOAuthCodeFlowBuilder(_oAuthClient);
            builderInvoker.Invoke(builder);
            var codeFlow = builder.Build();
            return new InteractiveMicrosoftOAuthStrategy(codeFlow, parameters);
        }

        public IMicrosoftOAuthStrategy CreateCachingStrategy(IMicrosoftOAuthStrategy strategy)
        {
            return new CachingMicrosoftOAuthStrategy(strategy, getOrCreateSessionSource());
        }

        private ISessionSource<MicrosoftOAuthResponse> getOrCreateSessionSource()
        {
            if (SessionSource == null)
                return new InMemorySessionSource<MicrosoftOAuthResponse>();
            else
                return SessionSource;
        }
    }
}