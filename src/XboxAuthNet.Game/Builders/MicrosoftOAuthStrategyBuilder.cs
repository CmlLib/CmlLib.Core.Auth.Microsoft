using System;
using System.Net.Http;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.OAuthStrategies;

namespace XboxAuthNet.Game.Builders
{
    public class MicrosoftOAuthStrategyBuilder<T> : MethodChaining<T>
    {
        private readonly MicrosoftOAuthCodeApiClient _oAuthClient;
        public ISessionSource<MicrosoftOAuthResponse>? SessionSource { get; set; }
        public bool UseCaching { get; set; } = true;
        private Func<IMicrosoftOAuthStrategy>? strategyGenerator;

        public MicrosoftOAuthStrategyBuilder(T returning, MicrosoftOAuthClientInfo clientInfo, HttpClient httpClient)
         : this(returning, clientInfo.CreateApiClientForOAuthCode(httpClient)) {}

        public MicrosoftOAuthStrategyBuilder(T returning, MicrosoftOAuthCodeApiClient apiClient) : base(returning)
             => _oAuthClient = apiClient;

        public T WithSessionSource(ISessionSource<MicrosoftOAuthResponse> sessionSource)
        {
            SessionSource = sessionSource;
            return GetThis();
        }

        public T WithCaching(bool useCaching)
        {
            UseCaching = useCaching;
            return GetThis();
        }

        public T UseSilentStrategy()
        {
            UseStrategy(() => 
            {
                var strategy = new SilentMicrosoftOAuthStrategy(_oAuthClient, getOrCreateSessionSource());
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseInteractiveStrategy() => 
            UseInteractiveStrategy(builder => builder, new MicrosoftOAuthParameters());
        
        public T UseInteractiveStrategy(MicrosoftOAuthParameters parameters) =>
            UseInteractiveStrategy(builder => builder, parameters);

        public T UseInteractiveStrategy(Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker) =>
            UseInteractiveStrategy(builderInvoker, new MicrosoftOAuthParameters());

        public T UseInteractiveStrategy(
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker, 
            MicrosoftOAuthParameters parameters)
        {
            UseStrategy(() => 
            {
                var builder = new MicrosoftOAuthCodeFlowBuilder(_oAuthClient);
                builderInvoker.Invoke(builder);

                var codeFlow = builder.Build();
                var strategy = new InteractiveMicrosoftOAuthStrategy(codeFlow, parameters);

                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T FromMicrosoftOAuthResponse(MicrosoftOAuthResponse response)
        {
            UseStrategy(() => 
            {
                var strategy = new MicrosoftOAuthStrategyFromResponse(response);
                return withCachingIfRequired(strategy);
            });
            return GetThis();
        }

        public T UseStrategy(Func<IMicrosoftOAuthStrategy> strategy)
        {
            this.strategyGenerator = strategy;
            return GetThis();
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
            if (strategyGenerator == null)
                throw new InvalidOperationException("Set OAuth strategy first");
            return strategyGenerator.Invoke();
        }
    }
}