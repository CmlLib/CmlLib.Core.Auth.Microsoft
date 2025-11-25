namespace CmlLib.Core.Auth.Microsoft.MojangLauncher;

public class MojangLauncherAccounts
{
    public static MojangLauncherAccounts Load(string path)
    {
        var accountsPath = Path.Combine(path, "launcher_accounts.json");
        var accounts = MojangLauncherAccount.ReadFile(accountsPath);

        var msaCredentialsPath = Path.Combine(path, "launcher_msa_credentials.bin");
        var msaCredentials = MojangLauncherMsaCredentials.ReadEncryptedFile(msaCredentialsPath);

        return new MojangLauncherAccounts(accounts, msaCredentials);
    }

    private readonly IReadOnlyList<MojangLauncherAccount> _accounts;
    private readonly MojangLauncherMsaCredentials _msaCredentials;

    private MojangLauncherAccounts(IReadOnlyList<MojangLauncherAccount> accounts, MojangLauncherMsaCredentials msaCredentials)
    {
        _accounts = accounts;
        _msaCredentials = msaCredentials;
    }

    public void GetSessions()
    {
        foreach (var account in _accounts)
        {
            Console.WriteLine(account.AccessToken);
            Console.WriteLine(account.AccessTokenExpiresAt);
            Console.WriteLine(account.Avatar?.Length);
            Console.WriteLine(account.LocalId);
            Console.WriteLine(account.MinecraftProfile?.Id);
            Console.WriteLine(account.MinecraftProfile?.Name);
            Console.WriteLine(account.RemoteId);
            Console.WriteLine(account.Type);
            Console.WriteLine(account.Username);

            var tokens = _msaCredentials
                .Tokens
                .Where(tokens => tokens.Xuid == account.RemoteId)
                .SelectMany(tokens => tokens.Tokens)
                .Where(token => token.RelyingParty == "rp://api.minecraftservices.com");
            
            foreach (var token in tokens)
            {
                Console.WriteLine(token.IdentityType);
                Console.WriteLine(token.RelyingParty);
                Console.WriteLine(token.Sandbox);
                Console.WriteLine(token.TokenData.IssueInstant);
                Console.WriteLine(token.TokenData.NotAfter);
                Console.WriteLine(token.TokenData.Token);
                Console.WriteLine(token.TokenData.UserHash);
                Console.WriteLine(token.TokenType);
            }
        }
    }
}