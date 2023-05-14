using System;
using CmlLib.Core.Auth.Microsoft.JE;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.SignoutStrategy;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JESignoutBuilder : XboxGameSignoutBuilder<JESignoutBuilder>
    {
        public JESignoutBuilder()
        {
            this.AddMicrosoftOAuthSessionClearing();
            this.AddXboxSessionClearing();
            this.AddGameSessionClearing();
        }

        public JESignoutBuilder AddMicrosoftOAuthSignout() =>
            this.AddMicrosoftOAuthSignout(JELoginHandler.DefaultMicrosoftOAuthClientInfo);

        public JESignoutBuilder AddMicrosoftOAuthSignout(Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker) =>
            this.AddMicrosoftOAuthSignout(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

        public JESignoutBuilder AddGameSessionClearing()
        {
            AddSignoutStrategy(
                new SessionClearingStrategy<JESession>(
                    new JESessionSource(SessionStorage)));
            return this;
        }
    }
}