using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft
{
    public class TEST
    {

        public static async Task Main()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            loginHandler.Authenticate()
                .WithSessionStorage()
                .WithSessionSource()
                .WithMicrosoftOAuth()
                .WithXboxAuth()
                .ExecuteAsync();
        }
    }
}