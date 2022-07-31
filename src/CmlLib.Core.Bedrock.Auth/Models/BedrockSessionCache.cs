using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Bedrock.Auth.Models;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockSessionCache : SessionCacheBase
    {
        [JsonPropertyName("bedrockTokens")]
        public BedrockToken[]? BedrockTokens { get; set; }
    }
}
