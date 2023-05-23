using System;
using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.GameAuthenticators;
using XboxAuthNet.Game.Executors;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game.Builders
{
    public class XboxGameAuthenticationBuilder<T>
        where T : ISession
    {
        protected Func<XboxGameAuthenticationBuilder<T>, IXboxAuthStrategy>? XboxAuthStrategyFactory;
        protected Func<XboxGameAuthenticationBuilder<T>, IXboxGameAuthenticator<T>>? GameAuthenticatorFactory;

        public IXboxGameAccountManager? AccountManager { get; set; }

        private ISessionStorage? _sessionStorage;
        public ISessionStorage SessionStorage
        {
            get => _sessionStorage ??= new InMemorySessionStorage();
            set => _sessionStorage = value;
        }

        private HttpClient? _httpClient;
        public HttpClient HttpClient
        {
            get => _httpClient ??= HttpHelper.DefaultHttpClient.Value;
            set => _httpClient = value;
        }

        public XboxGameAuthenticationBuilder<T> WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this.XboxAuthStrategyFactory = (_ => xboxAuthStrategy);
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithXboxAuth(Func<XboxGameAuthenticationBuilder<T>, IXboxAuthStrategy> factory)
        {
           this.XboxAuthStrategyFactory = factory;
           return this;
        }

        public XboxGameAuthenticationBuilder<T> WithGameAuthenticator(IXboxGameAuthenticator<T> authenticator)
        {
            this.GameAuthenticatorFactory = (_ => authenticator);
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithGameAuthenticator(Func<XboxGameAuthenticationBuilder<T>, IXboxGameAuthenticator<T>> factory)
        {
            this.GameAuthenticatorFactory = factory;
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithAccountManager(IXboxGameAccountManager accountManager)
        {
            this.AccountManager = accountManager;
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithAccount(IXboxGameAccount account)
        {
            WithSessionStorage(account.SessionStorage);
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithNewAccount(IXboxGameAccountManager accountManager)
        {
            this.AccountManager = accountManager;
            WithAccount(accountManager.NewAccount());
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithDefaultAccount(IXboxGameAccountManager accountManager)
        {
            this.AccountManager = accountManager;
            WithAccount(accountManager.GetDefaultAccount());
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithSessionStorage(ISessionStorage sessionStorage)
        {
            this.SessionStorage = sessionStorage;
            return this;
        }

        public XboxGameAuthenticationBuilder<T> WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public virtual IAuthenticationExecutor<T> Build()
        {
            // XboxAuthStrategy
            if (XboxAuthStrategyFactory == null)
                throw new InvalidOperationException("Set XboxAuthStrategy first");
            var xboxAuthStrategy = XboxAuthStrategyFactory.Invoke(this);

            // XboxGameAuthenticator
            if (GameAuthenticatorFactory == null)
                throw new InvalidOperationException("Set GameAuthenticator first");
            var gameAuthenticator = GameAuthenticatorFactory.Invoke(this);

            // Execute
            return new XboxGameAuthenticationExecutor<T>(
                xboxAuthStrategy, 
                gameAuthenticator, 
                AccountManager);
        }

        public Task<T> ExecuteAsync()
        {
            var executor = Build();
            return executor.ExecuteAsync();
        }
    }
}