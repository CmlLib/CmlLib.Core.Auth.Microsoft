using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.SignoutStrategy;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class XboxGameSignoutBuilder<T> where T : XboxGameSignoutBuilder<T>
    {
        private List<ISignoutStrategy> _strategies = new List<ISignoutStrategy>();
        public HttpClient? HttpClient { get; set; }
        public ISessionStorage? SessionStorage { get; set; }

        public T WithHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
            return getThis();
        }

        public T WithSessionStorage(ISessionStorage sessionStorage)
        {
            SessionStorage = sessionStorage;
            return getThis();
        }

        public T AddSignoutStrategy(ISignoutStrategy strategy)
        {
            _strategies.Add(strategy);
            return getThis();
        }

        protected ISessionStorage GetOrCreateSessionStorage()
        {
            return SessionStorage ?? throw new InvalidOperationException("Set SessionStorage first");
        }

        protected HttpClient GetOrCreateHttpClient()
        {
            return HttpClient ?? HttpHelper.DefaultHttpClient.Value;
        }

        public T ClearSignoutStrategy()
        {
            _strategies.Clear();
            return getThis();
        }

        public async Task ExecuteAsync()
        {
            foreach (var strategy in _strategies)
            {
                await strategy.Signout();
            }
        }

        private T getThis()
        {
            return (T)this;
        }
    }
}