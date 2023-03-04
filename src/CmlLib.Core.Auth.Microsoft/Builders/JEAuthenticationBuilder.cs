using System;
using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JEAuthenticationBuilder : XboxGameAuthenticationBuilder<JEAuthenticationBuilder>
    {
        public bool UseCaching { get; set; } = true;
        public ISessionSource<XboxGameSession>? SessionSource { get; set; }
        private Func<JEAuthenticationBuilder, IXboxGameAuthenticator>? strategy;

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

        private HttpClient getHttpClient()
        {
            return HttpClient ?? HttpHelper.DefaultHttpClient.Value;
        }

        public JEAuthenticationBuilder WithCaching(bool useCaching)
        {
            this.UseCaching = useCaching;
            return GetThis();
        }

        public JEAuthenticationBuilder WithSessionSource(ISessionSource<XboxGameSession> sessionSource)
        {
            this.SessionSource = sessionSource;
            return GetThis();
        }

        public JEAuthenticationBuilder UseInteractiveStrategy()
        {
            strategy = createInteractiveJEAuthenticator;
            return GetThis();
        }

        private IXboxGameAuthenticator createInteractiveJEAuthenticator(JEAuthenticationBuilder builder)
        {
            var authenticator = new DummyGameAuthenticator();

            if (builder.UseCaching)
                return new CachingGameSession(authenticator, builder.SessionSource!);
            else
                return authenticator;
        }

        public JEAuthenticationBuilder UseSilentStrategy()
        {
            strategy = createSilentJEAuthenticator;
            return GetThis();
        }

        private IXboxGameAuthenticator createSilentJEAuthenticator(JEAuthenticationBuilder builder)
        {
            var authenticator = createInteractiveJEAuthenticator(builder);
            return new SilentXboxGameAuthenticator(authenticator, builder.SessionSource!);
        }

        internal override void PreExecute()
        {
            // SessionStorage
            if (SessionStorage == null)
                SessionStorage = new InMemorySessionStorage();

            // MicrosoftOAuth
            if (MicrosoftOAuth.SessionSource == null)
                MicrosoftOAuth.SessionSource = new MicrosoftOAuthSessionSource(SessionStorage);
            var oAuthStrategy = MicrosoftOAuth.Build();

            // XboxAuth
            if (XboxAuth.SessionSource == null)
                XboxAuth.SessionSource = new InMemorySessionSource<XboxAuthTokens>();
            XboxAuth.WithMicrosoftOAuthStrategy(oAuthStrategy);
            var xboxAuthStrategy = XboxAuth.Build();
            WithXboxAuth(xboxAuthStrategy);

            // GameAuthenticator
            if (strategy == null)
                throw new InvalidOperationException();
            WithGameAuthenticator(strategy.Invoke(this));
        }
    }
}

            // problem
            // Default SessionSource should be set if client does not set any SessionSource

            // #1
            // passing SessionStorage to MicrosoftOAuthBuilder and let it to decide which SessionSource should be used
            // eg) WithSessionStorage(SessionStorage sessionStorage)
            // make it active object, instead passively set
            //     - why is this benefitial? -> idk for now

            // #2
            // add TrySetSessionSource method to MicrosoftOAuthBuilder to use default SessionSource when none of SessionSource was set
            // eg) TrySetSessionSource(SessionSource<T> sessionSource)

            // assumption
            // What if new default setting is required? Add new property, HttpClient
            // #1: passing HttpClient to MicrosoftOAuthBuilder and let it to decide HttpClient
            // #2: add TrySetHttpClient method to MicrosoftOAuthBuilder to use default HttpClient when none of HttpClient was set
            // Those methods force to modify MicrosoftOAuthBuilder and AuthenticationBuilder

            // assumption
            // What if another default SessionSource decision strategy is required? eg) `DefaultSessionSourceGenerator` decide default SessionSource
            // #1: MicrosoftOAuthBuilder should be modified: add new method like WithSessionStorage(DefaultSessionSourceGenerator sg)
            // #2: AuthenticationBuilder should be modified: add new code like MicrosoftOAuthBuilder.TrySetSessionSource(ssGenerator.Generate())

            // conclusion
            // #2 is better way, Encapsulation of MicrosoftOAuthBuilder is worth more
            // #2 means the client should provide default SessionSource decision strategy

            // more
            // find more elegant way then #1, #2
            // remove default SessionSource feature and simply internal API