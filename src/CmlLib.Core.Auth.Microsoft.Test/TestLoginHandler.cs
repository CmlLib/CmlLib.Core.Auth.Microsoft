using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.Test.Mock;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    internal class MockObjects
    {
        public ICacheManager<JavaEditionSessionCache> CacheManager { get; init; }
        public MockOAuthApi OAuthApi { get; init; }
        public MockXboxLiveApi XboxLiveApi { get; init; }
        public MockMojangXboxApi MojangXboxApi { get; init; }
    }

    internal class TestLoginHandler
    {
        public const string NotExpiredJwt = ".eyJleHAiOjE5MDk2NjEzMTh9."; // exp: 2030.07.07
        public const string ExpiredJwt = ".eyJleHAiOjk2Mjk3NjUxOH0."; // exp: 2000.07.07

        public (MockObjects, JavaEditionLoginHandler) CreateMockEnvironment()
        {
            var cacheManager = new InMemoryCacheManger<JavaEditionSessionCache>();
            var oauthApi = new MockOAuthApi();
            var xboxLiveApi = new MockXboxLiveApi();
            var mojangXboxApi = new MockMojangXboxApi();

            var loginHandler = new JavaEditionLoginHandlerBuilder()
                .WithMicrosoftOAuthApi(oauthApi)
                .WithCacheManager(cacheManager)
                .WithXboxLiveApi(xboxLiveApi)
                .WithMojangXboxApi(mojangXboxApi)
                .Build();

            var objects = new MockObjects
            {
                CacheManager = cacheManager,
                OAuthApi = oauthApi,
                XboxLiveApi = xboxLiveApi,
                MojangXboxApi = mojangXboxApi,
            };
            return (objects, loginHandler);
        }

        public JavaEditionSessionCache CreateDefaultTestCase()
        {
            var msToken = new MicrosoftOAuthResponse()
            {
                AccessToken = "OldOAuthAccessToken",
                RefreshToken = "OldOAuthRefreshToken",
                ExpireIn = 9999999 // not expired
            };
            var xsts = new XboxAuthResponse()
            {
                Token = "OldXstsToken",
                XuiClaims = new XboxAuthXuiClaims
                {
                    UserHash = "OldUserHash",
                    XboxUserId = "OldXboxUserId"
                }
            };
            var mcToken = new MojangXboxLoginResponse
            {
                AccessToken = NotExpiredJwt,
                ExpiresIn = 86400,
                ExpiresOn = DateTime.UtcNow.AddDays(1)
            };
            var msession = new MSession
            {
                AccessToken = "OldAccessToken",
                Username = "OldUsername",
                UUID = "OldUUID"
            };

            return new JavaEditionSessionCache
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mcToken,
                GameSession = msession
            };
        }

        [Test]
        public async Task TestLoginFromCache_ValidMojangSession()
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            var testcase = CreateDefaultTestCase();

            await mockObjects.CacheManager.SaveCache(testcase);

            var result = await loginHandler.LoginFromCache();

            Assert.AreEqual(testcase.MicrosoftOAuthToken, result.MicrosoftOAuthToken);
            Assert.AreEqual(testcase.XstsToken, result.XstsToken);
            Assert.AreEqual(testcase.MojangXboxToken, result.MojangXboxToken);

            Assert.AreEqual(testcase.GameSession, result.GameSession);
            Assert.AreEqual(testcase.GameSession.Username, result.GameSession.Username);
            Assert.AreEqual(testcase.GameSession.UUID, result.GameSession.UUID);

            // accessToken should be always updated to MojangXboxLoginResponse.AccessToken
            Assert.AreEqual(NotExpiredJwt, result.GameSession.AccessToken);

            // check cached session is same as result
            var cache = await mockObjects.CacheManager.ReadCache();
            Assert.AreEqual(result, cache);
        }

        public static object[] ExpiredMojangXboxLoginResponses = new object[]
        {
            new MojangXboxLoginResponse
            {
                AccessToken = NotExpiredJwt,
                ExpiresIn = 86400,
                ExpiresOn = DateTime.UtcNow.AddDays(-10)
            },
            new MojangXboxLoginResponse
            {
                AccessToken = ExpiredJwt,
                ExpiresIn = 86400,
                ExpiresOn = DateTime.UtcNow.AddDays(10)
            }
        };

        [Test]
        [TestCaseSource(nameof(ExpiredMojangXboxLoginResponses))]
        public async Task TestLoginFromCache_ValidOAuthExpiredMojang(MojangXboxLoginResponse mcToken)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            var testcase = CreateDefaultTestCase();
            testcase.MojangXboxToken = mcToken;

            await mockObjects.CacheManager.SaveCache(testcase);

            var result = await loginHandler.LoginFromCache();

            // all tokens should be refreshed
            var cache = await mockObjects.CacheManager.ReadCache();
            Assert.NotNull(cache?.MicrosoftOAuthToken);
            Assert.NotNull(cache?.XstsToken);
            Assert.NotNull(cache?.MojangXboxToken);
            Assert.NotNull(cache?.GameSession);

            Assert.AreEqual("MockOAuthApi_GetOrRefreshTokens_AccessToken", result.MicrosoftOAuthToken?.AccessToken);
            Assert.AreEqual("MockOAuthApi_GetOrRefreshTokens_RefreshToken", result.MicrosoftOAuthToken?.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", result.XstsToken?.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", result.XstsToken?.UserHash);
            Assert.AreEqual("MockXboxLiveApi_XboxUserId", result.XstsToken?.UserXUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", result.MojangXboxToken?.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", result.MojangXboxToken?.Username);
            
            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", result.GameSession?.Username); 
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", result.GameSession?.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", result.GameSession?.AccessToken);

            // check cached session is same as result
            Assert.AreEqual(result, cache);
        }

        [Test]
        [TestCaseSource(nameof(ExpiredMojangXboxLoginResponses))]
        public void TestLoginFromCache_ExpiredOAuthExpiredMojang(MojangXboxLoginResponse mcToken)
        {
            Assert.ThrowsAsync<MicrosoftOAuthException>(async () => {
                var (mockObjects, loginHandler) = CreateMockEnvironment();
                var testcase = CreateDefaultTestCase();
                testcase.MicrosoftOAuthToken.ExpireIn = 0; // make token expired
                testcase.MojangXboxToken = mcToken;

                await mockObjects.CacheManager.SaveCache(testcase);

                await loginHandler.LoginFromCache();
            });
        }

        [Test]
        public async Task TestLoginFromOAuth()
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            
            var result = await loginHandler.LoginFromOAuth();

            // all tokens should be refreshed
            var cache = await mockObjects.CacheManager.ReadCache();
            Assert.NotNull(cache?.MicrosoftOAuthToken);
            Assert.NotNull(cache?.XstsToken);
            Assert.NotNull(cache?.MojangXboxToken);
            Assert.NotNull(cache?.GameSession);

            Assert.AreEqual("MockOAuthApi_RequestNewTokens_AccessToken", result.MicrosoftOAuthToken?.AccessToken);
            Assert.AreEqual("MockOAuthApi_RequestNewTokens_RefreshToken", result.MicrosoftOAuthToken?.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", result.XstsToken?.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", result.XstsToken?.UserHash);
            Assert.AreEqual("MockXboxLiveApi_XboxUserId", result.XstsToken?.UserXUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", result.MojangXboxToken?.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", result.MojangXboxToken?.Username);

            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", result.GameSession?.Username);
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", result.GameSession?.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", result.GameSession?.AccessToken);

            // check cached session is same as result
            Assert.AreEqual(result, cache);
        }
    }
}
