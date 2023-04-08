using System;
using System.IO;
using XboxAuthNet.Game;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Bedrock.Auth
{
    public static class Extensions
    {
        public static BELoginHandler ForBedrockEdition(this LoginHandlerBuilder self)
        {
            var sessionStorage = self.SessionStorage ?? 
                new JsonFileSessionStorage(Path.Combine(Environment.CurrentDirectory, "cmllib_bedrock.json"));
            return new BELoginHandler(self.HttpClient, sessionStorage);
        }
    }
}