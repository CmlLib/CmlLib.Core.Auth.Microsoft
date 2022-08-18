using System.Net.Http;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public class MicrosoftOAuthApiBuilder
    {
        public static MicrosoftOAuthApiBuilder Create(string clientId)
        {
            return new MicrosoftOAuthApiBuilder(clientId);
        }

        private string ClientId { get; set; }
        public string Scope { get; set; }
        public HttpClient? HttpClient { get; set; }

        private MicrosoftOAuthApiBuilder(string clientId)
        {
            this.ClientId = clientId;
            this.Scope = XboxAuth.XboxScope;
        }

        public MicrosoftOAuthApiBuilder WithScope(string scope)
        {
            this.Scope = scope;
            return this;
        }

        public MicrosoftOAuthApiBuilder WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public MicrosoftOAuthApi Build()
        {
            if (HttpClient == null)
                HttpClient = HttpHelper.DefaultHttpClient.Value;

            return new MicrosoftOAuthApi(new MicrosoftOAuth(ClientId, Scope, HttpClient));
        }
    }
}
