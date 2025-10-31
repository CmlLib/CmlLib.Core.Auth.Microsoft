using System.Text.Json;
using System.Text.Json.Nodes;
using XboxAuthNet.Game.Accounts.JsonStorage;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Accounts;

public class JsonXboxGameAccountManager : IXboxGameAccountManager
{
    public static readonly JsonSerializerOptions DefaultSerializerOption = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly IJsonStorage _jsonStorage;
    private readonly Func<ISessionStorage, IXboxGameAccount> _converter;
    private readonly JsonSerializerOptions? _jsonOptions;
    private readonly List<IXboxGameAccount> _accounts = [];

    private bool isLoaded;

    public JsonXboxGameAccountManager(string filePath) : 
        this(filePath, XboxGameAccount.FromSessionStorage, DefaultSerializerOption)
    {

    }

    public JsonXboxGameAccountManager(
        string filePath,
        Func<ISessionStorage, IXboxGameAccount> converter,
        JsonSerializerOptions? jsonOptions)
    {
        this._jsonStorage = new JsonFileStorage(filePath);
        this._converter = converter;
        this._jsonOptions = jsonOptions;
    }

    public JsonXboxGameAccountManager(
        IJsonStorage storage,
        Func<ISessionStorage, IXboxGameAccount> converter,
        JsonSerializerOptions? jsonOptions)
    {
        this._jsonStorage = storage;
        this._converter = converter;
        this._jsonOptions = jsonOptions;
    }

    public XboxGameAccountCollection GetAccounts()
    {
        if (!isLoaded)
        {
            var node = _jsonStorage.ReadAsJsonNode();
            loadFromJson(node);
            isLoaded = true;
        }

        return XboxGameAccountCollection.FromAccounts(_accounts);
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
        _accounts.Add(account);
        return account;
    }

    public void ClearAccounts()
    {
        _accounts.Clear();
        SaveAccounts();
    }

    public void SaveAccounts()
    {
        var json = serializeToJson();
        _jsonStorage.Write(json, _jsonOptions);
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