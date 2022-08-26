using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.OAuth;
using Microsoft.Identity.Client;
using System;
using System.IO;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public static class Extensions
    {
        public static TBuilder WithMsalOAuth<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            IPublicClientApplication app, Func<MsalOAuthApiFactory, IMicrosoftOAuthApi> factory)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            var oauthApi = factory.Invoke(new MsalOAuthApiFactory(app));
            return builder
                .WithMicrosoftOAuthApi(oauthApi)
                .WithMsalCacheManager();
        }

        public static TBuilder WithMsalCacheManager<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_msalsession.json");
            return builder.WithMsalCacheManager(defaultPath);
        }

        public static TBuilder WithMsalCacheManager<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            string path)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            return builder.WithCacheManager(new MsalSessionCacheManager<TSession>(path));
        }
    }
}
