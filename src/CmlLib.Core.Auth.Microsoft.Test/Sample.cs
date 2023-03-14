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

        public static async Task<MSession> InteractivelyWithOptionsNew1()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var sessionStorage = new InMemorySessionStorage();

            var builder = loginHandler.AuthenticateInteractively()
                .WithSessionStorage(sessionStorage)
                .WithCaching(true);

            var ooo = builder.CreateMicrosoftOAuthBuilder()
                .WithCaching(true)
                .UseInteractiveStrategy()
                .Build();

            var xxx = builder.CreateXboxAuthBuilder(ooo)
                .WithCaching(true)
                .UseBasicStrategy()
                .Build();

            var session = await builder.ExecuteForLauncherAsync(xxx);
        }

        public static async Task<MSession> InteractivelyWithOptionsNew2()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var sessionStorage = new InMemorySessionStorage();

            var session = await loginHandler.AuthenticateInteractively()
                .WithSessionStorage(sessionStorage)
                .WithCaching(true)
                .WithMicrosoftOAuth(builder => builder
                    .WithCaching(true)
                    .UseInteractiveStrategy())
                .WithMsalClient(builder => builder
                    .WithCaching(true)
                    .UseInteractiveStrategy())
                .WithXboxAuth(builder => builder
                    .WithCaching(true)
                    .UseBasicStrategy())
                .ExecuteForLauncherAsync();
        }

        public static async Task<MSession> InteractivelyWithOptionsNew3()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var sessionStorage = new InMemorySessionStorage();

            // extension method is very useful to extend closed class
            // extension method cannot add new state in the class

            var session = await loginHandler.AuthenticateInteractively()
                .WithSessionStorage(sessionStorage)
                .WithCaching(true)
                .MicrosoftOAuth.WithCaching(true) // require state for MicrosoftOAuthStrategyBuilder, this force JEABuilder to hold MicrosoftOAuth instance.
                .MicrosoftOAuth.UseInteractiveStrategy()
                .WithMsalOAuth(builder => builder
                    .WithCaching(true)
                    .UseInteractiveStrategy())
                .XboxAuth.WithCaching(true)
                .XboxAuth.UseBasicStrategy()
                .ExecuteForLauncherAsync();
        }

        public static async Task<MSession> InteractivelyWithOptionsNew4()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();
            
            var sessionStorage = new InMemorySessionStorage();

            var session = await loginHandler.AuthenticateInteractively()
                .WithSessionStorage(sessionStorage)
                .WithCaching(true)
                .WithMicrosoftOAuth()
                    .MicrosoftOAuth.WithCaching(true)
                    .MicrosoftOAuth.UseInteractiveStrategy()
                    .XboxAuth.WithCaching(true)
                    .XboxAuth.UseBasicStrategy()
                    .Build()
                .ExecuteForLauncherAsync();

            return session;
        }

        public static async Task<MSession> InteractivelyWithOptionsNew5()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            var sessionStorage = new InMemorySessionStorage();

            var session = await loginHandler.AuthenticateSilently()
                .WithSessionStorage(sessionStorage)
                .WithCaching(true)
                .WithMicrosoftOAuth(builder => builder
                    .MicrosoftOAuth.WithCaching(true)
                    .MicrosoftOAuth.UseInteractiveStrategy()
                    .XboxAuth.WithCaching(true)
                    .XboxAuth.UseBasicStrategy())
                .ExecuteForLauncherAsync();
            
            return session;
        }
    }
}