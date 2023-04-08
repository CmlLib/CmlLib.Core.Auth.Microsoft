using System;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JEInteractiveAuthenticationBuilder : AbstractJEAuthenticationBuilder<JEInteractiveAuthenticationBuilder>
    {
        public JEInteractiveAuthenticationBuilder()
        {
            WithMicrosoftOAuth(builder => {}); // use default settings
        }

        public JEInteractiveAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker)
        {
            this.WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builder => 
            {
                builder.MicrosoftOAuth.UseInteractiveStrategy();
                builder.XboxAuth.UseBasicStrategy();
                builderInvoker.Invoke(builder);
            });
            return this;
        }

        protected override IXboxGameAuthenticator<JESession> CreateGameAuthenticator()
        {
            return CreateDefaultGameAuthenticator();
        }
    }
}