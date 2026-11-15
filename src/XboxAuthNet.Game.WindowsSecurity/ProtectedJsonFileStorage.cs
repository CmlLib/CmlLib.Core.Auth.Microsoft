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
        catch (Exception exception) when (exception is CryptographicException || exception is JsonException)
        {
            // 재로그인을 허용하되 복구 가능한 원본은 덮어쓰지 않고 보존한다.
            var backupPath = _filePath + ".corrupt." + Guid.NewGuid().ToString("N");
            File.Move(_filePath, backupPath);
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

        // 같은 볼륨에서 교체해야 저장 실패 시 기존 암호문을 보존할 수 있다.
        var temporaryPath = _filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write))
            {
                stream.Write(encrypted, 0, encrypted.Length);
                stream.Flush(flushToDisk: true);
            }

            if (File.Exists(_filePath))
                File.Replace(temporaryPath, _filePath, null);
            else
                File.Move(temporaryPath, _filePath);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }
}
