using System;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.XboxGame;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public sealed class JESilentAuthenticationBuilder : AbstractJEAuthenticationBuilder<JESilentAuthenticationBuilder>
    {
        public JESilentAuthenticationBuilder()
        {
            WithMicrosoftOAuth(builder => {}); // use default settings
        }

        public JESilentAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker) =>
            this.WithSilentMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

        public JESilentAuthenticationBuilder WithAccountManager(IXboxGameAccountManager accountManager) =>
            this.WithDefaultAccount(accountManager);

        protected override IXboxGameAuthenticator BuildAuthenticator()
        {
            var authenticator = CreateDefaultGameAuthenticator();
            return new SilentXboxGameAuthenticator<JESession>(authenticator, GetOrCreateSessionSource());
        }
    }
}