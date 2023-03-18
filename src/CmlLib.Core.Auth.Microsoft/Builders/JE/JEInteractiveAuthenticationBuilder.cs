using System;
using CmlLib.Core.Auth.Microsoft.XboxGame;

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
                builder.WithXboxGameAuthenticationBuilder(this);
                builder.MicrosoftOAuth.WithCaching(true);
                builder.XboxAuth.WithCaching(true);
                
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