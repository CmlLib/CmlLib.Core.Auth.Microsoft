using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.XboxAuthStrategies
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