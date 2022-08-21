using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Bedrock.Auth.Models;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockSessionCache : SessionCacheBase
    {
        [JsonPropertyName("bedrockTokens")]
        public BedrockToken[]? BedrockTokens { get; set; }

        public override bool CheckValidation()
        {
            if (!base.CheckValidation())
                return false;

            // TODO: check jwt expiration

            return BedrockTokens != null && BedrockTokens.Length > 0;
        }
    }
}
