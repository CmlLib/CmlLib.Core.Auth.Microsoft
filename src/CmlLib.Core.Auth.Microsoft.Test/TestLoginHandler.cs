using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.Test.Mock;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.Test
{
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

            var loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .WithMicrosoftOAuthApi(oauthApi)
                .WithCacheManager(cacheManager)
                .WithXboxLiveApi(xboxLiveApi)
                .WithMojangXboxApi(mojangXboxApi)
                .Build();

            var objects = new MockObjects(
                cacheManager: cacheManager,
                oAuthApi: oauthApi,
                xboxLiveApi: xboxLiveApi,
                mojangXboxApi: mojangXboxApi);
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
                XboxTokens = new XboxLive.XboxAuthTokens
                {
                    XstsToken = xsts
                },
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
            Assert.AreEqual(testcase.XboxTokens?.XstsToken, result.XboxTokens?.XstsToken);
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

        public static object?[] ExpiredMojangXboxLoginResponses = new object?[]
        {
            null,
            new MojangXboxLoginResponse
            {
                AccessToken = null,
                ExpiresIn = 86400,
                ExpiresOn = DateTime.UtcNow.AddDays(10)
            },
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
        public async Task TestLoginFromCache_ValidOAuthExpiredMojang(MojangXboxLoginResponse? mcToken)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            var testcase = CreateDefaultTestCase();
            testcase.MojangXboxToken = mcToken;

            await mockObjects.CacheManager.SaveCache(testcase);

            var result = await loginHandler.LoginFromCache();

            // all tokens should be refreshed
            var cache = await mockObjects.CacheManager.ReadCache();
            Assert.NotNull(cache?.MicrosoftOAuthToken);
            Assert.NotNull(cache?.XboxTokens?.XstsToken);
            Assert.NotNull(cache?.MojangXboxToken);
            Assert.NotNull(cache?.GameSession);

            Assert.AreEqual("MockOAuthApi_GetOrRefreshTokens_AccessToken", result.MicrosoftOAuthToken?.AccessToken);
            Assert.AreEqual("MockOAuthApi_GetOrRefreshTokens_RefreshToken", result.MicrosoftOAuthToken?.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", result.XboxTokens?.XstsToken?.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", result.XboxTokens?.XstsToken?.UserHash);
            Assert.AreEqual("MockXboxLiveApi_XboxUserId", result.XboxTokens?.XstsToken?.UserXUID);
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
                Assert.NotNull(testcase.MicrosoftOAuthToken);
                testcase.MicrosoftOAuthToken!.ExpireIn = 0; // make token expired
                testcase.MojangXboxToken = mcToken;

                await mockObjects.CacheManager.SaveCache(testcase);

                await loginHandler.LoginFromCache();
            });
        }

        public static object?[] NullMSessions = new object?[]
        {
            null,
            new MSession(null, null, null),
            new MSession(null, "ac", "ud"),
            new MSession("un", null, null),
            new MSession("un", "ac", null)
        };

        [Test]
        [TestCaseSource(nameof(NullMSessions))]
        public async Task TestLoginFromCache_NullGameSession(MSession? session)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            var testcase = CreateDefaultTestCase();
            testcase.GameSession = session;

            await mockObjects.CacheManager.SaveCache(testcase);
            var result = await loginHandler.LoginFromCache();
            var cache = await mockObjects.CacheManager.ReadCache();

            Assert.NotNull(cache?.MicrosoftOAuthToken);
            Assert.NotNull(cache?.XboxTokens?.XstsToken);
            Assert.NotNull(cache?.MojangXboxToken);
            Assert.NotNull(cache?.GameSession);

            Assert.AreEqual("OldOAuthAccessToken", result.MicrosoftOAuthToken?.AccessToken);
            Assert.AreEqual("OldOAuthRefreshToken", result.MicrosoftOAuthToken?.RefreshToken);
            Assert.AreEqual("OldXstsToken", result.XboxTokens?.XstsToken?.Token);
            Assert.AreEqual("OldUserHash", result.XboxTokens?.XstsToken?.UserHash);
            Assert.AreEqual("OldXboxUserId", result.XboxTokens?.XstsToken?.UserXUID);
            Assert.AreEqual(testcase.MojangXboxToken?.AccessToken, result.MojangXboxToken?.AccessToken);
            Assert.AreEqual(testcase.MojangXboxToken?.Username, result.MojangXboxToken?.Username);

            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", result.GameSession?.Username);
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", result.GameSession?.UUID);
            Assert.AreEqual(testcase.MojangXboxToken?.AccessToken, result.GameSession?.AccessToken);
        }

        [Test]
        public async Task TestLoginFromOAuth()
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            
            var result = await loginHandler.LoginFromOAuth();

            // all tokens should be refreshed
            var cache = await mockObjects.CacheManager.ReadCache();
            Assert.NotNull(cache?.MicrosoftOAuthToken);
            Assert.NotNull(cache?.XboxTokens?.XstsToken);
            Assert.NotNull(cache?.MojangXboxToken);
            Assert.NotNull(cache?.GameSession);

            Assert.AreEqual("MockOAuthApi_RequestNewTokens_AccessToken", result.MicrosoftOAuthToken?.AccessToken);
            Assert.AreEqual("MockOAuthApi_RequestNewTokens_RefreshToken", result.MicrosoftOAuthToken?.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", result.XboxTokens?.XstsToken?.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", result.XboxTokens?.XstsToken?.UserHash);
            Assert.AreEqual("MockXboxLiveApi_XboxUserId", result.XboxTokens?.XstsToken?.UserXUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", result.MojangXboxToken?.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", result.MojangXboxToken?.Username);

            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", result.GameSession?.Username);
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", result.GameSession?.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", result.GameSession?.AccessToken);

            // check cached session is same as result
            Assert.AreEqual(result, cache);
        }

        public static object?[] MockOAuthResponse = new object?[]
        {
            null,
            new MicrosoftOAuthResponse(),
            new MicrosoftOAuthResponse
            {
                AccessToken = ""
            }
        };

        [Test]
        [TestCaseSource(nameof(MockOAuthResponse))]
        public void TestGetOrRefreshTokens(MicrosoftOAuthResponse oauth)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            mockObjects.OAuthApi.GetOrRefreshTokensReturn = oauth;

            var ex = Assert.ThrowsAsync<MicrosoftOAuthException>(async () =>
            {
                var result = await loginHandler.LoginFromCache();
            });
        }

        [Test]
        [TestCaseSource(nameof(MockOAuthResponse))]
        public void TestRequestNewTokens(MicrosoftOAuthResponse oauth)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            mockObjects.OAuthApi.RequestNewTokensReturn = oauth;

            var ex = Assert.ThrowsAsync<MicrosoftOAuthException>(async () =>
            {
                var result = await loginHandler.LoginFromOAuth();
            });
        }

        public static object?[] MockXboxAuthTokens = new object?[]
        {
            null,
            new XboxAuthTokens(),
            new XboxAuthTokens
            {
                XstsToken = new XboxAuthResponse()
            },
            new XboxAuthTokens
            {
                XstsToken = new XboxAuthResponse
                {
                    Token = "token"
                }
            },
            new XboxAuthTokens
            {
                XstsToken = new XboxAuthResponse
                {
                    XuiClaims = new XboxAuthXuiClaims
                    {
                        UserHash = "userhash"
                    }
                }
            }
        };

        [Test]
        [TestCaseSource(nameof(MockXboxAuthTokens))]
        public void TestXboxLiveApi(XboxAuthTokens tokens)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();
            mockObjects.XboxLiveApi.ReturnObject = tokens;

            var ex = Assert.ThrowsAsync<XboxAuthException>(async () =>
            {
                var result = await loginHandler.LoginFromOAuth();
            });
        }

        [Test]
        [TestCase(null, null, null)]
        [TestCase(null, "uuid", "username")]
        [TestCase("accesstoken", null, "username")]
        [TestCase("accesstoken", "uuid", null)]
        public void TestMojangXboxApi(string accessToken, string uuid, string username)
        {
            var (mockObjects, loginHandler) = CreateMockEnvironment();

            mockObjects.MojangXboxApi.MockAccessToken = accessToken;
            mockObjects.MojangXboxApi.MockProfileUUID = uuid;
            mockObjects.MojangXboxApi.MockProfileUsername = username;

            var ex = Assert.ThrowsAsync<MinecraftAuthException>(async () =>
            {
                var result = await loginHandler.LoginFromOAuth();
            });
        }
    }
}
