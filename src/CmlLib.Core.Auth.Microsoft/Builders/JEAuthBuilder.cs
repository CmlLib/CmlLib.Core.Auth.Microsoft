using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JEAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        private ISessionSource<XboxGameSession> _sessionSource;
        private readonly XboxGameAuthenticationParameters _parameters;

        public JEAuthBuilder(XboxGameAuthenticationParameters parameters)
        {
            _parameters = parameters;

            if (parameters.SessionStorage == null)
                _sessionSource = new InMemorySessionSource<XboxGameSession>();
            else
                _sessionSource = new SessionFromStorage<XboxGameSession>("G", parameters.SessionStorage);
        }

        public JEAuthBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this._parameters.SessionStorage = sessionStorage;
            return this;
        }

        public JEAuthBuilder WithSessionSource(ISessionSource<XboxGameSession> sessionSource)
        {
            this._sessionSource = sessionSource;
            return this;
        }

        public AuthenticationBuilder Interactively()
        {
            var authenticator = new DummyGameAuthenticator();
            WithCachingGameAuthenticator(authenticator);

            return new AuthenticationBuilder(this._parameters, JELoginHandler.DefaultMicrosoftOAuthClientInfo)
                .WithExecutor(builder => builder
                    .WithMicrosoftOAuth()
                    .ExecuteAsync());
        }

        public AuthenticationBuilder noname()
        {
            var authenticator = new DummyGameAuthenticator();
            WithCachingGameAuthenticator(authenticator);

            return new AuthenticationBuilder(this._parameters, JELoginHandler.DefaultMicrosoftOAuthClientInfo)
                .WithExecutor(builder => builder
                    .WithMicrosoftOAuth()
                    .ExecuteAsync());
        }

        public AuthenticationBuilder Silently()
        {
            var authenticator = new DummyGameAuthenticator();
            WithCachingGameAuthenticator(new SilentXboxGameAuthenticator(_sessionSource, authenticator));

            return new AuthenticationBuilder(this._parameters, JELoginHandler.DefaultMicrosoftOAuthClientInfo)
                .WithExecutor(builder => builder
                    .WithSilentMicrosoftOAuth()
                    .ExecuteAsync());
        }

        private JEAuthBuilder WithCachingGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            authenticator = createCachingAuthenticator(authenticator);
            return WithGameAuthenticator(createCachingAuthenticator(authenticator));
        }

        private JEAuthBuilder WithGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            this._parameters.GameAuthenticator = authenticator;
            return this;
        }

        private IXboxGameAuthenticator createCachingAuthenticator(IXboxGameAuthenticator authenticator)
        {
            return new CachingXboxGameSession(_sessionSource, authenticator);
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            return noname().ExecuteAsync();
        }
    }
}