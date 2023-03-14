using CmlLib.Core.Auth.Microsoft.XboxGame;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JEInteractiveAuthenticationBuilder : AbstractJEAuthenticationBuilder<JEInteractiveAuthenticationBuilder>
    {
        public MicrosoftXboxBuilder<JEInteractiveAuthenticationBuilder> WithMicrosoftOAuth()
        {
            return this.WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo)
                .MicrosoftOAuth.UseInteractiveStrategy()
                .XboxAuth.UseBasicStrategy();
        }

        protected override IXboxGameAuthenticator<JESession> CreateGameAuthenticator()
        {
            return CreateDefaultGameAuthenticator();
        }
    }
}