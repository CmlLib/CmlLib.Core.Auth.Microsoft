using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    public class Sample
    {
        public static async Task<MSession> Simplest()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var session = await loginHandler.Authenticate();
            return session;
        }

        public static async Task<MSession> Interactively()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var session = await loginHandler.AuthenticateInteractively()
                .ExecuteForLauncherAsync();

            return session;
        }

        public static async Task<MSession> Silently()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();
            
            var session = await loginHandler.AuthenticateSilently()
                .ExecuteForLauncherAsync();

            return session;
        }

        public static async Task<MSession> InteractivelyWithOptions()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var sessionStorage = new InMemorySessionStorage();

            var session = await loginHandler.AuthenticateInteractively()
                .WithSessionStorage(sessionStorage)
                .WithCaching(true)
                .MicrosoftOAuth.WithCaching(true)
                .MicrosoftOAuth.UseInteractiveStrategy(new MicrosoftOAuthParameters
                {
                    LoginHint = "hello@gmail.com",
                    Prompt = MicrosoftOAuthPromptModes.SelectAccount                   
                })
                .XboxAuth.WithCaching(true)
                .XboxAuth.UseBasicStrategy()
                .ExecuteForLauncherAsync();

            return session;
        }
    }
}