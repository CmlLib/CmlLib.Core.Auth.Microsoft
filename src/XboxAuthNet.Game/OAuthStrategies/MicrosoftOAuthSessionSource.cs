using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.OAuthStrategies
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