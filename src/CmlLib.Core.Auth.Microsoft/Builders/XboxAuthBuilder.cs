using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        protected XboxGameAuthenticationParameters Parameters { get; private set; }

        public XboxAuthBuilder(XboxGameAuthenticationParameters parameters)
        {
            this.Parameters = parameters;
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
            var cacheStorage = this.Parameters.CacheStorage;

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