using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.Executors;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public abstract class XboxGameAuthenticationBuilder<T> where T : XboxGameAuthenticationBuilder<T>
    {
        private IXboxAuthStrategy? _xboxAuthStrategy;
        public ISessionStorage? SessionStorage { get; set; }
        public HttpClient? HttpClient { get; set; }

        public T WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this._xboxAuthStrategy = xboxAuthStrategy;
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

        public abstract IAuthenticationExecutor Build();

        public virtual Task<ISession> ExecuteAsync()
        {
            return Build().ExecuteAsync();
        }
    }
}