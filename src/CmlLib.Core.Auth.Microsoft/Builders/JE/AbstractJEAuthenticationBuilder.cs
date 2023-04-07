using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.JE;
using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.Executors;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public abstract class AbstractJEAuthenticationBuilder<T> : XboxGameAuthenticationBuilder<T>
        where T : AbstractJEAuthenticationBuilder<T>
    {
        public bool UseCaching { get; set; } = true;
        public bool CheckGameOwnership { get; set; } = false;
        public ISessionSource<JESession>? SessionSource { get; set; }
        private Func<IXboxGameAuthenticator<JESession>>? strategy;

        public T WithCaching() => WithCaching(true);
        public T WithCaching(bool useCaching)
        {
            this.UseCaching = useCaching;
            return GetThis();
        }

        public T WithCheckingGameOwnership() => WithCheckingGameOwnership(true);
        public T WithCheckingGameOwnership(bool checkGameOwnership)
        {
            this.CheckGameOwnership = checkGameOwnership;
            return GetThis();
        }

        public T WithSessionSource(ISessionSource<JESession> sessionSource)
        {
            this.SessionSource = sessionSource;
            return GetThis();
        }

        protected ISessionSource<JESession> GetOrCreateSessionSource()
        {
            return SessionSource ??= new JESessionSource(SessionStorage);
        }

        public override IAuthenticationExecutor Build()
        {
            if (XboxAuthStrategyFactory == null)
                throw new InvalidOperationException("Set XboxAuthStrategy first");
            var xboxAuthStrategy = XboxAuthStrategyFactory.Invoke((T)this);

            // GameAuthenticator
            var gameAuthenticator = CreateGameAuthenticator();

            // Executor
            return new XboxGameAuthenticationExecutor<JESession>(xboxAuthStrategy, gameAuthenticator);
        }

        protected IXboxGameAuthenticator<JESession> CreateDefaultGameAuthenticator()
        {
            var authenticator = new JEAuthenticator(HttpClient, GetOrCreateSessionSource());
            if (UseCaching)
                return new CachingGameSession<JESession>(authenticator, SessionSource!);
            else
                return authenticator;
        }

        protected abstract IXboxGameAuthenticator<JESession> CreateGameAuthenticator();

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