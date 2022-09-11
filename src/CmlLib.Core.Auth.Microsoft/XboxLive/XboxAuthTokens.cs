using System.Text.Json.Serialization;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.XboxLive
{
    public class XboxAuthTokens
    {
        [JsonPropertyName("deviceToken")]
        public XboxAuthResponse? DeviceToken { get; set; }

        [JsonPropertyName("titleToken")]
        public XboxAuthResponse? TitleToken { get; set; }

        [JsonPropertyName("userToken")]
        public XboxAuthResponse? UserToken { get; set; }

        [JsonPropertyName("xstsToken")]
        public XboxAuthResponse? XstsToken { get; set; }
    }
}
