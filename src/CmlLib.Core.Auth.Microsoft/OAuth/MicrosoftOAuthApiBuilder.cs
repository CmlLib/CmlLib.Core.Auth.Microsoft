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

        private string ClientId;
        private string Scope;
        private HttpClient? HttpClient;
        private IWebUI? WebUI;
        private MicrosoftOAuthParameters? Parameters;

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

        public MicrosoftOAuthApiBuilder WithWebUI(IWebUI webUi)
        {
            this.WebUI = webUi;
            return this;
        }

        public MicrosoftOAuthApiBuilder WithOAuthParameters(MicrosoftOAuthParameters parm)
        {
            this.Parameters = parm;
            return this;
        }

        public MicrosoftOAuthApi Build()
        {
            if (HttpClient == null)
                HttpClient = HttpHelper.DefaultHttpClient.Value;

            var oauth = new MicrosoftOAuth(ClientId, Scope, HttpClient);

            if (WebUI == null)
                return new MicrosoftOAuthApi(oauth);
            else
                return new MicrosoftOAuthApiWithWebUI(WebUI, oauth, Parameters ?? new MicrosoftOAuthParameters());
        }
    }
}
