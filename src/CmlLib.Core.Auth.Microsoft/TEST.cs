using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft
{
    public class TEST
    {
        public static async Task Main()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var result1 = await loginHandler.AuthenticateInteractively()
                .WithInteractiveMicrosoftOAuth(new MicrosoftOAuthParameters())
                .ExecuteAsync();

            var result2 = await loginHandler.AuthenticateInteractively()
                .WithInteractiveMicrosoftOAuth(builder => builder
                    .WithUIOptions(new WebUIOptions
                    {
                        ParentObject = new object()
                    }), 
                    new MicrosoftOAuthParameters())
                .WithBasicXboxAuth()
                .ExecuteAsync();
            
            var result3 = await loginHandler.AuthenticateSilently()
                .WithSilentMicrosoftOAuth()
                .WithBasicXboxAuth()
                .ExecuteAsync();
            
            var result4 = await loginHandler.AuthenticateSilently()
                .ExecuteAsync();

            var result5 = await loginHandler.Authenticate();
        }
    }
}