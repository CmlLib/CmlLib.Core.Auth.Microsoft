using System;
using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JESilentAuthenticationBuilder : AbstractJEAuthenticationBuilder<JESilentAuthenticationBuilder>
    {
        public JESilentAuthenticationBuilder()
        {
            WithMicrosoftOAuth(builder => {}); // use default settings
        }

        public JESilentAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker)
        {
            this.WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builder => 
            {
                builder.MicrosoftOAuth.UseSilentStrategy();
                builder.XboxAuth.UseBasicStrategy();
                builderInvoker.Invoke(builder);
            });
            return this;
        }

        protected override IXboxGameAuthenticator<JESession> CreateGameAuthenticator()
        {
            var authenticator = CreateDefaultGameAuthenticator();
            return new SilentXboxGameAuthenticator<JESession>(authenticator, GetOrCreateSessionSource());
        }
    }
}