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
    public class MsalMinecraftLoginHandler<T> 
        where T : SessionCacheBase
    {
        public AbstractLoginHandler<T> LoginHandler { get; private set; }

        private readonly IPublicClientApplication app;

        public MsalMinecraftLoginHandler(IPublicClientApplication application, AbstractLoginHandler<T> loginHandler)
        {
            this.app = application;
            this.LoginHandler = loginHandler;
        }

        public async Task<T> LoginSilent()
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

        public Task<T> LoginInteractive(bool useEmbeddedWebView = false)
            => LoginInteractive(null, useEmbeddedWebView);

        public async Task<T> LoginInteractive(CancellationToken? cancellationToken, bool useEmbeddedWebView = false)
        {
            var msalLoginResult = await msalAcquireTokenInteractive(cancellationToken, useEmbeddedWebView);
            return await LoginWithMsalResult(msalLoginResult);
        }

        public async Task<T> LoginDeviceCode(Func<DeviceCodeResult, Task> deviceCodeResultCallback)
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

        public async Task<T> LoginWithMsalResult(AuthenticationResult result)
        {
            var msToken = MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
            var session = await LoginHandler.GetAllTokens(msToken);
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
