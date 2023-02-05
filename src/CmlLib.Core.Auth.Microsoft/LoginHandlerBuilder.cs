using System;
using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.Cache;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandlerBuilder
    {
        private static Lazy<HttpClient> lazyHttpClient = new Lazy<HttpClient>(() => new HttpClient());
        public static HttpClient DefaultHttpClient => lazyHttpClient.Value;

        public static LoginHandlerBuilder Create()
        {
            return new LoginHandlerBuilder();
        }

        private LoginHandlerBuilder() {}

        public HttpClient? HttpClient { get; set; }
        public ICacheStorage<XboxGameSession>? CacheStorage { get; set; }

        public LoginHandlerBuilder WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public LoginHandlerBuilder WithCacheStorage(ICacheStorage<XboxGameSession> cacheStorage)
        {
            this.CacheStorage = cacheStorage;
            return this;
        }

        public JELoginHandler ForJavaEdition()
        {
            return new JELoginHandler(HttpClient, CacheStorage);
        }
    }
}