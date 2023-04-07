using System;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.SignoutStrategy;
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
                    new SessionFromStorage<JESession>("G", GetOrCreateSessionStorage())));
            return this;
        }
    }
}