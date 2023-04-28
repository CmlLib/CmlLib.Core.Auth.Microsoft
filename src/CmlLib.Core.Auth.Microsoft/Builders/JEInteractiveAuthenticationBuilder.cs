using System;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
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

        public JEInteractiveAuthenticationBuilder WithMicrosoftOAuth(Action<MicrosoftXboxBuilder> builderInvoker) =>
            this.WithInteractiveMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

        public JEInteractiveAuthenticationBuilder WithAccountManager(XboxGameAccountManager<JEGameAccount> accountManager) =>
            this.WithNewAccount(accountManager);

        protected override IXboxGameAuthenticator BuildAuthenticator() =>
            CreateDefaultGameAuthenticator();
    }
}