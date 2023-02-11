using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private readonly XboxGameAuthenticationParameters _parameters;
        private ISessionSource<XboxAuthTokens> XboxSessionSource;

        public XboxAuthBuilder(
            XboxGameAuthenticationParameters parameters)
        {
            this._parameters = parameters;

            if (parameters.OAuthStrategy == null)
                throw new ArgumentException("OAuthStrategy was null");

            if (parameters.SessionStorage == null)
                XboxSessionSource = new InMemorySessionSource<XboxAuthTokens>();
            else
                XboxSessionSource = new XboxSessionSource(parameters.SessionStorage);
        }

        public XboxAuthBuilder WithBasicXboxAuth()
        {
            var httpClient = _parameters.HttpClient ?? HttpHelper.DefaultHttpClient.Value;
            WithXboxAuthStrategy(oAuthStrategy => new BasicXboxAuthStrategy(httpClient, XboxSessionSource));
            return this;
        }

        public XboxAuthBuilder WithXboxAuthStrategy(Func<IMicrosoftOAuthStrategy, IXboxAuthStrategy> factory)
        {
            if (_parameters.OAuthStrategy == null)
                throw new InvalidOperationException("OAuthStrategy was null");

            var xboxAuthStrategy = factory.Invoke(_parameters.OAuthStrategy);
            WithXboxAuthStrategy(xboxAuthStrategy);
            return this;
        }


        public XboxAuthBuilder WithXboxAuthStrategy(IXboxAuthStrategy xboxAuthStrategy)
        {
            _parameters.XboxAuthStrategy = xboxAuthStrategy;
            return this;
        }

        public XboxAuthBuilder WithXboxSessionSource(ISessionSource<XboxAuthTokens> source)
        {
            XboxSessionSource = source;
            return this;
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            if (this._parameters.Executor == null)
            {
                throw new InvalidOperationException();
            }
            if (this._parameters.OAuthStrategy == null)
            {
                throw new InvalidOperationException();
            }
            if (this._parameters.XboxAuthStrategy == null)
            {
                throw new InvalidOperationException();
            }
            if (this._parameters.GameAuthenticator == null)
            {
                throw new InvalidOperationException();
            }
            if (this._parameters.SessionStorage == null)
            {
                throw new InvalidOperationException();
            }

            return this._parameters.Executor.Authenticate(
                this._parameters.OAuthStrategy, 
                this._parameters.XboxAuthStrategy, 
                this._parameters.GameAuthenticator, 
                this._parameters.SessionStorage);
        }
    }
}