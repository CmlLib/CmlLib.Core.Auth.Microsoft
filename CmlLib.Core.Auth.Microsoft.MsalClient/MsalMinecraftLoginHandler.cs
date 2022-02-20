using Microsoft.Identity.Client;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalMinecraftLoginHandler
    {
        IPublicClientApplication app;
        ICacheManager<SessionCache> cacheManager;
        LoginHandler loginHandler;

        public MsalMinecraftLoginHandler(IPublicClientApplication application)
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_msalsession.json");
            this.app = application;
            this.cacheManager = new MsalSessionCacheManager(defaultPath);
            this.loginHandler = new LoginHandler(cacheManager);
        }

        public MsalMinecraftLoginHandler(IPublicClientApplication application, string cacheFilePath)
        { 
            this.app = application;
            this.cacheManager = new MsalSessionCacheManager(cacheFilePath);
            this.loginHandler = new LoginHandler(cacheManager);
        }

        public MsalMinecraftLoginHandler(IPublicClientApplication application, ICacheManager<SessionCache> _cacheManager)
        {
            this.app = application;
            this.cacheManager = _cacheManager;
            this.loginHandler = new LoginHandler(cacheManager);
        }

        public async Task<MSession> LoginSilent()
        {
            var cachedSession = loginHandler.LoginFromCache();
            if (cachedSession != null)
                return cachedSession;

            var accounts = await app.GetAccountsAsync();
            var result = await app.AcquireTokenSilent(MsalMinecraftLoginHelper.DefaultScopes, accounts.FirstOrDefault())
                .ExecuteAsync();
            return LoginWithMsalResult(result);
        }

        public Task<MSession> LoginInteractive(bool useEmbeddedWebView = false)
            => LoginInteractive(null, useEmbeddedWebView);

        public async Task<MSession> LoginInteractive(CancellationToken? cancellationToken, bool useEmbeddedWebView = false)
        {
            var t = app.AcquireTokenInteractive(MsalMinecraftLoginHelper.DefaultScopes)
                .WithUseEmbeddedWebView(useEmbeddedWebView);

            AuthenticationResult result;
            if (cancellationToken.HasValue)
                result = await t.ExecuteAsync(cancellationToken.Value);
            else
                result = await t.ExecuteAsync();

            return LoginWithMsalResult(result);
        }

        public async Task<MSession> LoginDeviceCode(Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            var result = await app.AcquireTokenWithDeviceCode(MsalMinecraftLoginHelper.DefaultScopes, deviceCodeResultCallback)
                .ExecuteAsync();
            return LoginWithMsalResult(result);
        }

        public MSession LoginWithMsalResult(AuthenticationResult result)
        {
            var session = loginHandler.LoginFromOAuth(new MicrosoftOAuthResponse
            {
                AccessToken = "d=" + result.AccessToken, // token prefix
                UserId = result.UniqueId,
                TokenType = result.TokenType,
                Scope = string.Join(",", result.Scopes)
            });

            return session;
        }

        public async Task RemoveAccounts()
        {
            var accounts = await app.GetAccountsAsync();
            while (accounts.Any())
            {
                var first = accounts.First();
                await app.RemoveAsync(first);
                accounts = await app.GetAccountsAsync();
            }

            // save empty session
            cacheManager.SaveCache(new SessionCache());
        }
    }
}
