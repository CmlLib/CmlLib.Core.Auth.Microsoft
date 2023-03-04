using System;
using System.Net.Http;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameAuthenticationBuilder<T> where T : XboxGameAuthenticationBuilder<T>
    {
        private IXboxAuthStrategy? _xboxAuthStrategy;
        private IXboxGameAuthenticator? _gameAuthenticator;

        public ISessionStorage? SessionStorage { get; set; }

        public HttpClient? HttpClient { get; set; }

        public T WithXboxAuth(IXboxAuthStrategy xboxAuthStrategy)
        {
            this._xboxAuthStrategy = xboxAuthStrategy;
            return GetThis();
        }

        public T WithGameAuthenticator(IXboxGameAuthenticator authenticator)
        {
            this._gameAuthenticator = authenticator;
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

        internal virtual void PreExecute()
        {
            // hook
        }

        public Task<XboxGameSession> ExecuteAsync()
        {
            PreExecute();

            if (_gameAuthenticator == null)
                throw new InvalidOperationException();
            if (_xboxAuthStrategy == null)
                throw new InvalidOperationException();

            return _gameAuthenticator.Authenticate(_xboxAuthStrategy);
        }
    }
}