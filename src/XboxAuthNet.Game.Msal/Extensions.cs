using System;
using XboxAuthNet.Game.Builders;

namespace XboxAuthNet.Game.Msal
{
    public static class Extensions
    {
        public static XboxGameAuthenticationBuilder<T> WithMsalOAuth<T>(
            this XboxGameAuthenticationBuilder<T> self,
            Action<MsalXboxBuilder> builderInvoker)
            where T : ISession
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