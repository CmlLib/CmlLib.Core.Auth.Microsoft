using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using XboxAuthNet.OAuth.Models;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft
{
    public class TEST
    {
        public static async Task Main()
        {
            var loginHandler = LoginHandlerBuilder.Create()
                .ForJavaEdition();

            await TestSilentAuth(loginHandler);
            await TestInteractiveAuth(loginHandler);
        }

        public static async Task TestInteractiveAuth(JELoginHandler loginHandler)
        {
            var oAuthStrategy = loginHandler.CreateMicrosoftOAuthBuilder()
                .CreateInteractiveStrategy();
            
            var xboxAuthStrategy = loginHandler.CreateXboxAuthBuilder(oAuthStrategy)
                .CreateBasicXboxAuth();

            var result = await loginHandler.AuthenticateInteractively()
                .WithXboxAuth(xboxAuthStrategy)
                .ExecuteAsync();
        }

        public static async Task TestInteractiveAuthSimpler(JELoginHandler loginHandler)
        {
            var result = await loginHandler.AuthenticateInteractively()
                .MicrosoftOAuth.WithCaching(true)
                .MicrosoftOAuth.WithSessionSource(null)
                .MicrosoftOAuth.UseInteractiveStrategy()
                .XboxAuth.WithCaching(true)
                .XboxAuth.WithSessionSource(null)
                .XboxAuth.UseBasicStrategy()
                .WithCaching(true)
                .WithSessionSource(null)
                .ExecuteAsync();
        }

        public static async Task TestInteractiveAuth2(JELoginHandler loginHandler)
        {
            var result = await loginHandler.AuthenticateInteractively()
                .ExecuteAsync();
        }

        public static async Task TestSilentAuth(JELoginHandler loginHandler)
        {
            var oAuthStrategy = loginHandler.CreateMicrosoftOAuthBuilder()
                .CreateSilentStrategy();
            
            var xboxAuthStrategy = loginHandler.CreateXboxAuthBuilder(oAuthStrategy)
                .CreateBasicXboxAuth();
            
            var result = await loginHandler.AuthenticateSilently()
                .WithXboxAuth(xboxAuthStrategy)
                .ExecuteAsync();
        }
    }
}