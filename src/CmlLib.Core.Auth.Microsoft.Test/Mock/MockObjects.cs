using CmlLib.Core.Auth.Microsoft.Cache;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockObjects
    {
        public ICacheManager<JavaEditionSessionCache> CacheManager { get; init; }
        public MockOAuthApi OAuthApi { get; init; }
        public MockXboxLiveApi XboxLiveApi { get; init; }
        public MockMojangXboxApi MojangXboxApi { get; init; }

        public MockObjects(
            ICacheManager<JavaEditionSessionCache> cacheManager,
            MockOAuthApi oAuthApi,
            MockXboxLiveApi xboxLiveApi,
            MockMojangXboxApi mojangXboxApi)
        {
            CacheManager = cacheManager;
            OAuthApi = oAuthApi;
            XboxLiveApi = xboxLiveApi;
            MojangXboxApi = mojangXboxApi;
        }

    }
}