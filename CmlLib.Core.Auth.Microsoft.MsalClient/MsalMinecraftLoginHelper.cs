using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalMinecraftLoginHelper
    {
        public static readonly string[] DefaultScopes = new[] { "XboxLive.signin" };

        public static PublicClientApplicationBuilder CreateDefaultApplicationBuilder(string cid)
            => PublicClientApplicationBuilder.Create(cid)
                .WithTenantId("consumers")
                .WithRedirectUri("http://localhost");

        public static Task RegisterCache(IPublicClientApplication app, MsalCacheSettings cacheSettings)
            => RegisterCache(app, cacheSettings.ToStorageCreationPropertiesBuilder().Build());

        public async static Task RegisterCache(IPublicClientApplication app, StorageCreationProperties storageProperties)
        {
            var cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties);
            cacheHelper.RegisterCache(app.UserTokenCache);
        }

        public static IPublicClientApplication BuildApplication(string cid)
            => CreateDefaultApplicationBuilder(cid).Build();

        public static Task<IPublicClientApplication> BuildApplicationWithCache(string cid)
            => BuildApplicationWithCache(cid, new MsalCacheSettings());

        public async static Task<IPublicClientApplication> BuildApplicationWithCache(string cid, StorageCreationProperties storageProperties)
        {
            var app = BuildApplication(cid);
            await RegisterCache(app, storageProperties);
            return app;
        }

        public async static Task<IPublicClientApplication> BuildApplicationWithCache(string cid, MsalCacheSettings cacheSettings)
        {
            var storageProperties = cacheSettings.ToStorageCreationPropertiesBuilder().Build();
            return await BuildApplicationWithCache(cid, storageProperties);
        }

        public async static Task<MSession> LoginSilent(IPublicClientApplication app)
        {
            var accounts = await app.GetAccountsAsync();
            var result = await app.AcquireTokenSilent(DefaultScopes, accounts.FirstOrDefault())
                .ExecuteAsync();
            return LoginWithMsalResult(result);
        }

        public static Task<MSession> Login(IPublicClientApplication app,
            bool trySilent = true, bool useEmbeddedWebView = false)
            => Login(app, null, trySilent, useEmbeddedWebView);

        public async static Task<MSession> Login(IPublicClientApplication app, CancellationToken? cancellationToken,
            bool trySilent=true, bool useEmbeddedWebView=false)
        {
            var accounts = await app.GetAccountsAsync();

            AuthenticationResult result;
            try
            {
                if (trySilent)
                {
                    var t = app.AcquireTokenSilent(DefaultScopes, accounts.FirstOrDefault());

                    if (cancellationToken.HasValue)
                        result = await t.ExecuteAsync(cancellationToken.Value);
                    else
                        result = await t.ExecuteAsync();
                }
                else
                    throw new MsalUiRequiredException("", "trySilent option was false");
            }
            catch (MsalUiRequiredException)
            {
                var t = app.AcquireTokenInteractive(DefaultScopes)
                    .WithUseEmbeddedWebView(useEmbeddedWebView);

                if (cancellationToken.HasValue)
                    result = await t.ExecuteAsync(cancellationToken.Value);
                else
                    result = await t.ExecuteAsync();
            }

            return LoginWithMsalResult(result);
        }

        public static MSession LoginWithMsalResult(AuthenticationResult result)
        {
            var handler = new LoginHandler(cacheManager: null);
            var session = handler.LoginFromOAuth(new MicrosoftOAuthResponse
            {
                AccessToken = "d=" + result.AccessToken,
                UserId = result.UniqueId,
                TokenType = result.TokenType,
                Scope = string.Join(",", result.Scopes)
            });

            return session;
        }

        public static async Task RemoveAccounts(IPublicClientApplication app)
        {
            var accounts = await app.GetAccountsAsync();
            while (accounts.Any())
            {
                var first = accounts.First();
                await app.RemoveAsync(first);
                accounts = await app.GetAccountsAsync();
            }
        }
    }
}
