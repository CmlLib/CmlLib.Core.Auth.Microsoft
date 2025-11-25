using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft.MojangLauncher;

public class MojangLauncherMsaCredentials
{
    public static MojangLauncherMsaCredentials ReadEncryptedFile(string path)
    {
        var fileContent = File.ReadAllBytes(path);
        var decryptedContent = ProtectedData.Unprotect(fileContent, null, DataProtectionScope.CurrentUser);
        var json = Encoding.UTF8.GetString(decryptedContent);
        using var doc = JsonDocument.Parse(json);

        var activeUserXuid = doc.RootElement.GetProperty("activeUserXuid").GetString();
        var tokens = MsaParser.ParseFromRoot(doc.RootElement);
        return new MojangLauncherMsaCredentials
        {
            ActiveUserXuid = activeUserXuid,
            Tokens = tokens
        };
    }

    public string? ActiveUserXuid { get; set; }
    public List<MsaRetailTokens> Tokens { get; set; } = [];

    public IEnumerable<MsaRetailToken> FindTokens(string xuid, string relyingParty)
    {
        return Tokens
            .Where(tokens => tokens.Xuid == xuid)
            .SelectMany(tokens => tokens.Tokens)
            .Where(token => token.RelyingParty == relyingParty);
    }
}