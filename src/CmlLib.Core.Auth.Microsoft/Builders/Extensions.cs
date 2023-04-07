using System;
using CmlLib.Core.Auth.Microsoft.SignoutStrategy;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public static class Extensions
    {
        public static T WithMicrosoftOAuth<T>(
            this XboxGameAuthenticationBuilder<T> self,
            MicrosoftOAuthClientInfo clientInfo,
            Action<MicrosoftXboxBuilder> builderInvoker)
            where T : XboxGameAuthenticationBuilder<T>
        {
            return self.WithXboxAuth(self => 
            {   
                var builder = new MicrosoftXboxBuilder(clientInfo);
                builder.WithXboxGameAuthenticationBuilder(self);

                builderInvoker.Invoke(builder);
                return builder.Build();
            });
        }

        public static T AddMicrosoftOAuthSessionClearing<T>(
            this XboxGameSignoutBuilder<T> self)
            where T : XboxGameSignoutBuilder<T>
        {
            return self.AddSignoutStrategy(
                new SessionClearingStrategy<MicrosoftOAuthResponse>(
                    new MicrosoftOAuthSessionSource(self.SessionStorage)));
        }

        public static T AddMicrosoftOAuthSignout<T>(
            this XboxGameSignoutBuilder<T> self,
            MicrosoftOAuthClientInfo clientInfo)
            where T : XboxGameSignoutBuilder<T>
        {
            return self.AddMicrosoftOAuthSignout(clientInfo, builder => builder);
        }

        public static T AddMicrosoftOAuthSignout<T>(
            this XboxGameSignoutBuilder<T> self,
            MicrosoftOAuthClientInfo clientInfo, 
            Func<MicrosoftOAuthCodeFlowBuilder, MicrosoftOAuthCodeFlowBuilder> builderInvoker)
            where T : XboxGameSignoutBuilder<T>
        {
            var apiClient = clientInfo.CreateApiClientForOAuthCode(self.HttpClient);
            var builder = new MicrosoftOAuthCodeFlowBuilder(apiClient);
            builderInvoker(builder);
            var codeFlow = builder.Build();

            return self.AddSignoutStrategy(new MicrosoftOAuthBrowserSignoutStrategy(codeFlow));
        }
    }
}