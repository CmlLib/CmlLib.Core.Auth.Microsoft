namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public static class Extensions
    {
        public static TBuilder WithJsonCacheManager<TBuilder, TSession>(this AbstractLoginHandlerBuilder<TBuilder, TSession> builder,
            string path)
            where TBuilder : AbstractLoginHandlerBuilder<TBuilder, TSession>
            where TSession : SessionCacheBase
        {
            var cacheManager = new JsonFileCacheManager<TSession>(path);
            return builder.WithCacheManager(cacheManager);
        }
    }
}
