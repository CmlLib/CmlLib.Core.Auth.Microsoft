using System;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JEAuthBuilder : 
        IBuilderWithMicrosoftOAuthStrategy<JEAuthBuilder>, 
        IBuilderWithXboxAuthStrategy<JEAuthBuilder>
    {
        private readonly HttpClient _httpClient;
        private readonly MicrosoftOAuthClientInfo _clientInfo;

        private IXboxGameAuthenticator? _gameAuthenticator;
        private IMicrosoftOAuthStrategy? _oAuthStrategy;
        private IXboxAuthStrategy? _xboxAuthStrategy;
        private ISessionStorage? _sessionStorage;
        private ISessionSource<XboxGameSession>? _sessionSource;

        public JEAuthBuilder(HttpClient httpClient, MicrosoftOAuthClientInfo clientInfo)
        {
            this._httpClient = httpClient;
            this._clientInfo = clientInfo;
        }

        public MicrosoftOAuthStrategyFactoryContext MicrosoftOAuthBuilderContext => 
            createMicrosoftOAuthBuilderContext();

        public XboxAuthStrategyFactoryContext XboxAuthStrategyBuilderContext =>
            createXboxAuthStrategyFactoryContext();

        private MicrosoftOAuthStrategyFactoryContext createMicrosoftOAuthBuilderContext()
        {
            var context = new MicrosoftOAuthStrategyFactoryContext()
            {
                ClientId = _clientInfo.ClientId,
                Scopes = _clientInfo.Scopes,
                HttpClient = _httpClient
            };

            if (_sessionStorage != null)
                context.SessionSource = new MicrosoftOAuthSessionSource(_sessionStorage);

            return context;
        }

        private XboxAuthStrategyFactoryContext createXboxAuthStrategyFactoryContext()
        {
            return new XboxAuthStrategyFactoryContext
            {
                HttpClient = _httpClient,
                OAuthStrategy = _oAuthStrategy
            };
        }

        public JEAuthBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this._sessionStorage = sessionStorage;
            return this;
        }

        public JEAuthBuilder WithSessionSource(ISessionSource<XboxGameSession> sessionSource)
        {
            this._sessionSource = sessionSource;
            return this;
        }

        public JEAuthBuilder WithMicrosoftOAuth(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            this._oAuthStrategy = oAuthStrategy;
            return this;
        }

        public JEAuthBuilder WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this._xboxAuthStrategy = xboxAuthStrategy;
            return this;
        }

        public JEAuthBuilder WithGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            this._gameAuthenticator = authenticator;
            return this;
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            if (_gameAuthenticator == null)
            {
                throw new InvalidOperationException();
            }
            if (_xboxAuthStrategy == null)
            {
                throw new InvalidOperationException();
            }
            if (_sessionSource == null)
            {
                throw new InvalidOperationException();
            }

            return _gameAuthenticator.Authenticate(_xboxAuthStrategy, _sessionSource);
        }
    }
}