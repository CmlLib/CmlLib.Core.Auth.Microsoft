using System.Text.Json;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.MojangLauncher;

public record MojangLauncherAccount(
    [property:JsonPropertyName("accessToken")] string? AccessToken,
    [property:JsonPropertyName("accessTokenExpiresAt")] string? AccessTokenExpiresAt,
    [property:JsonPropertyName("avatar")] string? Avatar,
    [property:JsonPropertyName("localId")] string? LocalId,
    [property:JsonPropertyName("minecraftProfile")] MojangLauncherMinecraftProfile? MinecraftProfile,
    [property:JsonPropertyName("remoteId")] string? RemoteId,
    [property:JsonPropertyName("type")] string? Type,
    [property:JsonPropertyName("username")] string? Username)
{
    public static IReadOnlyList<MojangLauncherAccount> ReadFile(string path)
    {
        using var accountsFile = File.OpenRead(path);
        using var accountsDoc = JsonDocument.Parse(accountsFile);
        return accountsDoc.RootElement
            .GetProperty("accounts")
            .EnumerateObject()
            .Select(item => item.Value.Deserialize<MojangLauncherAccount>())
            .Where(item => item != null)
            .ToList()!;
    }
}

public record MojangLauncherMinecraftProfile(
    [property:JsonPropertyName("id")]   string? Id,
    [property:JsonPropertyName("name")] string? Name
);