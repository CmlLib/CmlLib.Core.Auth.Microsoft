using System.Text.Json;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.MojangLauncher;

public record MsaRetailTokens(
    string Xuid,
    IReadOnlyCollection<MsaRetailToken> Tokens
);

public class MsaRetailToken
{
    [property: JsonPropertyName("IdentityType")]
    public string? IdentityType { get; set; }

    [property: JsonPropertyName("Sandbox")]
    public string? Sandbox { get; set; }

    [property: JsonPropertyName("TokenType")]
    public string? TokenType { get; set; }

    [property: JsonPropertyName("RelyingParty")]
    public string? RelyingParty { get; set; }

    [property: JsonPropertyName("TokenData")]
    public MsaRetailTokenData TokenData { get; set; } = new();
}

public class MsaRetailTokenData
{
    [property: JsonPropertyName("Token")]
    public string? Token { get; set; }

    [property: JsonPropertyName("NotAfter")]
    public string? NotAfter { get; set; }

    [property: JsonPropertyName("IssueInstant")]
    public string? IssueInstant { get; set; }

    [property: JsonIgnore]
    public string? UserHash { get; set; }
}

public class MsaParser
{
    public static List<MsaRetailTokens> ParseFromRoot(JsonElement root)
    {
        return root
            .GetProperty("credentials")
            .EnumerateObject()
            .Select(item =>
            {
                var xuid = item.Name;
                var tokens = item.Value.EnumerateObject()
                    .Where(item => item.Name.Contains("RETAIL"))
                    .SelectMany(item => ParseRetailToken(item.Value.ToString()))
                    .ToList();
                return new MsaRetailTokens(xuid, tokens);
            })
            .ToList();
    }

    public static List<MsaRetailToken> ParseRetailToken(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return ParseRetailToken(doc.RootElement);
    }

    public static List<MsaRetailToken> ParseRetailToken(JsonElement root)
    {
        try
        {
            return root
                .GetProperty("tokens")
                .EnumerateArray()
                .Select(element =>
                {
                    var token = element.Deserialize<MsaRetailToken>();
                    if (token == null)
                        return null;

                    try
                    {
                        token.TokenData.UserHash = GetUserHash(element);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    return token;
                })
                .Where(token => token != null)
                .ToList()!;
        }
        catch
        {
            return [];
        }
    }

    private static string GetUserHash(JsonElement token)
    {
        return token
            .GetProperty("TokenData")
            .GetProperty("DisplayClaims")
            .GetProperty("xui")
            .EnumerateArray()
            .First()
            .GetProperty("uhs")
            .ToString();
    }
}