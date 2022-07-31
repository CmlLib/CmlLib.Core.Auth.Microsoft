using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandlerBuilder
    {
        internal static readonly Lazy<HttpClient> DefaultHttpClient
            = new Lazy<HttpClient>(() => new HttpClient());

        public static LoginHandlerBuilder Create()
        {
            return new LoginHandlerBuilder();
        }

        private LoginHandlerBuilder() { }

        public HttpClient HttpClient { get; private set; } = DefaultHttpClient.Value;
        public string? ClientId { get; private set; }

        public LoginHandlerBuilder SetHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public LoginHandlerBuilder SetClientId(string clientId)
        {
            this.ClientId = clientId;
            return this;
        }
    }
}
