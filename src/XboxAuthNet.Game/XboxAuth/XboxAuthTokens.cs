using System.Text.Json.Serialization;
using XboxAuthNet.XboxLive.Responses;

namespace XboxAuthNet.Game.XboxAuth;

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

    public bool Validate() => XstsToken?.Validate() ?? false;
}