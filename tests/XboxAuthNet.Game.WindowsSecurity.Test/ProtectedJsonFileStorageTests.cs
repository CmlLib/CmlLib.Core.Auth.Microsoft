using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;
using Xunit;

namespace XboxAuthNet.Game.WindowsSecurity.Test;

public sealed class ProtectedJsonFileStorageTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
    private readonly string _filePath;

    public ProtectedJsonFileStorageTests()
    {
        Directory.CreateDirectory(_directory);
        _filePath = Path.Combine(_directory, "accounts.bin");
    }

    [Fact]
    public void MissingFileReturnsNull()
    {
        Assert.Null(new ProtectedJsonFileStorage(_filePath).ReadAsJsonNode());
        Assert.Empty(Directory.GetFiles(_directory));
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void WriteCreatesDirectoriesAndReplacesEncryptedData()
    {
        var path = Path.Combine(_directory, "nested", "accounts.bin");
        var storage = new ProtectedJsonFileStorage(path);
        storage.Write(new JsonObject { ["token"] = "old-test-token" }, null);
        storage.Write(new JsonObject { ["token"] = "new-test-token" }, null);

        var encrypted = File.ReadAllBytes(path);
        var decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
        Assert.Equal("new-test-token", JsonNode.Parse(decrypted)!["token"]!.GetValue<string>());
        Assert.Equal("new-test-token", storage.ReadAsJsonNode()!["token"]!.GetValue<string>());
        Assert.DoesNotContain("new-test-token", Encoding.UTF8.GetString(encrypted));
        Assert.Single(Directory.GetFiles(Path.GetDirectoryName(path)!));
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void FailedReplacementPreservesOriginalAndCleansTemporaryFile()
    {
        var storage = new ProtectedJsonFileStorage(_filePath);
        storage.Write(new JsonObject { ["token"] = "old-test-token" }, null);
        var original = File.ReadAllBytes(_filePath);

        using (var lockedFile = new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read))
        {
            Assert.Throws<IOException>(() =>
                storage.Write(new JsonObject { ["token"] = "new-test-token" }, null));
        }

        Assert.Equal(original, File.ReadAllBytes(_filePath));
        Assert.Equal("old-test-token", storage.ReadAsJsonNode()!["token"]!.GetValue<string>());
        Assert.Single(Directory.GetFiles(_directory));
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void CorruptCiphertextIsPreservedAndCanBeReplaced()
    {
        AssertRecovery(Array.Empty<byte>());
        AssertRecovery(new byte[] { 1, 2, 3 });

        var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes("{}"), null, DataProtectionScope.CurrentUser);
        AssertRecovery(encrypted[..(encrypted.Length / 2)]);
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void InvalidDecryptedJsonIsPreservedAndCanBeReplaced()
    {
        var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes("{invalid"), null, DataProtectionScope.CurrentUser);
        AssertRecovery(encrypted);
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void RecoveryDoesNotHideSharingErrors()
    {
        File.WriteAllBytes(_filePath, new byte[] { 1, 2, 3 });
        var storage = new ProtectedJsonFileStorage(_filePath);

        using (var lockedFile = new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.None))
        {
            Assert.Throws<IOException>(() => storage.ReadAsJsonNode());
        }

        Assert.Single(Directory.GetFiles(_directory));
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void FailedQuarantineLeavesOriginalAndPropagatesError()
    {
        var original = new byte[] { 1, 2, 3 };
        File.WriteAllBytes(_filePath, original);
        var storage = new ProtectedJsonFileStorage(_filePath);

        using (var lockedFile = new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read))
        {
            Assert.Throws<IOException>(() => storage.ReadAsJsonNode());
        }

        Assert.Equal(original, File.ReadAllBytes(_filePath));
        Assert.Single(Directory.GetFiles(_directory));
    }

    [WindowsFact]
    [SupportedOSPlatform("windows")]
    public void AccountManagerCanSaveNewAccountAfterCorruption()
    {
        File.WriteAllBytes(_filePath, new byte[] { 1, 2, 3 });
        var manager = new JsonXboxGameAccountManager(
            new ProtectedJsonFileStorage(_filePath),
            storage => new TestAccount(storage),
            null);
        Assert.Empty(manager.GetAccounts());

        var account = manager.NewAccount();
        account.SessionStorage.Set("identifier", "test-account");
        manager.SaveAccounts();

        var reloadedManager = new JsonXboxGameAccountManager(
            new ProtectedJsonFileStorage(_filePath),
            storage => new TestAccount(storage),
            null);
        Assert.Equal("test-account", Assert.Single(reloadedManager.GetAccounts()).Identifier);
        Assert.Single(Directory.GetFiles(_directory, "accounts.bin.corrupt.*"));
    }

    [SupportedOSPlatform("windows")]
    private void AssertRecovery(byte[] original)
    {
        File.WriteAllBytes(_filePath, original);
        var storage = new ProtectedJsonFileStorage(_filePath);
        var previousBackups = Directory.GetFiles(_directory, "accounts.bin.corrupt.*");

        Assert.Null(storage.ReadAsJsonNode());
        Assert.False(File.Exists(_filePath));
        var backup = Assert.Single(Directory.GetFiles(_directory, "accounts.bin.corrupt.*").Except(previousBackups));
        Assert.Equal(original, File.ReadAllBytes(backup));
        Assert.Null(storage.ReadAsJsonNode());

        storage.Write(new JsonObject { ["token"] = "test-token" }, null);
        Assert.Equal("test-token", storage.ReadAsJsonNode()!["token"]!.GetValue<string>());
        Assert.Equal(original, File.ReadAllBytes(backup));
    }

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
    }

    private sealed class TestAccount(ISessionStorage storage) : XboxGameAccount(storage)
    {
        protected override string? GetIdentifier()
        {
            return SessionStorage.GetOrDefault<string?>("identifier", null);
        }
    }
}
