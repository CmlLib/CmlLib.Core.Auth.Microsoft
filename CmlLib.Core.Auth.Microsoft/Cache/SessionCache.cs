using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using System.Text.Json.Serialization;
using CmlLib.Core.Auth.Microsoft.Mojang;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class SessionCache
    {
        [JsonPropertyName("microsoftOAuthSession")]
        public MicrosoftOAuthResponse? MicrosoftOAuthToken { get; set; }

        [JsonPropertyName("xstsSession")]
        public XboxAuthResponse? XstsToken { get; set; }

        [JsonPropertyName("xboxSession")] // to keep backwards compatibility, it keeps old name
        public MojangXboxLoginResponse? MojangXboxToken { get; set; }

        [JsonPropertyName("gameSession")]
        public MSession? GameSession { get; set; }
    }
}
