using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.Sessions;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.WindowsSecurity;

var directoryPath = MinecraftPath.GetOSDefaultPath();
var path = Path.Combine(directoryPath, "cml_accounts_ws.bin");
var storage = new ProtectedJsonFileStorage(path);
ProtectedJsonFileStorageMigration.TryMigrate(Path.Combine(directoryPath, "cml_accounts.json"), path);
var loginHandler = new JELoginHandlerBuilder()
    .WithAccountManager(
        new JsonXboxGameAccountManager(
            storage,
            JEGameAccount.FromSessionStorage,
            null))
    .Build();

var accounts = loginHandler.AccountManager.GetAccounts();
foreach (var account in accounts)
{
    if (account is JEGameAccount jeAccount)
    {
        Console.WriteLine(jeAccount.Identifier);
        Console.WriteLine(jeAccount.Profile?.Username);
        Console.WriteLine(jeAccount.Profile?.UUID);

        foreach (var skin in jeAccount.Profile?.Skins ?? [])
        {
            Console.WriteLine(skin);
        }
        foreach (var cape in jeAccount.Profile?.Capes ?? [])
        {
            Console.WriteLine(cape);
        }
    }
}

var a = int.Parse(Console.ReadLine());

MSession session;
if (a == 0)
    session = await loginHandler.AuthenticateInteractively();
else
    session = await loginHandler.AuthenticateSilently();

Console.WriteLine(session.Username);
Console.WriteLine(session.UUID);