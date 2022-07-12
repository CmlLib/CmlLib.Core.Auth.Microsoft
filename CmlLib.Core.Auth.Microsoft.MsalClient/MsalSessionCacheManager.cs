using CmlLib.Core.Auth.Microsoft.Cache;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalSessionCacheManager : JsonFileCacheManager<SessionCache>
    {
        public MsalSessionCacheManager(string filepath) : base(filepath)
        {
        }

        // Microsoft OAuth tokens should be managed by MSAL.NET
        // SaveCache method does not cache OAuth tokens. only caching GameSession and XboxSession
        public override void SaveCache(SessionCache obj)
        {
            obj.MicrosoftOAuthToken = null;
            base.SaveCache(obj);
        }
    }
}
