using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;

namespace MsalClientTest
{
    class Example
    {
        public static async Task Start()
        {
            // Session variable to store minecraft login result
            MSession? session = null;

            // Create IPublicClientApplication
            var app = await MsalMinecraftLoginHelper.BuildApplicationWithCache("CLIENT-ID");

            // Choose login method
            Console.WriteLine("Choose login method: ");
            Console.WriteLine("1. DeviceCode   2. WebBrowser   3. EmbeddedWebView");
            int loginMode = int.Parse(Console.ReadLine() ?? "2");

            try
            {
                if (loginMode == 1)
                {
                    session = await LoginDeviceCode(app);
                }
                else if (loginMode == 2)
                {
                    session = await LoginInteractive(app);
                }
                else if (loginMode == 3)
                {
                    session = await LoginEmbeddedWebView(app);
                }
                else
                {
                    return;
                }

                Console.WriteLine("Login Success: " + session.Username);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

            Console.ReadLine();
        }

        public static async Task<MSession> LoginInteractive(IPublicClientApplication app)
        {
            var loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .WithMsalOAuth(app, factory => factory.CreateInteractiveApi())
                .Build();

            Console.WriteLine("Login mode: Interactive");

            try
            {
                var session = await loginHandler.LoginFromCache();
                return session.GameSession;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var session = await loginHandler.LoginFromOAuth();
                return session.GameSession;
            }
        }

        public static async Task<MSession> LoginEmbeddedWebView(IPublicClientApplication app)
        {
            var loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .WithMsalOAuth(app, factory => factory.CreateWithEmbeddedWebView())
                .Build();

            Console.WriteLine("Login mode: WithEmbeddedWebView");

            try
            {
                var session = await loginHandler.LoginFromCache();
                return session.GameSession;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var session = await loginHandler.LoginFromOAuth();
                return session.GameSession;
            }
        }

        public static async Task<MSession> LoginDeviceCode(IPublicClientApplication app)
        {
            var loginHandler = new LoginHandlerBuilder()
                .ForJavaEdition()
                .WithMsalOAuth(app, factory => factory.CreateDeviceCodeApi(result =>
                {
                    Console.WriteLine($"Code: {result.UserCode}, ExpiresOn: {result.ExpiresOn.LocalDateTime}");
                    Console.WriteLine(result.Message);
                    return Task.CompletedTask;
                }))
            .Build();

            Console.WriteLine("Login mode: WithDeviceCode");

            try
            {
                var session = await loginHandler.LoginFromCache();
                return session.GameSession;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var session = await loginHandler.LoginFromOAuth();
                return session.GameSession;
            }
        }
    }
}
