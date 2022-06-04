using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft.Mojang
{
    public class MojangXboxLoginResponse
    {
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("roles")]
        public string[]? Roles { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("expires_on")]
        public DateTime ExpiresOn { get; set; }
        
        [JsonPropertyName("errorType")]
        public string? ErrorType { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
