using System;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public abstract class XboxGameAuthenticationBuilder<T>
        where T : XboxGameAuthenticationBuilder<T>
    {
        protected Func<T, IXboxAuthStrategy>? XboxAuthStrategyFactory;
        public ISessionStorage? SessionStorage { get; set; }
        public HttpClient? HttpClient { get; set; }

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

        protected HttpClient GetOrCreateHttpClient()
        {
            return HttpClient ??= HttpHelper.DefaultHttpClient.Value;
        }

        protected ISessionStorage GetOrCreateSessionStorage()
        {
            return SessionStorage ??= new InMemorySessionStorage();
        }

        protected virtual T GetThis()
        {
            return (T)this;
        }

        public abstract IAuthenticationExecutor Build();

        public Task<ISession> ExecuteAsync()
        {
            return Build().ExecuteAsync();
        }
    }
}