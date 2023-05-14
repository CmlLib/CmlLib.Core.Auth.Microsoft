using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using XboxAuthNet.Game.SessionStorages;
using System.Linq;

namespace XboxAuthNet.Game.Accounts;

public class JsonXboxGameAccountManager : IXboxGameAccountManager
{
    private readonly string _filePath;
    private Func<ISessionStorage, IXboxGameAccount> _converter;
    private readonly JsonSerializerOptions? _jsonOptions;

    public JsonXboxGameAccountManager(
        string filePath,
        Func<ISessionStorage, IXboxGameAccount> converter,
        JsonSerializerOptions? jsonOptions = default)
    {
        this._filePath = filePath;
        this._converter = converter;
        this._jsonOptions = jsonOptions;
        Accounts = new();
    }

    public XboxGameAccountCollection Accounts { get; private set; }

    public void Load()
    {
        var node = readAsJson();
        loadFromJson(node);
    }

    private void loadFromJson(JsonNode? node)
    {
        Accounts.Clear();
        var accounts = parseAccounts(node);
        foreach (var account in accounts)
        {
            Accounts.Add(account);
        }
    }

    private JsonNode? readAsJson()
    {
        if (!File.Exists(_filePath))
            return null;
        using var fs = File.OpenRead(_filePath);
        return JsonNode.Parse(fs);
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
        var first = Accounts.FirstOrDefault();
        if (first != null)
            return first;
        else
            return NewAccount();
    }

    public IXboxGameAccount NewAccount()
    {
        var sessionStorage = JsonSessionStorage.CreateEmpty(_jsonOptions);
        var account = convertSessionStorageToAccount(sessionStorage);
        Accounts.Add(account);
        return account;
    }

    public void SaveAccounts()
    {
        var json = serializeToJson();

        using var fs = File.Create(_filePath);
        using var writer = new Utf8JsonWriter(fs);
        json.WriteTo(writer, _jsonOptions);

        loadFromJson(json); // reload
    }

    private JsonNode serializeToJson()
    {
        var rootObject = new JsonObject();
        foreach (var account in Accounts)
        {
            var identifier = account.Identifier;
            if (string.IsNullOrEmpty(identifier))
                continue;

            var jsonSessionStorage = account.SessionStorage as JsonSessionStorage;
            if (jsonSessionStorage == null)
                continue;

            rootObject.Add(identifier, jsonSessionStorage.ToJsonObject());
        }
        return rootObject;
    }

    private IXboxGameAccount convertSessionStorageToAccount(ISessionStorage sessionStorage)
    {
        return _converter.Invoke(sessionStorage);
    }
}