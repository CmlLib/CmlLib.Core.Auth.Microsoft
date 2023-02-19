using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
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
            
            var tresult1 = await loginHandler
                .Authenticate().Interactively()
                .WithMicrosoftOAuth().Interactive()
                .WithBasicXboxAuth()
                .ExecuteAsync();

            var result1 = await loginHandler
                .Authenticate()
                .ExecuteAsync();

            var result2 = await loginHandler
                .Authenticate().Interactively()
                .WithMicrosoftOAuth().Interactive()
                .ExecuteAsync();

            var result3 = await loginHandler
                .Authenticate().Interactively()
                .WithMicrosoftOAuth().Interactive()
                .WithBasicXboxAuth()
                .ExecuteAsync();
            
            var result4 = await loginHandler
                .Authenticate().Interactively()
                .ExecuteAsync();

            var sresult1 = await loginHandler
                .Authenticate().Silently()
                .WithSilentMicrosoftOAuth().Silent()
                .ExecuteAsync();

            var sresult2 = await loginHandler
                .Authenticate().Silently()
                .ExecuteAsync();
        }
    }
}