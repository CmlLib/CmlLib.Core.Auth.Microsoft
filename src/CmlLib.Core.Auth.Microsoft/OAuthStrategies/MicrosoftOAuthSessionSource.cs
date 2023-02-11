using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.OAuthStrategies
{
    public class MicrosoftOAuthSessionSource : SessionFromStorage<MicrosoftOAuthResponse>
    {
        public static string KeyName { get; } = "MicrosoftOAuth";
        public MicrosoftOAuthSessionSource(ISessionStorage sessionStorage)
         : base(KeyName, sessionStorage)
        {

        }
    }
}