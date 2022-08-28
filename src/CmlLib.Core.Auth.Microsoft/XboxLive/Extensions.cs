using CmlLib.Core.Auth.Microsoft.Cache;
using System;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public static class Extensions
    {
        public static TBuilder WithXboxSisuAuthApi<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            Func<XboxSisuAuthBuilder, XboxSisuAuthBuilder> func)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            builder.With((_, context) =>
            {
                if (context.ClientId == null)
                    throw new InvalidOperationException("Set ClientId first");

                var sisu = new XboxSisuAuthBuilder()
                    .WithHttpClient(context.HttpClient)
                    .WithClientId(context.ClientId);

                sisu = func.Invoke(sisu);

                builder.WithXboxLiveApi(sisu.Build());
            });

            return (TBuilder)builder;
        }

        public static TBuilder WithXboxAuthNetApi<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            Func<XboxAuthNetApiBuilder, XboxAuthNetApiBuilder> func)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            builder.With((_, context) =>
            {
                var authNet = new XboxAuthNetApiBuilder()
                    .WithHttpClient(context.HttpClient);

                authNet = func.Invoke(authNet);

                builder.WithXboxLiveApi(authNet.Build());
            });

            return (TBuilder)builder;
        }
    }
}
