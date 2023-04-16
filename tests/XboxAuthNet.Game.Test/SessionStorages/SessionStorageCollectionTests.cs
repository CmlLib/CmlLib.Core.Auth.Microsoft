using NUnit.Framework;
using System.Linq;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.SessionStorages;

public class SessionStorageCollectionTests
{
    private ISessionStorage createMockSessionStorage(string? key)
    {
        var storage = new InMemorySessionStorage();
        storage.Set<string>("key", key);
        return storage;
    }

    private SessionStorageCollection createCollection()
    {
        return new SessionStorageCollection(new MockSessionStorageKeyAssigner());
    }

    [Test]
    public void TestInit()
    {
        var collection = createCollection();
        Assert.That(collection.Count, Is.EqualTo(0));
    }

    [Test]
    public void TestAdd()
    {
        var key = nameof(TestAdd);
        var collection = createCollection();
        var newStorage = createMockSessionStorage(key);
        collection.Add(newStorage);

        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(collection.ContainsKey(key));
        
        var added = collection.First();
        Assert.That(added.Key, Is.EqualTo(key));
        Assert.That(added.Value, Is.EqualTo(newStorage));
        Assert.That(collection.Contains(added.Value));
    }

    [Test]
    public void TestDuplicatedKey()
    {
        var key = nameof(TestDuplicatedKey);
        var collection = createCollection();

        var storage1 = createMockSessionStorage(key);
        var storage2 = createMockSessionStorage(key);
        storage2.Set<bool>("is_overwritten", true);

        collection.Add(storage1);
        collection.Add(storage2);

        Assert.That(collection.Count, Is.EqualTo(1));
        var added = collection.Get(key);
        var isOverwritten = added.Get<bool>("is_overwritten");
        Assert.That(isOverwritten);
    }

    [Test]
    public void TestClear()
    {
        var collection = createCollection();
        collection.Add(createMockSessionStorage("1"));
        collection.Add(createMockSessionStorage("2"));
        collection.Add(createMockSessionStorage("3"));
        collection.Clear();
        Assert.That(collection.Count, Is.EqualTo(0));
    }
}