using System.Text.Json.Serialization;

namespace XboxAuthNet.Game.Test;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(MockObject))]
[JsonSerializable(typeof(InnerMockObject))]
internal partial class TestJsonContext : JsonSerializerContext
{
}
