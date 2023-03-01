using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JEAuthenticationBuilder
    {
        private IXboxGameAuthenticator? _gameAuthenticator;
        private IXboxAuthStrategy? _xboxAuthStrategy;
        private ISessionSource<XboxGameSession>? _sessionSource;

        public MicrosoftOAuthStrategyBuilder MicrosoftOAuth => new MicrosoftOAuthStrategyBuilder(
            JELoginHandler.DefaultMicrosoftOAuthClientInfo, httpClient);

        public XboxAuthStrategyBuilder XboxAuth => new XboxAuthStrategyBuilder(httpClient, MicrosoftOAuth.Build());

        public JEAuthenticationBuilder WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this._xboxAuthStrategy = xboxAuthStrategy;
            return this;
        }

        public JEAuthenticationBuilder WithGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            this._gameAuthenticator = authenticator;
            return this;
        }

        public JEAuthenticationBuilder WithSessionSource(ISessionSource<XboxGameSession> sessionSource)
        {
            this._sessionSource = sessionSource;
            return this;
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            if (_gameAuthenticator == null)
                throw new InvalidOperationException();
            if (_xboxAuthStrategy == null)
                throw new InvalidOperationException();
            if (_sessionSource == null)
                throw new InvalidOperationException();

            return _gameAuthenticator.Authenticate(_xboxAuthStrategy, _sessionSource);
        }
    }
}