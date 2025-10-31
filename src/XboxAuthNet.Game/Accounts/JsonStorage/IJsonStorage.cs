using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.Accounts.JsonStorage;

public interface IJsonStorage
{
    JsonNode? ReadAsJsonNode();
    void Write(JsonNode node, JsonSerializerOptions? serializerOptions);
}
