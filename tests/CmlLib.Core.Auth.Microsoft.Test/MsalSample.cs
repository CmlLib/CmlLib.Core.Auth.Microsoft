using System.Threading.Tasks;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Msal;
using Microsoft.Identity.Client;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    public class MsalSample
    {
        IPublicClientApplication app = null!;

        public async Task Setup()
        {
            app = await MsalClientHelper.BuildApplicationWithCache("499c8d36-be2a-4231-9ebd-ef291b7bb64c");
        }

        public async Task<MSession> Silently()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();

            var session = await loginHandler.AuthenticateSilently()
                .WithMsalOAuth(builder => builder
                    .MsalOAuth.UseSilentStrategy(app))
                .ExecuteForLauncherAsync();
            return session;
        }

        public async Task<MSession> Interactively()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();

            var session = await loginHandler.AuthenticateInteractively()
                .WithMsalOAuth(builder => builder
                    .MsalOAuth.UseInteractiveStrategy(app))
                .ExecuteForLauncherAsync();
            return session;
        }

        public async Task<MSession> DeviceCode()
        {
            var loginHandler = JELoginHandlerBuilder.BuildDefault();

            var session = await loginHandler.AuthenticateInteractively()
                .WithMsalOAuth(builder => builder
                    .MsalOAuth.UseDeviceCodeStrategy(app, deviceCodeHandler))
                .ExecuteForLauncherAsync();
            return session;
        }

        private Task deviceCodeHandler(DeviceCodeResult deviceCode)
        {
            Console.WriteLine(deviceCode.Message);
            return Task.CompletedTask;
        }
    }
}