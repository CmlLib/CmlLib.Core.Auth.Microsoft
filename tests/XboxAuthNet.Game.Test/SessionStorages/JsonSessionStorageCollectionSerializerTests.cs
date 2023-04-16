using System;
using System.Text.Json.Nodes;
using NUnit.Framework;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.SessionStorages;

public class JsonSessionStorageCollectionSerializerTests
{
    private ISessionStorage createMockSessionStorage(string? key)
    {
        var storage = new JsonSessionStorage(new JsonObject());
        storage.Set<string>("key", key);
        return storage;
    }

    private SessionStorageCollection createCollection()
    {
        return new SessionStorageCollection(new MockSessionStorageKeyAssigner());
    }

    [Test]
    public void TestSerialization()
    {
        var collection = createCollection();
        collection.Add(createMockSessionStorage("1"));
        collection.Add(createMockSessionStorage("1"));
        collection.Add(createMockSessionStorage("2"));
        collection.Add(createMockSessionStorage("2"));

        var json = JsonSessionStorageCollectionSerializer.Serialize(collection);
        Console.WriteLine(json);
    }

    [Test]
    public void TestDeserialization()
    {
        var collection = createCollection();
        collection.Add(createMockSessionStorage("1"));
        collection.Add(createMockSessionStorage("1"));
        collection.Add(createMockSessionStorage("2"));
        collection.Add(createMockSessionStorage("2"));

        var json = JsonSessionStorageCollectionSerializer.Serialize(collection);

        collection = JsonSessionStorageCollectionSerializer.Deserialize(json, new MockSessionStorageKeyAssigner());

        foreach (var item in collection)
        {
            Console.WriteLine(item.Key);
            Console.WriteLine(item.Value);
        }
    }

    public void testUsage()
    {
        var accountManager = new JEAccountManager();
        var accounts = accountManager.GetAccounts();

        foreach (var account in accounts)
        {
            // account.SessionStorage
            // account.GameProfile
            // account.GameSession
            // account.Identifier
        }

        var myAccount = accounts.GetAccount("Identifier");
        var result = await loginHandler.AuthenticateSilently()
            .WithSessionStorage(myAccount.SessionStorage)
            .ExecuteAsync();

        accountManager.Save();
    }

    public async void testUsage2()
    {
        var accountManager = JEAccountManager.LoadFrom();
        var newAccount = accountManager.CreateAccount();

        var result = await LoginHandlerBuilder.AuthenticateSilently()
            .WithSessionStorage(newAccount)
            .ExecuteAsync();
    }
}