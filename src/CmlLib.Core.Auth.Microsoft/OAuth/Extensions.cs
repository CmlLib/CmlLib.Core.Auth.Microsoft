using CmlLib.Core.Auth.Microsoft.Cache;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public static class Extensions
    {
        public static TBuilder WithMicrosoftOAuthApi<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            Func<MicrosoftOAuthApiBuilder, MicrosoftOAuthApiBuilder> msOAuthConfig)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            builder.With((_, context) =>
            {
                if (string.IsNullOrEmpty(context.ClientId))
                    throw new InvalidOperationException("Set ClientId first");

                var msBuilder = MicrosoftOAuthApiBuilder.Create(context.ClientId!);

                if (context.HttpClient != null)
                    msBuilder.WithHttpClient(context.HttpClient);

                msBuilder = msOAuthConfig.Invoke(msBuilder);
                builder.WithMicrosoftOAuthApi(msBuilder.Build());
            });

            return (TBuilder)builder;
        }
    }
}
