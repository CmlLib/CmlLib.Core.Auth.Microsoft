using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.Builders.XboxAuth;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders.OAuth
{
    public abstract class AbstractMicrosoftOAuthBuilder : IXboxGameAuthenticationExecutorBuilder
    {
        protected XboxGameAuthenticationParameters Parameters { get; private set; }
        protected MicrosoftOAuthCodeApiClient OAuthClient { get; private set; }

        public AbstractMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthClientInfo clientInfo)
        {
            if (string.IsNullOrEmpty(clientInfo.ClientId))
                throw new ArgumentException("Cannot initialize Microsoft OAuth client using current client information. Please specify client id.");
            
            this.OAuthClient = new MicrosoftOAuthCodeApiClient(
                clientInfo.ClientId, 
                clientInfo.Scopes ?? "", 
                parameters.HttpClient ?? HttpHelper.DefaultHttpClient.Value);

            this.Parameters = parameters;
        }

        public AbstractMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthCodeApiClient client)
        {
            this.OAuthClient = client;
            this.Parameters = parameters;
        }

        public XboxAuthBuilder FromOAuthResponse(MicrosoftOAuthResponse response)
        {
            var strategy = new MockMicrosoftOAuthStrategy(response);
            return WithOAuthStrategy(strategy);
        }

        public XboxAuthBuilder WithOAuthStrategy(IMicrosoftOAuthStrategy oAuthStrategy)
        {
            Parameters.OAuthStrategy = oAuthStrategy;
            return new XboxAuthBuilder(Parameters);
        }

        public abstract Task<XboxGameSession> ExecuteAsync();
    }
}