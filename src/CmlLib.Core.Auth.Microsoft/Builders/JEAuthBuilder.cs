using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JEAuthBuilder
    {
        private IXboxGameAuthenticator? _gameAuthenticator;
        private IMicrosoftOAuthStrategy? _oAuthStrategy;
        private IXboxAuthStrategy? _xboxAuthStrategy;
        private ISessionStorage? _sessionStorage;
        private ISessionSource<XboxGameSession>? _sessionSource;

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

        public JEAuthBuilder WithMicrosoftOAuth(Func<MicrosoftOAuthBuilderContext, IMicrosoftOAuthStrategy> factory)
        {
            var context = new MicrosoftOAuthBuilderContext()
            {
                ClientId = clientID,
                Scopes = scopes,
                HttpClient = httpClient
            };

            if (_sessionStorage != null)
                context.SessionSource = new MicrosoftOAuthSessionSource(_sessionStorage);

            var strategy = factory.Invoke(context);
            return WithMicrosoftOAuth(strategy);
        }

        public JEAuthBuilder WithSilentMicrosoftOAuth()
        {
            return WithMicrosoftOAuth(context => {
                var apiClient = createMicrosoftOAuthApiClient(context);
                var factory = new MicrosoftOAuthStrategyFactory(apiClient);
                factory.SessionSource = context.SessionSource;
                var strategy = factory.CreateSilentStrategy();
                return factory.CreateCachingStrategy(strategy);
            });
        }

        public JEAuthBuilder WithInteractiveMicrosoftOAuth(MicrosoftOAuthParameters parameters)
        {
            return WithInteractiveMicrosoftOAuth(builder => builder, parameters);
        }

        public JEAuthBuilder WithInteractiveMicrosoftOAuth(
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker, 
            MicrosoftOAuthParameters parameters)
        {
            return WithMicrosoftOAuth(context => {
                var apiClient = createMicrosoftOAuthApiClient(context);
                var factory = new MicrosoftOAuthStrategyFactory(apiClient);
                factory.SessionSource = context.SessionSource;
                var strategy = factory.CreateInteractiveStrategy(builderInvoker, parameters);
                return factory.CreateCachingStrategy(strategy);
            });
        }

        private MicrosoftOAuthCodeApiClient createMicrosoftOAuthApiClient(MicrosoftOAuthBuilderContext context)
        {
            var apiClient = new MicrosoftOAuthCodeApiClient(
                context.ClientId ?? throw new InvalidOperationException(),
                context.Scopes ?? throw new InvalidOperationException(),
                context.HttpClient ?? HttpHelper.DefaultHttpClient.Value);
            return apiClient;
        }

        public JEAuthBuilder WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this._xboxAuthStrategy = xboxAuthStrategy;
            return this;
        }

        public JEAuthBuilder WithXboxAuth(Func<XboxAuthStrategyBuilder, IXboxAuthStrategy> builderInvoker)
        {
            var builder = new XboxAuthStrategyBuilder();
            var strategy = builderInvoker.Invoke(builder);
            return WithXboxAuth(strategy);
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

            return _gameAuthenticator.Authenticate(_xboxAuthStrategy);
        }
    }
}