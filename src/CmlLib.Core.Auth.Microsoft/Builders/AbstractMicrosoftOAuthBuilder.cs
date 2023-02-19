using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public abstract class AbstractMicrosoftOAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        public XboxGameAuthenticationParameters Parameters { get; private set; }
        public MicrosoftOAuthCodeApiClient OAuthClient { get; private set; }

        private ISessionSource<MicrosoftOAuthResponse>? _microsoftOAuthTokenSource;
        public ISessionSource<MicrosoftOAuthResponse> MicrosoftOAuthTokenSource
        {
            get => _microsoftOAuthTokenSource ??= createOAuthTokenSource(Parameters);
            set => _microsoftOAuthTokenSource = value;
        }

        public AbstractMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthClientInfo clientInfo)
        {
            if (string.IsNullOrEmpty(clientInfo.ClientId))
                throw new ArgumentException("Cannot initialize Microsoft OAuth client using current client information. Please specify client id.");
            
            this.OAuthClient = new MicrosoftOAuthCodeApiClient(
                clientInfo.ClientId, 
                clientInfo.Scopes ?? "", 
                parameters.HttpClient);

            this.Parameters = parameters;
        }

        public AbstractMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthCodeApiClient client)
        {
            this.OAuthClient = client;
            this.Parameters = parameters;
        }

        private ISessionSource<MicrosoftOAuthResponse> createOAuthTokenSource(XboxGameAuthenticationParameters parameters)
        {
            if (parameters.SessionStorage == null)
                return new InMemorySessionSource<MicrosoftOAuthResponse>();
            else
                return new MicrosoftOAuthSessionSource(parameters.SessionStorage);
        }

        public AbstractMicrosoftOAuthBuilder WithOAuthTokenSource(ISessionSource<MicrosoftOAuthResponse> source)
        {
            MicrosoftOAuthTokenSource = source;
            return this;
        }

        public XboxAuthBuilder FromOAuthResponse(MicrosoftOAuthResponse response)
        {
            var strategy = new MockMicrosoftOAuthStrategy(response);
            return WithOAuthStrategy(strategy);
        }

        public XboxAuthBuilder WithOAuthStrategy(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            return new XboxAuthBuilder(oAuthStrategy, Parameters);
        }

        public XboxAuthBuilder WithOAuthCachingStrategy(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            var strategy = new CachingMicrosoftOAuthStrategy(oAuthStrategy, MicrosoftOAuthTokenSource);
            return WithOAuthStrategy(strategy);
        }

        public abstract Task<XboxGameSession> ExecuteAsync();
    }
}