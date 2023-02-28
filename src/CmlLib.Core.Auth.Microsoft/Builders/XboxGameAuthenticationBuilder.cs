using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameAuthenticationBuilder : 
        IBuilderWithMicrosoftOAuthStrategy<XboxGameAuthenticationBuilder>, 
        IBuilderWithXboxAuthStrategy<XboxGameAuthenticationBuilder>
    {
        private readonly MicrosoftOAuthStrategyFactoryContext _oAuthContext;
        private IMicrosoftOAuthStrategy? _oAuthStrategy;
        private readonly XboxAuthStrategyFactoryContext _xboxAuthContext;
        private IXboxAuthStrategy? _xboxAuthStrategy;

        private IXboxGameAuthenticator? _gameAuthenticator;
        private ISessionStorage? _sessionStorage;
        private ISessionSource<XboxGameSession>? _sessionSource;

        private readonly IXboxGameAuthentcationExecutor _executor;

        public XboxGameAuthenticationBuilder(
            IXboxGameAuthentcationExecutor executor,
            MicrosoftOAuthStrategyFactoryContext oAuthContext,
            XboxAuthStrategyFactoryContext xboxAuthContext) =>
            (_executor, _oAuthContext, _xboxAuthContext) = (executor, oAuthContext, xboxAuthContext);

        public MicrosoftOAuthStrategyFactoryContext MicrosoftOAuthBuilderContext => 
            createMicrosoftOAuthBuilderContext();

        public XboxAuthStrategyFactoryContext XboxAuthStrategyBuilderContext =>
            createXboxAuthStrategyFactoryContext();

        private MicrosoftOAuthStrategyFactoryContext createMicrosoftOAuthBuilderContext()
        {
            if (_sessionStorage != null)
                _oAuthContext.SessionSource = new MicrosoftOAuthSessionSource(_sessionStorage);

            return _oAuthContext;
        }

        private XboxAuthStrategyFactoryContext createXboxAuthStrategyFactoryContext()
        {
            if (_sessionStorage != null)
                _xboxAuthContext.SessionSource = new XboxSessionSource(_sessionStorage);

            return _xboxAuthContext;
        }

        public XboxGameAuthenticationBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this._sessionStorage = sessionStorage;
            return this;
        }

        public XboxGameAuthenticationBuilder WithMicrosoftOAuth(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            this._oAuthStrategy = oAuthStrategy;
            return this;
        }

        public XboxGameAuthenticationBuilder WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this._xboxAuthStrategy = xboxAuthStrategy;
            return this;
        }

        public XboxGameAuthenticationBuilder WithGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            this._gameAuthenticator = authenticator;
            return this;
        }

        public XboxGameAuthenticationBuilder WithSessionSource(ISessionSource<XboxGameSession> sessionSource)
        {
            this._sessionSource = sessionSource;
            return this;
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            return _executor.ExecuteAsync(_gameAuthenticator, _xboxAuthStrategy, _sessionSource);
        }
    }
}