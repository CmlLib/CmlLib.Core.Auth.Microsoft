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

        public (MockCacheManager<SessionCache>, MockXboxLiveApi, MockMojangXboxApi, LoginHandler) CreateMockEnvironment()
        {
            var cacheManager = new MockCacheManager<SessionCache>();
            var xboxLiveApi = new MockXboxLiveApi();
            var mojangXboxApi = new MockMojangXboxApi();
            var loginHandler = new LoginHandler(builder =>
            {
                builder.SetXboxLiveApi(xboxLiveApi);
                builder.SetMojangXboxApi(mojangXboxApi);
                builder.SetCacheManager(cacheManager);
            });
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

            cacheManager.Cache = new SessionCache()
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mcToken,
                GameSession = msession
            };

            var resultSession = await loginHandler.LoginFromCache();

            Assert.AreEqual(msToken, cacheManager.Cache.MicrosoftOAuthToken);
            Assert.AreEqual(xsts, cacheManager.Cache.XstsToken);
            Assert.AreEqual(mcToken, cacheManager.Cache.MojangXboxToken);
            Assert.AreEqual(resultSession, cacheManager.Cache.GameSession);

            Assert.AreEqual(msession.Username, resultSession.Username);
            Assert.AreEqual(msession.UUID, resultSession.UUID);

            // accessToken should be always updated to MojangXboxLoginResponse.AccessToken
            Assert.AreEqual(NotExpiredJwt, resultSession.AccessToken);
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
            var xsts = new XboxAuthResponse();
            var msession = new MSession
            {
                AccessToken = "OldAccessToken",
                Username = "OldUsername",
                UUID = "OldUUID"
            };

            cacheManager.Cache = new SessionCache()
            {
                MicrosoftOAuthToken = msToken,
                XstsToken = xsts,
                MojangXboxToken = mcToken,
                GameSession = msession
            };

            var resultSession = await loginHandler.LoginFromCache();

            // all tokens should be refreshed
            Assert.AreEqual("MockXboxLiveApi_AccessToken", cacheManager.Cache.MicrosoftOAuthToken.AccessToken);
            Assert.AreEqual("MockXboxLiveApi_RefreshToken", cacheManager.Cache.MicrosoftOAuthToken.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", cacheManager.Cache.XstsToken.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", cacheManager.Cache.XstsToken.UserHash);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", cacheManager.Cache.MojangXboxToken.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", cacheManager.Cache.MojangXboxToken.Username);
            
            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", resultSession.Username); 
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", resultSession.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", resultSession.AccessToken); 

            // resultSession === cachedSession
            Assert.AreEqual(resultSession, cacheManager.Cache.GameSession);
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

            cacheManager.Cache = new SessionCache();

            var resultSession = await loginHandler.LoginFromOAuth();

            // all tokens should be refreshed
            Assert.AreEqual("MockXboxLiveApi_AccessToken", cacheManager.Cache.MicrosoftOAuthToken.AccessToken);
            Assert.AreEqual("MockXboxLiveApi_RefreshToken", cacheManager.Cache.MicrosoftOAuthToken.RefreshToken);
            Assert.AreEqual("MockXboxLiveApi_Token", cacheManager.Cache.XstsToken.Token);
            Assert.AreEqual("MockXboxLiveApi_UserHash", cacheManager.Cache.XstsToken.UserHash);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", cacheManager.Cache.MojangXboxToken.AccessToken);
            Assert.AreEqual("MockMojangXboxApi_Username", cacheManager.Cache.MojangXboxToken.Username);

            Assert.AreEqual("MockMojangXboxApi_ProfileUsername", resultSession.Username);
            Assert.AreEqual("MockMojangXboxApi_ProfileUUID", resultSession.UUID);
            Assert.AreEqual("MockMojangXboxApi_AccessToken", resultSession.AccessToken);

            // resultSession === cachedSession
            Assert.AreEqual(resultSession, cacheManager.Cache.GameSession);
        }
    }
}
