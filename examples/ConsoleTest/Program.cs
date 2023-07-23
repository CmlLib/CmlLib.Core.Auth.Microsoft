using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using Microsoft.Extensions.Logging;
using XboxAuthNet.Game.Msal;
using XboxAuthNet.Game.Accounts;
using Microsoft.Identity.Client;
using XboxAuthNet.Game;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.XboxLive;
using XboxAuthNet.Game.SessionStorages;
using System.Text.Json;
using XboxAuthNet.OAuth.CodeFlow;
using XboxAuthNet.OAuth.CodeFlow.Parameters;

// logger
var loggerFactory = LoggerFactory.Create(config => 
{
    config.ClearProviders();
    config.AddSimpleConsole();
    config.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
});
var logger = loggerFactory.CreateLogger("CmlLib.Core");

// initialize MSAL
bool useMsal = true;
IPublicClientApplication? app = null;
if (useMsal)
    app = await MsalClientHelper.BuildApplicationWithCache("499c8d36-be2a-4231-9ebd-ef291b7bb64c");
IPublicClientApplication getApp() =>
    app ?? throw new InvalidOperationException("MSAL was not initialized yet. Set useMsal = true;");

// initialize loginHandler
var loginHandler = new JELoginHandlerBuilder()
    .WithLogger(logger)
    .Build();

while (true)
{
    // list accounts
    Console.WriteLine();
    Console.WriteLine("Select account to login: ");
    Console.WriteLine("[0] New Account");
    var accounts = loginHandler.AccountManager.GetAccounts();
    var number = 1;
    foreach (var account in accounts)
    {
        if (account is not JEGameAccount jeAccount)
            continue;
        
        Console.WriteLine($"[{number}] {account.Identifier}");
        Console.WriteLine($"    LastAccess: {jeAccount.LastAccess}");
        Console.WriteLine($"    Username: {jeAccount.Profile?.Username}");
        Console.WriteLine($"    UUID: {jeAccount.Profile?.UUID}");
        number++;
    }

    // find account
    Console.Write("\nNumber: ");
    var selectedAccountNumber = int.Parse(Console.ReadLine() ?? "0");
    IXboxGameAccount selectedAccount;
    if (selectedAccountNumber == 0)
        selectedAccount = loginHandler.AccountManager.NewAccount();
    else
        selectedAccount = accounts.ElementAt(selectedAccountNumber - 1);
    Console.WriteLine();

    // authentication mode
    Console.WriteLine(
        "Select Authentication mode: \n" +
        "[1] Default\n" +
        "[2] Interactive (OAuth)\n" +
        "[3] Silent (OAuth)\n" +
        "[4] Interactive (Msal)\n" +
        "[5] Silent (Msal)\n" +
        "[6] DeviceCode (Msal)\n" +
        "[7] Signout without OAuth signout page\n" +
        "[8] Signout with OAuth signout page\n" +
        "[9] Show token informations");
    Console.Write("\n\nNumber: ");
    var selectedAuthMode = int.Parse(Console.ReadLine() ?? "1");

    // authentication
    Console.WriteLine("Start Authentication...");
    MSession session;
    switch (selectedAuthMode)
    {
        case 1: // default
            session = await loginHandler.Authenticate(selectedAccount);
            break;
        case 2: // interactive (OAuth)
            session = await loginHandler.AuthenticateInteractively(selectedAccount);
            break;
        case 3: // silent (OAuth)
            session = await loginHandler.AuthenticateSilently(selectedAccount);
            break;
        case 4: // Interactive (Msal)
            {
                var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
                authenticator.AddMsalOAuth(getApp(), msal => msal.Interactive());
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddForceJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }

        case 5: // Silent (Msal)
            {
                var authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount();
                authenticator.AddMsalOAuth(getApp(), msal => msal.Silent());
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }

        case 6: // DeviceCode (Msal)
            {
                var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
                authenticator.AddMsalOAuth(getApp(), msal => msal.DeviceCode(code =>
                {
                    Console.WriteLine(code.Message);
                    return Task.CompletedTask;
                }));
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }
        case 7: // Signout without OAuth signout page 
            {
                await loginHandler.Signout(selectedAccount);
                continue;
            }
        case 8: // Signout with OAuth signout page
            {
                var authenticator = loginHandler.CreateAuthenticator(selectedAccount, default);
                authenticator.AddMicrosoftOAuthBrowserSignout(JELoginHandler.DefaultMicrosoftOAuthClientInfo);
                await authenticator.ExecuteAsync();
                continue;
            }
        case 9: // Show entire session
            {
                var ss = selectedAccount.SessionStorage;
                if (ss is JsonSessionStorage jsonSessionStorage)
                {
                    var jsonObject = jsonSessionStorage.ToJsonObject();
                    var json = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    Console.WriteLine(json);
                }
                continue;
            }
        case 10:
            {
                var authenticator = loginHandler.CreateAuthenticator(selectedAccount, default);
                authenticator.AddForceMicrosoftOAuthForJE(oauth => oauth.CodeFlow());
                //authenticator.AddMsalOAuth(getApp(), msal => msal.InteractiveWithSingleAccount());
                await authenticator.ExecuteAsync();
                return;
            }
        default:
            Console.WriteLine("Wrong authentication mode");
            continue;
    }

    // show result
    Console.WriteLine(
        "Login result: \n" +
        $"Username: {session.Username}\n" +
        $"UUID: {session.UUID}");
    Console.WriteLine("Done");
}
