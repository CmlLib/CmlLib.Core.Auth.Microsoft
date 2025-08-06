using System.Text.Json;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.Sessions;

public class JEProfile
{
    [JsonPropertyName("id")]
    public string? UUID { get; set; }

    [JsonPropertyName("name")]
    public string? Username { get; set; }

    [JsonPropertyName("skins")]
    public IReadOnlyCollection<JEProfileSkin> Skins { get; set; } = [];

    [JsonPropertyName("capes")]
    public IReadOnlyCollection<JEProfileCape> Capes { get; set; } = [];

    public static JEProfile ParseFromJson(JsonElement root)
    {
        var profile = new JEProfile();

        if (root.TryGetProperty("id", out var idProperty))
            profile.UUID = idProperty.ToString();

        if (root.TryGetProperty("name", out var nameProperty))
            profile.Username = nameProperty.ToString();

        // ignore format errors during parsing 'skins' property
        try
        {
            profile.Skins = root
                .GetProperty("skins")
                .EnumerateArray()
                .Select(arr => arr.Deserialize<JEProfileSkin>())
                .Where(skin => skin != null)
                .ToList()!;
        }
        catch
        {
            profile.Skins = [];
        }

        // ignore format errors during parsing 'capes' property
        try
        {
            profile.Capes = root
                .GetProperty("capes")
                .EnumerateArray()
                .Select(arr => arr.Deserialize<JEProfileCape>())
                .Where(cape => cape != null)
                .ToList()!;
        }
        catch
        {
            profile.Capes = [];
        }

        return profile;
    }
}

public record JEProfileSkin(
    [property:JsonPropertyName("id")] string? Id,
    [property:JsonPropertyName("state")] string? State, // ACTIVE or INACTIVE
    [property:JsonPropertyName("url")] string? Url,
    [property:JsonPropertyName("textureKey")] string? TextureKey,
    [property:JsonPropertyName("variant")] string? Variant);

public record JEProfileCape(
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("state")] string? State, // ACTIVE or INACTIVE
    [property: JsonPropertyName("url")] string? Url,
    [property: JsonPropertyName("alias")] string? Alias);