using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.Mojang;
using CmlLib.Core.Auth.Microsoft.Test.Mock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    internal class TestLoginHandler
    {
        public const string NotExpiredJwt = ".eyJleHAiOjE5MDk2NjEzMTh9."; // exp: 2030.07.07
        public const string ExpiredJwt = ".eyJleHAiOjk2Mjk3NjUxOH0."; // exp: 2000.07.07

        public (MockCacheManager<JavaEditionSessionCache>, MockXboxLiveApi, MockMojangXboxApi, JavaEditionLoginHandler) CreateMockEnvironment()
        {
            var cacheManager = new MockCacheManager<JavaEditionSessionCache>();
            var xboxLiveApi = new MockXboxLiveApi();
            var mojangXboxApi = new MockMojangXboxApi();
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition()
                .SetXboxLiveApi(xboxLiveApi)
                .SetMojangXboxApi(mojangXboxApi)
                .SetCacheManager(cacheManager)
                .Build();

            return (cacheManager, xboxLiveApi, mojangXboxApi, loginHandler);
        }

        [Test]
        public async Task TestLoginFromCache_ValidMojangSession()
        {
            var (cacheManager, xboxLiveApi, mojangXboxApi, loginHandler) = CreateMockEnvironment();

            var msToken = new MicrosoftOAuthResponse();
            var xsts = new XboxAuthResponse();
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

            cacheManager.Cache = new JavaEditionSessionCache()
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mcToken,
                GameSession = msession
            };

            var resultSession = await loginHandler.LoginFromCache();

            var cache = cacheManager.ReadCache();
            Assert.AreEqual(msToken, cache.MicrosoftOAuthToken);
            Assert.AreEqual(xsts, cache.XstsToken);
            Assert.AreEqual(mcToken, cache.MojangXboxToken);
            Assert.AreEqual(resultSession.GameSession, cache.GameSession);

            Assert.AreEqual(msession.Username, resultSession.GameSession.Username);
            Assert.AreEqual(msession.UUID, resultSession.GameSession.UUID);

            // accessToken should be always updated to MojangXboxLoginResponse.AccessToken
            Assert.AreEqual(NotExpiredJwt, resultSession.GameSession.AccessToken);
        }

        public static object[] ExpiredMicrosoftOAuthResponseCases = new object[]
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
        [TestCaseSource(nameof(ExpiredMicrosoftOAuthResponseCases))]
        public async Task TestLoginFromCache_ExpiredMojangSession(MojangXboxLoginResponse mcToken)
        {
            var (cacheManager, xboxLiveApi, mojangXboxApi, loginHandler) = CreateMockEnvironment();

            var msToken = new MicrosoftOAuthResponse()
            {
                RefreshToken = "test_RefreshToken"
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
            var msession = new MSession
            {
                AccessToken = "OldAccessToken",
                Username = "OldUsername",
                UUID = "OldUUID"
            };

            cacheManager.Cache = new JavaEditionSessionCache()
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mcToken,
                GameSession = msession
            };

            var resultSession = await loginHandler.LoginFromCache();

            // all tokens should be refreshed
            var cache = cacheManager.ReadCache();
            Assert.NotNull(cache.MicrosoftOAuthToken);
            Assert.NotNull(cache.MojangXboxToken);
            Assert.NotNull(cache.XstsToken);

            Assert.AreEqual("MockXboxLiveApi_AccessToken", cache.MicrosoftOAuthToken!.AccessToken);
            Assert.AreEqual("MockXboxLiveApi_RefreshToken", cache.MicrosoftOAuthToken!.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", cache.XstsToken!.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", cache.XstsToken!.UserHash);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", cache.MojangXboxToken!.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", cache.MojangXboxToken!.Username);
            
            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", resultSession.GameSession.Username); 
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", resultSession.GameSession.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", resultSession.GameSession.AccessToken); 

            // resultSession === cachedSession
            Assert.AreEqual(resultSession?.GameSession, cache.GameSession);
        }

        [Test]
        public async Task TestLoginFromOAuth()
        {
            var (cacheManager, xboxLiveApi, mojangXboxApi, loginHandler) = CreateMockEnvironment();
            var oauthResponse = new MicrosoftOAuthResponse
            {
                AccessToken = "test_AccessToken",
                RefreshToken = "test_RefreshToken"
            };

            cacheManager.Cache = new JavaEditionSessionCache();

            var resultSession = await loginHandler.LoginFromOAuth();

            // all tokens should be refreshed
            var cache = cacheManager.ReadCache();
            Assert.NotNull(cache.MicrosoftOAuthToken);
            Assert.NotNull(cache.MojangXboxToken);
            Assert.NotNull(cache.XstsToken);

            Assert.AreEqual("MockXboxLiveApi_AccessToken", cache.MicrosoftOAuthToken!.AccessToken);
            Assert.AreEqual("MockXboxLiveApi_RefreshToken", cache.MicrosoftOAuthToken!.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", cache.XstsToken!.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", cache.XstsToken!.UserHash);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", cache.MojangXboxToken!.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", cache.MojangXboxToken!.Username);

            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", resultSession.GameSession.Username);
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", resultSession.GameSession.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", resultSession.GameSession.AccessToken);

            // resultSession === cachedSession
            Assert.AreEqual(resultSession.GameSession, cache.GameSession);
        }
    }
}
