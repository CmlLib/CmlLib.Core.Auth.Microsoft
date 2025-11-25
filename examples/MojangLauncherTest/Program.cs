using CmlLib.Core;
using CmlLib.Core.Auth.Microsoft.MojangLauncher;

var accounts = MojangLauncherAccounts.Load(MinecraftPath.GetOSDefaultPath());
accounts.GetSessions();