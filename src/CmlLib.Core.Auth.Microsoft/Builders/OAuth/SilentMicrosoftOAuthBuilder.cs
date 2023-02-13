using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.Builders.XboxAuth;

namespace CmlLib.Core.Auth.Microsoft.Builders.OAuth
{
    public sealed class SilentMicrosoftOAuthBuilder : AbstractMicrosoftOAuthBuilder
    {
        public SilentMicrosoftOAuthBuilder(
            XboxGameAuthenticationParameters parameters, 
            MicrosoftOAuthClientInfo clientInfo)
             : base(parameters, clientInfo)
        {

        }

        public XboxAuthBuilder Silent()
        {
            var oauth = new SilentMicrosoftOAuthStrategy(OAuthClient);
            return WithOAuthStrategy(oauth);
        }

        public override Task<XboxGameSession> ExecuteAsync()
        {
            return Silent().ExecuteAsync();
        }
    }
}