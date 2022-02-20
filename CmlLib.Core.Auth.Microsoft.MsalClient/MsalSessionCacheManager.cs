using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalSessionCacheManager : JsonFileCacheManager<SessionCache>
    {
        public MsalSessionCacheManager(string filepath) : base(filepath)
        {
        }

        // Microsoft OAuth tokens should be managed by MSAL.NET
        // SaveCAche method does not cache OAuth tokens. only caching GameSession and XboxSession
        public override void SaveCache(SessionCache obj)
        {
            obj.MicrosoftOAuthSession = null;
            base.SaveCache(obj);
        }
    }
}
