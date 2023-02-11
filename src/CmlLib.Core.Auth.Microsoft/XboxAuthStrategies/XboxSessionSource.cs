using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.XboxAuthStrategies
{
    public class XboxSessionSource : SessionFromStorage<XboxAuthTokens>
    {
        public static string KeyName { get; } = "XboxTokens";

        public XboxSessionSource(ISessionStorage sessionStorage)
         : base(KeyName, sessionStorage)
        {

        }
    }
}