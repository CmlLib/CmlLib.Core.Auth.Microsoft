using System;
using System.Net.Http;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftOAuthStrategyBuilder : JEAuthenticationBuilder
    {
        private readonly MicrosoftOAuthCodeApiClient _oAuthClient;
        public ISessionSource<MicrosoftOAuthResponse>? SessionSource { get; set; }
        public bool UseCaching { get; set; } = true;
        private IMicrosoftOAuthStrategy? strategy;

        public MicrosoftOAuthStrategyBuilder(MicrosoftOAuthClientInfo clientInfo, HttpClient httpClient)
         : this(clientInfo.CreateApiClientForOAuthCode(httpClient)) {}

        public MicrosoftOAuthStrategyBuilder(MicrosoftOAuthCodeApiClient apiClient)
             => _oAuthClient = apiClient;

        public MicrosoftOAuthStrategyBuilder WithSessionSource(ISessionSource<MicrosoftOAuthResponse> sessionSource)
        {
            SessionSource = sessionSource;
            return this;
        }

        public MicrosoftOAuthStrategyBuilder WithCaching(bool useCaching)
        {
            UseCaching = useCaching;
            return this;
        }

        public MicrosoftOAuthStrategyBuilder UseSilentStrategy()
        {
            var strategy = new SilentMicrosoftOAuthStrategy(_oAuthClient, getOrCreateSessionSource());
            UseStrategy(withCachingIfRequired(strategy));
            return this;
        }

        public MicrosoftOAuthStrategyBuilder UseInteractiveStrategy() => 
            UseInteractiveStrategy(builder => builder, new MicrosoftOAuthParameters());
        
        public MicrosoftOAuthStrategyBuilder CreateInteractiveStrategy(MicrosoftOAuthParameters parameters) =>
            UseInteractiveStrategy(builder => builder, parameters);

        public MicrosoftOAuthStrategyBuilder CreateInteractiveStrategy(Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker) =>
            UseInteractiveStrategy(builderInvoker, new MicrosoftOAuthParameters());

        public MicrosoftOAuthStrategyBuilder UseInteractiveStrategy(
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker, 
            MicrosoftOAuthParameters parameters)
        {
            var builder = new MicrosoftOAuthCodeFlowBuilder(_oAuthClient);
            builderInvoker.Invoke(builder);

            var codeFlow = builder.Build();
            var strategy = new InteractiveMicrosoftOAuthStrategy(codeFlow, parameters);
            UseStrategy(withCachingIfRequired(strategy));
            return this;
        }

        private MicrosoftOAuthStrategyBuilder UseStrategy(IMicrosoftOAuthStrategy strategy)
        {
            this.strategy = strategy;
            return this;
        }

        public IMicrosoftOAuthStrategy CreateCachingStrategy(IMicrosoftOAuthStrategy strategy)
        {
            return new CachingMicrosoftOAuthStrategy(strategy, getOrCreateSessionSource());
        }

        private IMicrosoftOAuthStrategy withCachingIfRequired(IMicrosoftOAuthStrategy strategy)
        {
            if (UseCaching)
                return CreateCachingStrategy(strategy);
            else
                return strategy;
        }

        private ISessionSource<MicrosoftOAuthResponse> getOrCreateSessionSource()
        {
            if (SessionSource == null)
                return new InMemorySessionSource<MicrosoftOAuthResponse>();
            else
                return SessionSource;
        }

        public IMicrosoftOAuthStrategy Build()
        {
            return strategy;
        }
    }
}