using CmlLib.Core.Auth.Microsoft.Cache;
using Microsoft.Identity.Client;
using System.IO;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public static class Extensions
    {
        public static TBuilder WithMsalCacheManager<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            IPublicClientApplication app)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_msalsession.json");
            return builder.WithMsalCacheManager(app, defaultPath);
        }

        public static TBuilder WithMsalCacheManager<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            IPublicClientApplication app, string path)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            return builder.WithCacheManager(new MsalSessionCacheManager<TSession>(app, path));
        }
    }
}
