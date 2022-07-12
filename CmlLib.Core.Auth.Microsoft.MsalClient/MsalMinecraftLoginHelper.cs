using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

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
    }
}
