using System;
using CmlLib.Core.Auth.Microsoft.Builders;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public static class Extensions
    {
        public static T WithMsalOAuth<T>(
            this XboxGameAuthenticationBuilder<T> self,
            Action<MsalXboxBuilder> builderInvoker)
            where T : XboxGameAuthenticationBuilder<T>
        {
            return self.WithXboxAuth(self => 
            {
                var builder = new MsalXboxBuilder();
                builder.WithXboxGameAuthenticationBuilder(self);
                builder.XboxAuth.UseBasicStrategy();
                builderInvoker.Invoke(builder);
                return builder.Build();
            });
        }
    }
}