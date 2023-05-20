using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.Sessions
{
    public class JEProfile
    {
        [JsonPropertyName("id")]
        public string? UUID { get; set; }

        [JsonPropertyName("name")]
        public string? Username { get; set; }

        [JsonPropertyName("skins")]
        public object? Skins { get; set; }

        [JsonPropertyName("capes")]
        public object? Capes { get; set; }
    }
}