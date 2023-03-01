using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxAuthStrategyBuilder : JEAuthenticationBuilder
    {
        private readonly HttpClient _httpClient;
        private readonly IMicrosoftOAuthStrategy _oAuthStrategy;
        public ISessionSource<XboxAuthTokens>? SessionSource { get; set; }
        public bool UseCaching { get; set; } = true;
        private IXboxAuthStrategy? strategy;

        public XboxAuthStrategyBuilder(
            HttpClient httpClient,
            IMicrosoftOAuthStrategy oAuthStrategy) =>
            (_httpClient, _oAuthStrategy) = (httpClient, oAuthStrategy);

        public XboxAuthStrategyBuilder WithSessionSource(ISessionSource<XboxAuthTokens> sessionSource)
        {
            SessionSource = sessionSource;
            return this;
        }

        public XboxAuthStrategyBuilder WithCaching(bool useCaching)
        {
            UseCaching = useCaching;
            return this;
        }

        public XboxAuthStrategyBuilder UseBasicStrategy()
        {
            UseStrategy(new BasicXboxAuthStrategy(_httpClient, _oAuthStrategy));
            return this;
        }

        public XboxAuthStrategyBuilder UseStrategy(IXboxAuthStrategy strategy)
        {
            this.strategy = strategy;
            return this;
        }

        public IXboxAuthStrategy CreateCachingStrategy(IXboxAuthStrategy strategy)
        {
            return new CachingXboxAuthStrategy(strategy, getOrCreateSessionSource());
        }

        private IXboxAuthStrategy withCachingIfRequired(IXboxAuthStrategy strategy)
        {
            if (UseCaching)
                return CreateCachingStrategy(strategy);
            else
                return strategy;
        }

        private ISessionSource<XboxAuthTokens> getOrCreateSessionSource()
        {
            if (SessionSource == null)
                return new InMemorySessionSource<XboxAuthTokens>();
            else
                return SessionSource;
        }

        public IXboxAuthStrategy Build()
        {
            return strategy;
        }
    }
}