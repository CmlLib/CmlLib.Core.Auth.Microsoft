using System;
using XboxAuthNet.OAuth;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.SignoutStrategy;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft
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