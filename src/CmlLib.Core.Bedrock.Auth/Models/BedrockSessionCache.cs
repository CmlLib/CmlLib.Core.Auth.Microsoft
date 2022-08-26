using CmlLib.Core.Auth.Microsoft.Cache;
using CmlLib.Core.Bedrock.Auth.Models;
using System.Text.Json.Serialization;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockSessionCache : SessionCacheBase
    {
        /// <summary>
        /// XSTS tokens, issued by xsts.auth.xboxlive.com
        /// </summary>
        [JsonPropertyName("xstsSession")]
        public XboxAuthResponse? XstsToken { get; set; }

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
