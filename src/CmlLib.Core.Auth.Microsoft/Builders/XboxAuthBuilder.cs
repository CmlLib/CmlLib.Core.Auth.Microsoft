using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class XboxAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private XboxGameAuthenticationParameters _parameters;
        private ISessionSource<XboxAuthTokens> _sessionSource;
        private IMicrosoftOAuthStrategy _oAuthStrategy;

        public XboxAuthBuilder(
            IMicrosoftOAuthStrategy oAuthStrategy,
            XboxGameAuthenticationParameters parameters)
        {
            this._parameters = parameters;
            this._oAuthStrategy = oAuthStrategy;

            if (parameters.SessionStorage == null)
                _sessionSource = new InMemorySessionSource<XboxAuthTokens>();
            else
                _sessionSource = new XboxSessionSource(parameters.SessionStorage);
        }

        public JEAuthBuilder WithBasicXboxAuth()
        {
            return WithXboxAuthCachingStrategy(oAuthStrategy => new BasicXboxAuthStrategy(_parameters.HttpClient, oAuthStrategy));
        }

        public JEAuthBuilder WithXboxAuthCachingStrategy(Func<IMicrosoftOAuthStrategy, IXboxAuthStrategy> factory)
        {
            var strategy = factory.Invoke(this._oAuthStrategy);
            var withCaching = createCachingStrategy(strategy);
            return WithXboxAuthStrategy(withCaching);
        }

        public JEAuthBuilder WithXboxAuthCachingStrategy(IXboxAuthStrategy xboxAuthStrategy)
        {
            var withCaching = createCachingStrategy(xboxAuthStrategy);
            return WithXboxAuthStrategy(withCaching);
        }

        public JEAuthBuilder WithXboxAuthStrategy(IXboxAuthStrategy xboxAuthStrategy)
        {
            _parameters.XboxAuthStrategy = xboxAuthStrategy;
            return new JEAuthBuilder(_parameters);
        }

        private IXboxAuthStrategy createCachingStrategy(IXboxAuthStrategy xboxAuthStrategy)
        {
            return new CachingXboxAuthStrategy(xboxAuthStrategy, _sessionSource);
        }

        public XboxAuthBuilder WithXboxSessionSource(ISessionSource<XboxAuthTokens> source)
        {
            _sessionSource = source;
            return this;
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            return WithBasicXboxAuth().ExecuteAsync();
        }
    }
}