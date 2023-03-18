using System;

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
            return self.WithXboxAuth(r => 
            {   
                var builder = new MicrosoftXboxBuilder(clientInfo);
                builderInvoker.Invoke(builder);
                return builder.Build();
            });
        }
    }
}