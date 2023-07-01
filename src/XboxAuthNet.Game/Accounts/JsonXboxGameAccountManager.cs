using System.Text.Json;
using System.Text.Json.Nodes;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Accounts;

public class JsonXboxGameAccountManager : IXboxGameAccountManager
{
    public static readonly JsonSerializerOptions DefaultSerializerOption = new JsonSerializerOptions
    {
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly string _filePath;
    private Func<ISessionStorage, IXboxGameAccount> _converter;
    private readonly JsonSerializerOptions? _jsonOptions;
    private XboxGameAccountCollection _accounts = new();
    private bool isLoaded = false;

    public JsonXboxGameAccountManager(string filePath) : 
        this(filePath, XboxGameAccount.FromSessionStorage, DefaultSerializerOption)
    {

    }

    public JsonXboxGameAccountManager(
        string filePath,
        Func<ISessionStorage, IXboxGameAccount> converter,
        JsonSerializerOptions? jsonOptions)
    {
        this._filePath = filePath;
        this._converter = converter;
        this._jsonOptions = jsonOptions;
    }

    public XboxGameAccountCollection GetAccounts()
    {
        if (!isLoaded)
        {
            var node = readAsJson();
            loadFromJson(node);
            isLoaded = true;
        }
        return _accounts;
    }

    private void loadFromJson(JsonNode? node)
    {
        _accounts.Clear();
        var accounts = parseAccounts(node);
        foreach (var account in accounts)
        {
            _accounts.Add(account);
        }
    }

    private JsonNode? readAsJson()
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

    private IEnumerable<IXboxGameAccount> parseAccounts(JsonNode? node)
    {
        var rootObject = node as JsonObject;
        if (rootObject == null)
            yield break;

        foreach (var kv in rootObject)
        {
            var innerObject = kv.Value as JsonObject;
            if (innerObject == null)
                continue;

            var sessionStorage = new JsonSessionStorage(innerObject, _jsonOptions);
            var account = convertSessionStorageToAccount(sessionStorage);
            if (!string.IsNullOrEmpty(account.Identifier))
                yield return account;
        }
    }

    public IXboxGameAccount GetDefaultAccount()
    {
        var first = GetAccounts().FirstOrDefault();
        if (first != null)
            return first;
        else
            return NewAccount();
    }

    public IXboxGameAccount NewAccount()
    {
        var sessionStorage = JsonSessionStorage.CreateEmpty(_jsonOptions);
        var account = convertSessionStorageToAccount(sessionStorage);
        GetAccounts().Add(account);
        return account;
    }

    public void ClearAccounts()
    {
        GetAccounts().Clear();
        SaveAccounts();
    }

    public void SaveAccounts()
    {
        var json = serializeToJson();

        var dirPath = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(dirPath))
            Directory.CreateDirectory(dirPath);

        using var fs = File.Create(_filePath);
        using var writer = new Utf8JsonWriter(fs);
        json.WriteTo(writer, _jsonOptions);

        loadFromJson(json); // reload
    }

    private JsonNode serializeToJson()
    {
        var rootObject = new JsonObject();
        foreach (var account in GetAccounts())
        {
            var identifier = account.Identifier;
            if (string.IsNullOrEmpty(identifier))
                continue;

            var jsonSessionStorage = account.SessionStorage as JsonSessionStorage;
            if (jsonSessionStorage == null)
                continue;

            rootObject.Add(identifier, jsonSessionStorage.ToJsonObjectForStoring());
        }
        return rootObject;
    }

    private IXboxGameAccount convertSessionStorageToAccount(ISessionStorage sessionStorage)
    {
        return _converter.Invoke(sessionStorage);
    }
}