using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        protected XboxGameAuthenticationParameters Parameters { get; private set; }
        protected ISessionSource<XboxAuthTokens> XboxSessionSource { get; private set; }

        public XboxAuthBuilder(XboxGameAuthenticationParameters parameters)
        {
            this.Parameters = parameters;

            if (parameters.SessionStorage == null)
                XboxSessionSource = new InMemorySessionSource<XboxAuthTokens>();
            else
                XboxSessionSource = new XboxSessionSource(parameters.SessionStorage);
        }

        public XboxAuthBuilder WithXboxSessionSource(ISessionSource<XboxAuthTokens> source)
        {
            XboxSessionSource = source;
            return this;
        }

        public XboxAuthBuilder WithXboxAuthStrategy(IXboxAuthStrategy xboxAuthStrategy)
        {
            Parameters.XboxAuthStrategy = xboxAuthStrategy;
            return this;
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            var gameAuthenticator = this.Parameters.GameAuthenticator;
            var xboxAuthStrategy = this.Parameters.XboxAuthStrategy;
            var cacheStorage = this.Parameters.SessionStorage;

            if (this.Parameters.Executor == null)
            {
                throw new InvalidOperationException();
            }
            if (gameAuthenticator == null)
            {
                throw new InvalidOperationException();
            }
            if (xboxAuthStrategy == null)
            {
                throw new InvalidOperationException();
            }
            if (cacheStorage == null)
            {
                throw new InvalidOperationException();
            }

            return this.Parameters.Executor.Authenticate(gameAuthenticator, xboxAuthStrategy, cacheStorage);
        }
    }
}