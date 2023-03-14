using CmlLib.Core.Auth.Microsoft.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JESilentAuthenticationBuilder : AbstractJEAuthenticationBuilder<JESilentAuthenticationBuilder>
    {
        public MicrosoftXboxBuilder<JESilentAuthenticationBuilder> WithMicrosoftOAuth()
        {
            return this.WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo)
                .MicrosoftOAuth.UseSilentStrategy()
                .XboxAuth.UseBasicStrategy();
        }

        protected override IXboxGameAuthenticator<JESession> CreateGameAuthenticator()
        {
            var authenticator = CreateDefaultGameAuthenticator();
            return new SilentXboxGameAuthenticator<JESession>(authenticator, GetOrCreateSessionSource());
        }
    }
}