using System.IO;
using XboxAuthNet.Game;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft
{
    public static class Extensions
    {
        public static string DefaultSessionStoragePath => Path.Combine(MinecraftPath.GetOSDefaultPath(), "cmllib_session.json");

        public static JELoginHandler ForJavaEdition(this LoginHandlerBuilder self)
        {
            var sessionStorage = self.SessionStorage ?? new JsonFileSessionStorage(DefaultSessionStoragePath);
            return new JELoginHandler(self.HttpClient, sessionStorage);
        }
    }
}