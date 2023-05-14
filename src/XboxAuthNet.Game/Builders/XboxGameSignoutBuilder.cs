using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.SignoutStrategy;
using XboxAuthNet.Game.XboxAuthStrategies;

namespace XboxAuthNet.Game.Builders
{
    public class XboxGameSignoutBuilder<T>
        where T : XboxGameSignoutBuilder<T>
    {
        private List<ISignoutStrategy> _strategies = new List<ISignoutStrategy>();

        private HttpClient? _httpClient;
        public HttpClient HttpClient
        {
            get => _httpClient ??= HttpHelper.DefaultHttpClient.Value;
            set => _httpClient = value;
        }

        private ISessionStorage? _sessionStorage;
        public ISessionStorage SessionStorage
        {
            get => _sessionStorage ??= new InMemorySessionStorage();
            set => _sessionStorage = value;
        }

        public T WithHttpClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
            return getThis();
        }

        public T WithAccount(IXboxGameAccount account)
        {
            WithSessionStorage(account.SessionStorage);
            return getThis();
        }

        public T AddSavingAccountManager(IXboxGameAccountManager accountManager)
        {
            AddSignoutStrategy(
                new SavingAccountManagerStrategy(accountManager));
            return getThis();
        }

        public T WithSessionStorage(ISessionStorage sessionStorage)
        {
            SessionStorage = sessionStorage;
            return getThis();
        }

        public T AddXboxSessionClearing()
        {
            AddSignoutStrategy(
                new SessionClearingStrategy<XboxAuthTokens>(
                    new XboxSessionSource(GetOrCreateSessionStorage())));
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