using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class SilentMicrosoftOAuthBuilder : AbstractMicrosoftOAuthBuilder
    {
        public SilentMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters, 
            MicrosoftOAuthClientInfo clientInfo)
             : base(parameters, clientInfo)
        {

        }

        // for method chaining the return type of `WithOAuthTokenSource` should be itself.
        public new SilentMicrosoftOAuthBuilder WithOAuthTokenSource(ISessionSource<MicrosoftOAuthResponse> source)
        {
            WithOAuthTokenSource(source);
            return this;
        }

        public MicrosoftXboxAuthBuilder Silent()
        {
            var oauth = new SilentMicrosoftOAuthStrategy(OAuthClient, MicrosoftOAuthTokenSource);
            return WithOAuthStrategy(oauth);
        }

        public override Task<XboxGameSession> ExecuteAsync()
        {
            return Silent().ExecuteAsync();
        }
    }
}