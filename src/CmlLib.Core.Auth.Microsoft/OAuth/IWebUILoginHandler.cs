using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public interface IWebUILoginHandler
    {
        string CreateOAuthUrl();
        MicrosoftOAuthCodeCheckResult CheckOAuthCodeResult(Uri uri);
    }
}
