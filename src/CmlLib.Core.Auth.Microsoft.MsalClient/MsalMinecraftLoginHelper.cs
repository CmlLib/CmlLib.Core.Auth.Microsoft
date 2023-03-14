﻿using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Linq;
using System.Threading.Tasks;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public static class MsalMinecraftLoginHelper
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

        public static MicrosoftOAuthResponse ToMicrosoftOAuthResponse(AuthenticationResult result)
        {
            return new MicrosoftOAuthResponse
            {
                AccessToken = "d=" + result.AccessToken, // token prefix
                UserId = result.UniqueId,
                TokenType = result.TokenType,
                Scope = string.Join(",", result.Scopes)
            };
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
