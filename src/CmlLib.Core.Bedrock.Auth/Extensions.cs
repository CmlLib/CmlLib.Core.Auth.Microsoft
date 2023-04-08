using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Bedrock.Auth
{
    public static class Extensions
    {
        public static BELoginHandler ForBedrockEdition(this LoginHandlerBuilder self)
        {
            var sessionStorage = self.SessionStorage ?? new JsonFileSessionStorage(self.DefaultSessionStoragePath);
            return new BELoginHandler(self.HttpClient, sessionStorage);
        }
    }
}