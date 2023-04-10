using System;
using System.Net.Http;
using System.Threading.Tasks;
using XboxAuthNet.Game.XboxAuthStrategies;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.XboxGame;
using XboxAuthNet.Game.Executors;

namespace XboxAuthNet.Game.Builders
{
    public abstract class XboxGameAuthenticationBuilder<T>
        where T : XboxGameAuthenticationBuilder<T>
    {
        protected Func<T, IXboxAuthStrategy>? XboxAuthStrategyFactory;

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

        public T WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this.XboxAuthStrategyFactory = (_ => xboxAuthStrategy);
            return GetThis();
        }

        public T WithXboxAuth(Func<T, IXboxAuthStrategy> factory)
        {
           this.XboxAuthStrategyFactory = factory;
           return GetThis();
        }

        public T WithSessionStorage(ISessionStorage sessionStorage)
        {
            this.SessionStorage = sessionStorage;
            return GetThis();
        }

        public T WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return GetThis();
        }

        protected virtual T GetThis()
        {
            return (T)this;
        }

        protected abstract IXboxGameAuthenticator BuildAuthenticator();

        public virtual IAuthenticationExecutor Build()
        {
            // XboxAuthStrategy
            if (XboxAuthStrategyFactory == null)
                throw new InvalidOperationException("Set XboxAuthStrategy first");
            var xboxAuthStrategy = XboxAuthStrategyFactory.Invoke((T)this);

            // XboxGameAuthenticator
            var authenticator = BuildAuthenticator();

            // Execute
            return new XboxGameAuthenticationExecutor(xboxAuthStrategy, authenticator);
        }

        public Task<ISession> ExecuteAsync()
        {
            var executor = Build();
            return executor.ExecuteAsync();
        }
    }
}