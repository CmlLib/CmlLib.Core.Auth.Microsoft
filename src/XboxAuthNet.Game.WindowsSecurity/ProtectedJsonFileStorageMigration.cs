using System.Text.Json;
using System.Text.Json.Nodes;

namespace XboxAuthNet.Game.WindowsSecurity;

public static class ProtectedJsonFileStorageMigration
{
    /// <summary>
    /// 평문 JSON 파일을 현재 사용자의 암호화 저장소로 옮긴다.
    /// </summary>
    /// <param name="jsonFilePath">마이그레이션할 평문 계정 JSON 파일의 경로.</param>
    /// <param name="protectedFilePath">암호화된 계정 파일을 생성할 경로.</param>
    /// <returns>이동을 완료하면 true, 원본이 없거나 대상 파일이 이미 있으면 false를 반환한다.</returns>
    /// <remarks>
    /// JSON 구조와 알 수 없는 필드를 유지하며 계정 형식 자체는 변환하지 않는다.
    /// 암호화 파일 저장 후 원본을 삭제한다. 파싱·암호화·파일 작업 오류는 호출자에게 전달한다.
    /// </remarks>
    public static bool TryMigrate(string jsonFilePath, string protectedFilePath)
    {
        var sourcePath = Path.GetFullPath(jsonFilePath);
        var destinationPath = Path.GetFullPath(protectedFilePath);
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

        new ProtectedJsonFileStorage(destinationPath).WriteNew(json);
        File.Delete(sourcePath);
        return true;
    }
}
