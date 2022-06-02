using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using System.Text.Json.Serialization;

namespace CmlLib.Core.Auth.Microsoft
{
    public class SessionCache
    {
        [JsonPropertyName("microsoftOAuthSession")]
        public MicrosoftOAuthResponse? MicrosoftOAuthSession { get; set; }

        [JsonPropertyName("xboxAuthSession")]
        public XboxAuthResponse? XboxAuthSession { get; set; }

        [JsonPropertyName("xboxSession")] // to keep backwards compatibility, it keeps old name
        public MojangXboxLoginResponse? MojangSession { get; set; }

        [JsonPropertyName("gameSession")]
        public MSession? GameSession { get; set; }
    }
}
