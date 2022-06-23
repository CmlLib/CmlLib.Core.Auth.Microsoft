using CmlLib.Core.Auth.Microsoft.Cache;
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
        public LoginHandler LoginHandler { get; private set; }

        IPublicClientApplication app;

        public MsalMinecraftLoginHandler(IPublicClientApplication application)
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_msalsession.json");
            this.app = application;
            this.LoginHandler = new LoginHandler(builder =>
            {
                builder.SetCacheManager(new MsalSessionCacheManager(defaultPath));
            });
        }

        public MsalMinecraftLoginHandler(IPublicClientApplication application, Action<LoginHandlerBuilder> builder)
        {
            this.app = application;
            this.LoginHandler = new LoginHandler(builder);
        }

        public async Task<MSession> LoginSilent()
        {
            var cachedSession = await LoginHandler.LoginFromCache();
            if (cachedSession != null)
                return cachedSession;

            var accounts = await app.GetAccountsAsync();
            var result = await app.AcquireTokenSilent(MsalMinecraftLoginHelper.DefaultScopes, accounts.FirstOrDefault())
                .ExecuteAsync();
            return await LoginWithMsalResult(result);
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

            return await LoginWithMsalResult(result);
        }

        public async Task<MSession> LoginDeviceCode(Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            var result = await app.AcquireTokenWithDeviceCode(MsalMinecraftLoginHelper.DefaultScopes, deviceCodeResultCallback)
                .ExecuteAsync();
            return await LoginWithMsalResult(result);
        }

        public async Task<MSession> LoginWithMsalResult(AuthenticationResult result)
        {
            var session = await LoginHandler.LoginFromOAuth(new MicrosoftOAuthResponse
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
            LoginHandler.ClearCache();
        }
    }
}
