using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class MicrosoftOAuthBuilder : AbstractMicrosoftOAuthBuilder
    {
        public MicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters,
            MicrosoftOAuthClientInfo clientInfo)
             : base(parameters, clientInfo)
        {

        }

        // for method chaining the return type of `WithOAuthTokenSource` should be itself.
        public new MicrosoftOAuthBuilder WithOAuthTokenSource(ISessionSource<MicrosoftOAuthResponse> source)
        {
            WithOAuthTokenSource(source);
            return this;
        }

        public XboxAuthBuilder Interactive()
        {
            var codeFlow = new MicrosoftOAuthCodeFlowBuilder(OAuthClient).Build();
            return Interactive(codeFlow, new MicrosoftOAuthParameters());
        }

        public XboxAuthBuilder Interactive(MicrosoftOAuthParameters parameters)
        {
            var codeFlow = new MicrosoftOAuthCodeFlowBuilder(OAuthClient).Build();
            return Interactive(codeFlow, parameters);
        }

        public XboxAuthBuilder Interactive(Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlow> builder)
        {
            var builderObj = new MicrosoftOAuthCodeFlowBuilder(OAuthClient);
            var codeFlow = builder.Invoke(builderObj);
            return Interactive(codeFlow, new MicrosoftOAuthParameters());
        }

        public XboxAuthBuilder Interactive(
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlow> builder, 
            MicrosoftOAuthParameters parameters)
        {
            var builderObj = new MicrosoftOAuthCodeFlowBuilder(OAuthClient);
            var codeFlow = builder.Invoke(builderObj);
            return Interactive(codeFlow, parameters);
        }

        public XboxAuthBuilder Interactive(
            MicrosoftOAuthCodeFlow codeFlow, 
            MicrosoftOAuthParameters parameters)
        {
            var oauth = new InteractiveMicrosoftOAuthStrategy(codeFlow, parameters);
            return WithOAuthCachingStrategy(oauth);
        }

        public override Task<XboxGameSession> ExecuteAsync()
        {
            return Interactive().ExecuteAsync();
        }
    }
}