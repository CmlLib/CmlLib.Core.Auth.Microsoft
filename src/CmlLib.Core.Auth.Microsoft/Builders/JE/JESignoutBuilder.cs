using System;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using CmlLib.Core.Auth.Microsoft.SignoutStrategy;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public class JESignoutBuilder : XboxGameSignoutBuilder<JESignoutBuilder>
    {
        public JESignoutBuilder()
        {
            AddMicrosoftOAuthSessionClearing();
            AddXboxSessionClearing();
            AddGameSessionClearing();
        }

        public JESignoutBuilder AddMicrosoftOAuthSignout() =>
            AddMicrosoftOAuthSignout(JELoginHandler.DefaultMicrosoftOAuthClientInfo);

        public JESignoutBuilder AddMicrosoftOAuthSignout(Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker) =>
            AddMicrosoftOAuthSignout(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

        public JESignoutBuilder AddMicrosoftOAuthSignout(MicrosoftOAuthClientInfo clientInfo) =>
            AddMicrosoftOAuthSignout(clientInfo, builder => builder);

        public JESignoutBuilder AddMicrosoftOAuthSignout(
            MicrosoftOAuthClientInfo clientInfo, 
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker)
        {
            var apiClient = clientInfo.CreateApiClientForOAuthCode(GetOrCreateHttpClient());
            var builder = new MicrosoftOAuthCodeFlowBuilder(apiClient);
            builderInvoker(builder);
            var codeFlow = builder.Build();

            AddSignoutStrategy(new MicrosoftOAuthBrowserSignoutStrategy(codeFlow));
            return this;
        }

        public JESignoutBuilder AddMicrosoftOAuthSessionClearing()
        {
            AddSignoutStrategy(
                new SessionClearingStrategy<MicrosoftOAuthResponse>(
                    new MicrosoftOAuthSessionSource(GetOrCreateSessionStorage())));
            return this;
        }

        public JESignoutBuilder AddXboxSessionClearing()
        {
            AddSignoutStrategy(
                new SessionClearingStrategy<XboxAuthTokens>(
                    new XboxSessionSource(GetOrCreateSessionStorage())));
            return this;
        }

        public JESignoutBuilder AddGameSessionClearing()
        {
            AddSignoutStrategy(
                new SessionClearingStrategy<JESession>(
                    new SessionFromStorage<JESession>("G", GetOrCreateSessionStorage())));
            return this;
        }
    }
}