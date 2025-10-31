using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.Accounts.JsonStorage;

public class JsonFileStorage : IJsonStorage
{
    private readonly string _filePath;

    public JsonFileStorage(string filePath)
    {
        _filePath = filePath;
    }

    public JsonNode? ReadAsJsonNode()
    {
        if (!File.Exists(_filePath))
            return null;

        try
        {
            using var fs = File.OpenRead(_filePath);
            return JsonNode.Parse(fs);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public void Write(JsonNode node, JsonSerializerOptions? serializerOptions)
    {
        var dirPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dirPath))
            Directory.CreateDirectory(dirPath);

        using var fs = File.Create(_filePath);
        using var writer = new Utf8JsonWriter(fs);
        node.WriteTo(writer, serializerOptions);
    }
}
