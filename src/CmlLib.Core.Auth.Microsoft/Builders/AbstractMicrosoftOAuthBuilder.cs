using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public abstract class AbstractMicrosoftOAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        protected XboxGameAuthenticationParameters Parameters { get; private set; }
        protected MicrosoftOAuthCodeApiClient OAuthClient { get; private set; }
        protected ISessionSource<MicrosoftOAuthResponse> MicrosoftOAuthTokenSource { get; private set; }

        public AbstractMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthClientInfo clientInfo)
        {
            this.Parameters = parameters;
            this.OAuthClient = new MicrosoftOAuthCodeApiClient(clientInfo.ClientId, clientInfo.Scopes, Parameters.HttpClient);

            if (parameters.SessionStorage == null)
                this.MicrosoftOAuthTokenSource = new InMemorySessionSource<MicrosoftOAuthResponse>();
            else
                this.MicrosoftOAuthTokenSource = new MicrosoftOAuthSessionSource(parameters.SessionStorage);
        }

        public AbstractMicrosoftOAuthBuilder WithOAuthTokenSource(ISessionSource<MicrosoftOAuthResponse> source)
        {
            MicrosoftOAuthTokenSource = source;
            return this;
        }

        public MicrosoftXboxAuthBuilder FromOAuthResponse(MicrosoftOAuthResponse response)
        {
            var strategy = new MockMicrosoftOAuthStrategy(response);
            return WithOAuthStrategy(strategy);
        }

        public MicrosoftXboxAuthBuilder WithOAuthStrategy(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            return new MicrosoftXboxAuthBuilder(oAuthStrategy, Parameters);
        }

        public abstract Task<XboxGameSession> ExecuteAsync();
    }
}