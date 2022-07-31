using CmlLib.Core.Auth.Microsoft.Cache;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
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

        private readonly IPublicClientApplication app;

        public MsalMinecraftLoginHandler(IPublicClientApplication application) : this(application, builder => { })
        {

        }

        public MsalMinecraftLoginHandler(IPublicClientApplication application, Action<LoginHandlerBuilder> builder)
        {
            var defaultPath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_msalsession.json");
            this.app = application;
            this.LoginHandler = new LoginHandler(innerBuilder =>
            {
                innerBuilder.SetCacheManager(new MsalSessionCacheManager(defaultPath));
                builder.Invoke(innerBuilder);
            });
        }

        public MsalMinecraftLoginHandler(IPublicClientApplication application, LoginHandler loginHandler)
        {
            this.app = application;
            this.LoginHandler = loginHandler;
        }

        public async Task<MSession> LoginSilent()
        {
            try
            {
                return await LoginHandler.LoginFromCache();
            }
            catch (Exception ex)
            {
                var pass = ex is MicrosoftOAuthException ||
                           ex is XboxAuthNet.XboxLive.XboxAuthException ||
                           ex is Mojang.MinecraftAuthException;

                if (!pass)
                    throw;
            }

            var msalLoginResult = await msalAcquireTokenSilent();
            return await LoginWithMsalResult(msalLoginResult);
        }

        public Task<MSession> LoginInteractive(bool useEmbeddedWebView = false)
            => LoginInteractive(null, useEmbeddedWebView);

        public async Task<MSession> LoginInteractive(CancellationToken? cancellationToken, bool useEmbeddedWebView = false)
        {
            var msalLoginResult = await msalAcquireTokenInteractive(cancellationToken, useEmbeddedWebView);
            return await LoginWithMsalResult(msalLoginResult);
        }

        public async Task<MSession> LoginDeviceCode(Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            var result = await app.AcquireTokenWithDeviceCode(MsalMinecraftLoginHelper.DefaultScopes, deviceCodeResultCallback)
                .ExecuteAsync();
            return await LoginWithMsalResult(result);
        }

        protected virtual async Task<AuthenticationResult> msalAcquireTokenSilent()
        {
            var accounts = await app.GetAccountsAsync();
            var result = await app.AcquireTokenSilent(MsalMinecraftLoginHelper.DefaultScopes, accounts.FirstOrDefault())
                .ExecuteAsync();
            return result;
        }

        protected virtual async Task<AuthenticationResult> msalAcquireTokenInteractive(CancellationToken? cancellationToken, bool useEmbeddedWebView)
        {
            var t = app.AcquireTokenInteractive(MsalMinecraftLoginHelper.DefaultScopes)
                .WithUseEmbeddedWebView(useEmbeddedWebView);

            AuthenticationResult result;
            if (cancellationToken.HasValue)
                result = await t.ExecuteAsync(cancellationToken.Value);
            else
                result = await t.ExecuteAsync();
            return result;
        }

        public async Task<MSession> LoginWithMsalResult(AuthenticationResult result)
        {
            var msToken = MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
            var session = await LoginHandler.LoginFromOAuth(msToken);
            return session;
        }

        public async Task RemoveAccounts()
        {
            await MsalMinecraftLoginHelper.RemoveAccounts(app);

            // save empty session
            LoginHandler.ClearCache();
        }
    }
}
