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
            // 사용할 수 없는 계정 파일을 삭제해 재로그인을 허용한다.
            File.Delete(_filePath);
            return null;
        }
    }

    /// <summary>
    /// 평문 JSON 파일을 현재 사용자의 암호화 저장소로 옮긴다.
    /// </summary>
    /// <param name="jsonFilePath">마이그레이션할 평문 계정 JSON 파일의 경로.</param>
    /// <returns>이동을 완료하면 true, 원본이 없거나 대상 파일이 이미 있으면 false를 반환한다.</returns>
    /// <remarks>
    /// JSON 구조와 알 수 없는 필드를 유지하며 계정 형식 자체는 변환하지 않는다.
    /// 암호화 파일 저장 후 원본을 삭제한다. 파싱·암호화·파일 작업 오류는 호출자에게 전달한다.
    /// </remarks>
    public bool TryMigrateFromJson(string jsonFilePath)
    {
        var sourcePath = Path.GetFullPath(jsonFilePath);
        var destinationPath = Path.GetFullPath(_filePath);
        if (string.Equals(sourcePath, destinationPath, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Source and destination paths must be different.", nameof(jsonFilePath));

        if (File.Exists(destinationPath))
            return false;

        JsonObject json;
        try
        {
            using var stream = File.OpenRead(sourcePath);
            json = JsonNode.Parse(stream) as JsonObject
                ?? throw new JsonException("The account storage must contain a JSON object.");
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        catch (DirectoryNotFoundException)
        {
            return false;
        }

        Write(json, null, overwrite: false);
        File.Delete(sourcePath);
        return true;
    }

    public void Write(JsonNode node, JsonSerializerOptions? serializerOptions)
    {
        Write(node, serializerOptions, overwrite: true);
    }

    private void Write(JsonNode node, JsonSerializerOptions? serializerOptions, bool overwrite)
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

            if (overwrite && File.Exists(_filePath))
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
