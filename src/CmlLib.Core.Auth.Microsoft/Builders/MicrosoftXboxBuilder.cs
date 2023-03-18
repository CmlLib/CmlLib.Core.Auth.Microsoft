using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftXboxBuilder
    {
        public HttpClient? HttpClient { get; set; }
        public ISessionStorage? SessionStorage { get; set; }

        public MicrosoftXboxBuilder(
            MicrosoftOAuthClientInfo clientInfo)
        {
            MicrosoftOAuth = createOAuthBuilder(clientInfo);
            XboxAuth = createXboxAuthBuilder();
        }

        public MicrosoftXboxBuilder WithXboxGameAuthenticationBuilder<T>(XboxGameAuthenticationBuilder<T> builder)
            where T : XboxGameAuthenticationBuilder<T>
        {
            if (builder.HttpClient != null)
                WithHttpClient(builder.HttpClient);
            if (builder.SessionStorage != null)
                WithSessionStorage(builder.SessionStorage);

            return this;
        }

        public MicrosoftXboxBuilder WithHttpClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
            return this;
        }

        public MicrosoftXboxBuilder WithSessionStorage(ISessionStorage sessionStorage)
        {
            this.SessionStorage = sessionStorage;
            return this;
        }

        public MicrosoftOAuthStrategyBuilder<MicrosoftXboxBuilder> MicrosoftOAuth { get; private set; }
        public XboxAuthStrategyBuilder<MicrosoftXboxBuilder> XboxAuth { get; private set; }

        private MicrosoftOAuthStrategyBuilder<MicrosoftXboxBuilder> createOAuthBuilder(MicrosoftOAuthClientInfo clientInfo)
        {
            var builder = new MicrosoftOAuthStrategyBuilder<MicrosoftXboxBuilder>(this, clientInfo, getHttpClient());
            return builder;
        }

        private XboxAuthStrategyBuilder<MicrosoftXboxBuilder> createXboxAuthBuilder()
        {
            var builder = new XboxAuthStrategyBuilder<MicrosoftXboxBuilder>(this, getHttpClient());
            return builder;
        }

        private HttpClient getHttpClient() => HttpClient ?? HttpHelper.DefaultHttpClient.Value;

        public IXboxAuthStrategy Build()
        {
            // MicrosoftOAuth
            // provide default SessionSource using _parentBuilder's SessionStorage
            if (MicrosoftOAuth.SessionSource == null && this.SessionStorage != null)
                MicrosoftOAuth.WithSessionSource(new MicrosoftOAuthSessionSource(this.SessionStorage));
            var oAuthStrategy = MicrosoftOAuth.Build();

            // XboxAuth
            XboxAuth.WithMicrosoftOAuthStrategy(oAuthStrategy);
            if (XboxAuth.SessionSource == null && this.SessionStorage != null)
                XboxAuth.WithSessionSource(new XboxSessionSource(this.SessionStorage));
            return XboxAuth.Build();
        }
    }
}