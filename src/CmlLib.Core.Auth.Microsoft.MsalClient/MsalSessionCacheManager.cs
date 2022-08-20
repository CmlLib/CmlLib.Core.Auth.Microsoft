using CmlLib.Core.Auth.Microsoft.Cache;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalSessionCacheManager<T> : JsonFileCacheManager<T>
        where T : SessionCacheBase
    {
        private readonly IPublicClientApplication app;

        public MsalSessionCacheManager(IPublicClientApplication app, string filepath) : base(filepath)
        {
            this.app = app;
        }

        // Microsoft OAuth tokens should be managed by MSAL.NET
        // SaveCache method does not cache OAuth tokens. only caching GameSession and XboxSession
        public override Task SaveCache(T? obj)
        {
            if (obj != null)
                obj.MicrosoftOAuthToken = null;
            return base.SaveCache(obj);
        }

        public override Task ClearCache()
        {
            return MsalMinecraftLoginHelper.RemoveAccounts(app);
        }
    }
}
