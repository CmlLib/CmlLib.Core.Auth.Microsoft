using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Text.Json;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Msal;
using XboxAuthNet.Game.Msal.OAuth;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.XboxLive;

// logger
var loggerFactory = LoggerFactory.Create(config =>
{
    config.ClearProviders();
    config.AddSimpleConsole();
    config.SetMinimumLevel(LogLevel.Debug);
});
var logger = loggerFactory.CreateLogger("CmlLib.Core");

// initialize MSAL
var app = await MsalClientHelper.BuildApplicationWithCache("499c8d36-be2a-4231-9ebd-ef291b7bb64c");

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
        if (account is JEGameAccount jeAccount)
        {
            Console.WriteLine($"[{number}] {account.Identifier}");
            Console.WriteLine($"    LastAccess: {jeAccount.LastAccess}");
            Console.WriteLine($"    Username: {jeAccount.Profile?.Username}");
            Console.WriteLine($"    UUID: {jeAccount.Profile?.UUID}");

            var skins = jeAccount.Profile?.Skins ?? [];
            var skinIndex = 0;
            foreach (var skin in skins)
            {
                Console.WriteLine($"    Skins[{skinIndex}]");
                Console.WriteLine($"        Id: {skin.Id}");
                Console.WriteLine($"        State: {skin.State}");
                Console.WriteLine($"        TextureKey: {skin.TextureKey}");
                Console.WriteLine($"        Url: {skin.Url}");
                skinIndex++;
            }

            var capes = jeAccount.Profile?.Capes ?? [];
            var capeIndex = 0;
            foreach (var cape in capes)
            {
                Console.WriteLine($"    Capes[{capeIndex}]");
                Console.WriteLine($"        Id: {cape.Id}");
                Console.WriteLine($"        State: {cape.State}");
                Console.WriteLine($"        Alias: {cape.Alias}");
                Console.WriteLine($"        Url: {cape.Url}");
                skinIndex++;
            }
        }
        else
        {
            Console.WriteLine($"[{number}] {account.Identifier} (NOT JEGameAccount)");
        }
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
        "\n" +
        "<OAuth, WINDOWS only>\n" +
        "[1] Default\n" +
        "[2] Interactive\n" +
        "[3] Silent\n" +
        "[4] CodeFlow + XboxSisuAuth\n" +
        "\n" +
        "<MSAL, Universal>\n" +
        "[5] Interactive\n" +
        "[6] Silent\n" +
        "[7] DeviceCode\n" +
        "\n" +
        "<Signout>\n" +
        "[8] Remove Account\n" +
        "[9] Signout (OAuth, WINDOWS only)\n" +
        "\n" +
        "<Debug>\n" +
        "[10] Show token informations");
    Console.Write("\n\nNumber: ");
    var selectedAuthMode = int.Parse(Console.ReadLine() ?? "1");

    // authentication
    Console.WriteLine("Start Authentication...");
    MSession session;
    switch (selectedAuthMode)
    {
        case 1: // default
            checkWindows();
            session = await loginHandler.Authenticate(selectedAccount);
            break;
        case 2: // interactive (OAuth)
            checkWindows();
            session = await loginHandler.AuthenticateInteractively(selectedAccount);
            break;
        case 3: // silent (OAuth)
            checkWindows();
            session = await loginHandler.AuthenticateSilently(selectedAccount);
            break;
        case 4: // CodeFlow + XboxSisuAuth
            {
                checkWindows();
                var authenticator = loginHandler.CreateAuthenticator(selectedAccount, default);
                authenticator.AddMicrosoftOAuthForJE(oauth => oauth.CodeFlow());
                authenticator.AddXboxAuthForJE(xbox => xbox.Sisu(XboxGameTitles.MinecraftJava));
                authenticator.AddJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }
        case 5: // Interactive (Msal)
            {
                var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
                authenticator.AddMsalOAuth(app, msal => msal.Interactive());
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddForceJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }
        case 6: // Silent (Msal)
            {
                var authenticator = loginHandler.CreateAuthenticator(selectedAccount, default);
                authenticator.AddMsalOAuth(app, msal => msal.Silent());
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }
        case 7: // DeviceCode (Msal)
            {
                var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
                authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(code =>
                {
                    Console.WriteLine(code.Message);
                    return Task.CompletedTask;
                }));
                authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
                authenticator.AddJEAuthenticator();
                session = await authenticator.ExecuteForLauncherAsync();
                break;
            }
        case 8: // Remove account
            {
                await loginHandler.Signout(selectedAccount);
                continue;
            }
        case 9: // Signout with OAuth signout page
            {
                checkWindows();
                await loginHandler.SignoutWithBrowser(selectedAccount);
                continue;
            }
        case 10: // Show entire session
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
        case 20: // hidden menu for testing
            {
                var authenticator = loginHandler.CreateAuthenticator(selectedAccount, default);
                authenticator.AddForceMicrosoftOAuthForJE(oauth => oauth.CodeFlow());
                //authenticator.AddMsalOAuth(getApp(), msal => msal.InteractiveWithSingleAccount());
                authenticator.AddXboxAuthForJE(xbox => xbox.Sisu(JELoginHandler.DefaultMicrosoftOAuthClientInfo.ClientId));
                await authenticator.ExecuteAsync();
                continue;
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

void checkWindows()
{
#if WINDOWS
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return;    
#endif

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("This authentication mode is only available on Windows. The TargetFramework in the csproj file must be set to the Windows version. (e.g. net8-windows)");
    Console.WriteLine("Press Enter to continue:");
    Console.ReadLine();
    Console.ResetColor();
}