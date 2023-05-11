using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    public class Sample
    {
        public static async Task<MSession> Simplest()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            var session = await loginHandler.Authenticate();
            return session;
        }

        public static async Task<MSession> Interactively()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            var session = await loginHandler.AuthenticateInteractively()
                .ExecuteForLauncherAsync();

            return session;
        }

        public static async Task<MSession> Silently()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            var session = await loginHandler.AuthenticateSilently()
                .ExecuteForLauncherAsync();

            return session;
        }

        public static async Task<MSession> InteractivelyWithOptionsNew()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();

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

        public static async Task Signout()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            await loginHandler.Signout();
        }

        public static async Task Signout2()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            var sessionStorage = new InMemorySessionStorage();

            await loginHandler.CreateSignout()
                .WithSessionStorage(sessionStorage)
                .AddMicrosoftOAuthSignout()
                .ExecuteAsync();
        }
    }
}