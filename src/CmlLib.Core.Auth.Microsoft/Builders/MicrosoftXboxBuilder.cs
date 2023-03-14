using System.Net.Http;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftXboxBuilder<T> where T : IBuilderWithXboxAuthStrategy
    {
        private readonly T _parentBuilder;
        private HttpClient? httpClient;

        public MicrosoftXboxBuilder(
            T parentBuilder,
            MicrosoftOAuthClientInfo clientInfo)
        {
            _parentBuilder = parentBuilder;
            MicrosoftOAuth = createOAuthBuilder(clientInfo);
            XboxAuth = createXboxAuthBuilder();
        }

        public MicrosoftXboxBuilder<T> WithHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            return this;
        }

        public MicrosoftOAuthStrategyBuilder<MicrosoftXboxBuilder<T>> MicrosoftOAuth { get; private set; }
        public XboxAuthStrategyBuilder<MicrosoftXboxBuilder<T>> XboxAuth { get; private set; }

        private MicrosoftOAuthStrategyBuilder<MicrosoftXboxBuilder<T>> createOAuthBuilder(MicrosoftOAuthClientInfo clientInfo)
        {
            var builder = new MicrosoftOAuthStrategyBuilder<MicrosoftXboxBuilder<T>>(this, clientInfo, getHttpClient());
            return builder;
        }

        private XboxAuthStrategyBuilder<MicrosoftXboxBuilder<T>> createXboxAuthBuilder()
        {
            var builder = new XboxAuthStrategyBuilder<MicrosoftXboxBuilder<T>>(this, getHttpClient());
            return builder;
        }

        private HttpClient getHttpClient() => httpClient ?? _parentBuilder.HttpClient;

        public IXboxAuthStrategy BuildXboxAuthStrategy()
        {
            // MicrosoftOAuth
            // provide default SessionSource using _parentBuilder's SessionStorage
            if (MicrosoftOAuth.SessionSource == null)
                MicrosoftOAuth.WithSessionSource(new MicrosoftOAuthSessionSource(_parentBuilder.SessionStorage));
            var oAuthStrategy = MicrosoftOAuth.Build();

            // XboxAuth
            XboxAuth.WithMicrosoftOAuthStrategy(oAuthStrategy);
            if (XboxAuth.SessionSource == null)
                XboxAuth.WithSessionSource(new XboxSessionSource(_parentBuilder.SessionStorage));
            return XboxAuth.Build();
        }

        public T Build()
        {
            _parentBuilder.XboxAuthStrategy = BuildXboxAuthStrategy();
            return _parentBuilder;
        }
    }
}