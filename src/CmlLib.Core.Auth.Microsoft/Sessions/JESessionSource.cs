using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Sessions
{
    public class JESessionSource : SessionFromStorage<JESession>
    {
        public static string KeyName { get; } = "JESession";

        public JESessionSource(ISessionStorage sessionStorage) : base(KeyName, sessionStorage)
        {

        }
    }
}