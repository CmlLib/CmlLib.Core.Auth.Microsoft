using CmlLib.Core.Auth;
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
            var app = MsalMinecraftLoginHelper.CreateDefaultApplicationBuilder("CLIENT-ID")
                .Build();

            // Initialize MsalMinecraftLoginHelper
            var handler = new MsalMinecraftLoginHandler(app);

            try
            {
                // Try login with cached session
                Console.WriteLine("Start LoginSilent");
                session = await handler.LoginSilent();
            }
            catch (MsalUiRequiredException)
            {
                // Choose login method
                Console.WriteLine("Choose login method: ");
                Console.WriteLine("1. DeviceCode   2. WebBrowser   3. EmbeddedWebView");
                int.TryParse(Console.ReadLine(), out int loginMode);

                if (loginMode == 1)
                {
                    // Login with DeviceCode
                    Console.WriteLine("Start LoginDeviceCode");
                    session = await handler.LoginDeviceCode(result =>
                    {
                        Console.WriteLine($"Code: {result.UserCode}, ExpiresOn: {result.ExpiresOn.LocalDateTime}");
                        Console.WriteLine(result.Message);
                        return Task.CompletedTask;
                    });
                }
                else if (loginMode == 2)
                {
                    // Login with web browser
                    Console.WriteLine("Start LoginInteraction");
                    session = await handler.LoginInteractive();
                }
                else if (loginMode == 3)
                {
                    // Login with embedded web view
                    Console.WriteLine("Start LoginInteraction(useEmbeddedWebView: true)");
                    session = await handler.LoginInteractive(useEmbeddedWebView: true);
                }
                else
                {
                    return;
                }
            }

            Console.WriteLine("Login Success: " + session.Username);
            Console.ReadLine();

            // Logout
            await handler.RemoveAccounts();
        }
    }
}
