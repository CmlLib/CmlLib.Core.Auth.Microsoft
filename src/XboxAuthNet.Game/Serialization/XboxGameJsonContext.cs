using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive.Responses;

namespace XboxAuthNet.Game.Serialization;

// 상위 게임 인증 모듈이 자체 모델의 메타데이터와 조합할 수 있도록 공개한다.
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(XboxAuthTokens))]
[JsonSerializable(typeof(XboxAuthResponse))]
[JsonSerializable(typeof(XboxAuthXuiClaims))]
[JsonSerializable(typeof(MicrosoftOAuthResponse))]
[JsonSerializable(typeof(MicrosoftUserPayload))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(JsonElement))]
[JsonSerializable(typeof(JsonNode))]
[JsonSerializable(typeof(JsonObject))]
public partial class XboxGameJsonContext : JsonSerializerContext
{
}
