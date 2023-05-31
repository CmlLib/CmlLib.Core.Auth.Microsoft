using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.Sessions;

public class MojangXboxAccessTokenPayload
{
    [JsonPropertyName("xuid")]
    public string? Xuid { get; set; }

    [JsonPropertyName("agg")]
    public string? Agg { get; set; }

    [JsonPropertyName("sub")]
    public string? Sub { get; set; }

    [JsonPropertyName("nbf")]
    public long Nbf { get; set; }

    [JsonPropertyName("auth")]
    public string? Auth { get; set; }

    [JsonPropertyName("iss")]
    public string? Iss { get; set; }

    [JsonPropertyName("exp")]
    public long Exp { get; set; }

    [JsonPropertyName("iat")]
    public long Iat { get; set; }

    [JsonPropertyName("platform")]
    public string? Platform { get; set; }

    [JsonPropertyName("yuid")]
    public string? Yuid { get; set; }
}