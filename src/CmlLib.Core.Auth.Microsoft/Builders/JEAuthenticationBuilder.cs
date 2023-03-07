using System;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JEAuthenticationBuilder : XboxGameAuthenticationBuilder<JEAuthenticationBuilder>
    {
        public bool UseCaching { get; set; } = true;
        public bool CheckGameOwnership { get; set; } = false;
        public ISessionSource<JESession>? SessionSource { get; set; }
        private Func<IXboxGameAuthenticator<JESession>>? strategy;

        public JEAuthenticationBuilder(MicrosoftOAuthClientInfo clientInfo)
        {
            MicrosoftOAuth = createOAuthBuilder(clientInfo);
            XboxAuth = createXboxAuthBuilder();
        }

        public MicrosoftOAuthStrategyBuilder<JEAuthenticationBuilder> MicrosoftOAuth { get; private set; }
        public XboxAuthStrategyBuilder<JEAuthenticationBuilder> XboxAuth { get; private set; }

        private MicrosoftOAuthStrategyBuilder<JEAuthenticationBuilder> createOAuthBuilder(MicrosoftOAuthClientInfo clientInfo)
        {
            var builder = new MicrosoftOAuthStrategyBuilder<JEAuthenticationBuilder>(this, clientInfo, getHttpClient());
            return builder;
        }

        private XboxAuthStrategyBuilder<JEAuthenticationBuilder> createXboxAuthBuilder()
        {
            var builder = new XboxAuthStrategyBuilder<JEAuthenticationBuilder>(this, getHttpClient());
            return builder;
        }

        public JEAuthenticationBuilder WithCaching() => WithCaching(true);
        public JEAuthenticationBuilder WithCaching(bool useCaching)
        {
            this.UseCaching = useCaching;
            return GetThis();
        }

        public JEAuthenticationBuilder WithCheckingGameOwnership() => WithCheckingGameOwnership(true);
        public JEAuthenticationBuilder WithCheckingGameOwnership(bool checkGameOwnership)
        {
            this.CheckGameOwnership = checkGameOwnership;
            return GetThis();
        }

        public JEAuthenticationBuilder WithSessionSource(ISessionSource<JESession> sessionSource)
        {
            this.SessionSource = sessionSource;
            return GetThis();
        }

        public JEAuthenticationBuilder UseInteractiveStrategy()
        {
            strategy = createInteractiveJEAuthenticator;
            return GetThis();
        }

        private IXboxGameAuthenticator<JESession> createInteractiveJEAuthenticator()
        {
            var authenticator = new JEAuthenticator(HttpClient!, SessionSource!);

            if (UseCaching)
                return new CachingGameSession<JESession>(authenticator, SessionSource!);
            else
                return authenticator;
        }

        public JEAuthenticationBuilder UseSilentStrategy()
        {
            strategy = createSilentJEAuthenticator;
            return GetThis();
        }

        private IXboxGameAuthenticator<JESession> createSilentJEAuthenticator()
        {
            var authenticator = createInteractiveJEAuthenticator();
            return new SilentXboxGameAuthenticator<JESession>(authenticator, SessionSource!);
        }

        private HttpClient getHttpClient()
        {
            return HttpClient ?? HttpHelper.DefaultHttpClient.Value;
        }

        public override IAuthenticationExecutor Build()
        {
            // SessionStorage
            if (SessionStorage == null)
                SessionStorage = new InMemorySessionStorage();

            // MicrosoftOAuth
            if (MicrosoftOAuth.SessionSource == null)
                MicrosoftOAuth.SessionSource = new MicrosoftOAuthSessionSource(SessionStorage);
            var oAuthStrategy = MicrosoftOAuth.Build();

            // XboxAuth
            if (XboxAuthStrategy == null)
            {
                XboxAuth.WithMicrosoftOAuthStrategy(oAuthStrategy);
                if (XboxAuth.SessionSource == null)
                    XboxAuth.SessionSource = new InMemorySessionSource<XboxAuthTokens>();
                XboxAuthStrategy = XboxAuth.Build();
            }

            // GameAuthenticator
            if (strategy == null)
                throw new InvalidOperationException();
            var gameAuthenticator = strategy.Invoke();

            // Executor
            return new XboxGameAuthenticationExecutor<JESession>(XboxAuthStrategy, gameAuthenticator);
        }

        public new async Task<JESession> ExecuteAsync()
        {
            var result = await base.ExecuteAsync();
            return (JESession)result;
        }

        public async Task<MSession> ExecuteForLauncherAsync()
        {
            var result = await ExecuteAsync();
            return result.ToLauncherSession();
        }
    }
}