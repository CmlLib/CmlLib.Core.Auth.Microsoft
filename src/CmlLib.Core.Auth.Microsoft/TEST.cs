using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
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

            var result1 = await loginHandler.Authenticate().ExecuteAsync();

            var result2 = await loginHandler.Authenticate()
                .WithMicrosoftOAuth()
                .Interactive()
                .ExecuteAsync();

            var result3 = await loginHandler.Authenticate()
                .WithMicrosoftOAuth()
                .Interactive()
                .WithBasicXboxAuth()
                .ExecuteAsync();
            
            var sresult1 = await loginHandler.AuthenticateSilently()
                .WithMicrosoftOAuth()
                .Silent()
                .ExecuteAsync();
        }
    }
}