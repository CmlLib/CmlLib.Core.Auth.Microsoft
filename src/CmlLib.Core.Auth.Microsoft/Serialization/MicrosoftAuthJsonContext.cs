using System.Text.Json.Serialization;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft.Serialization;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(JEToken))]
[JsonSerializable(typeof(JEProfile))]
[JsonSerializable(typeof(JEProfileSkin))]
[JsonSerializable(typeof(JEProfileCape))]
[JsonSerializable(typeof(MojangXboxAccessTokenPayload))]
public partial class MicrosoftAuthJsonContext : JsonSerializerContext
{
}
