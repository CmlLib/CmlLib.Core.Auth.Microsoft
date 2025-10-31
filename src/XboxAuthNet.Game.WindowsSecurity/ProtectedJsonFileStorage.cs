using System.Text.Json;
using System.Text.Json.Nodes;
using System.Security.Cryptography;
using XboxAuthNet.Game.Accounts.JsonStorage;

namespace XboxAuthNet.Game.WindowsSecurity;

public class ProtectedJsonFileStorage : IJsonStorage
{
    private readonly string _filePath;

    public ProtectedJsonFileStorage(string filePath)
    {
        _filePath = filePath;
    }

    public JsonNode? ReadAsJsonNode()
    {
        if (!File.Exists(_filePath))
            return null;

        try
        {
            var encrypted = File.ReadAllBytes(_filePath);
            var decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
            return JsonNode.Parse(decrypted);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public void Write(JsonNode node, JsonSerializerOptions? serializerOptions)
    {
        using var ms = new MemoryStream();
        JsonSerializer.Serialize(ms, node, serializerOptions);
        var encrypted = ProtectedData.Protect(ms.ToArray(), null, DataProtectionScope.CurrentUser);

        var dirPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dirPath))
            Directory.CreateDirectory(dirPath);
        File.WriteAllBytes(_filePath, encrypted);
    }
}
