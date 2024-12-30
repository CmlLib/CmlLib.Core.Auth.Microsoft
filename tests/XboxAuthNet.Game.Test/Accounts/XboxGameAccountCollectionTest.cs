using NUnit.Framework;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.Accounts;

[TestFixture]
public class XboxGameAccountCollectionTest
{
    [Test]
    public void TestEmpty()
    {
        var collection = new XboxGameAccountCollection();
        Assert.That(collection.Count, Is.EqualTo(0));
    }

    [Test]
    public void TestAddOne()
    {
        var account = TestAccount.Create("test");
        var collection = new XboxGameAccountCollection();
        collection.Add(account);

        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(collection.First(), Is.EqualTo(account));
    }

    [Test]
    public void TestAddSameIdentifiers()
    {
        var account1 = TestAccount.Create("test", DateTime.MinValue);
        var account2 = TestAccount.Create("test", DateTime.MaxValue);
        var collection = new XboxGameAccountCollection()
        {
            account1, account2
        };

        var actualAccount = collection.GetAccount("test");
        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(actualAccount, Is.EqualTo(account2));
    }

    [Test]
    public void TestRemoveAccount()
    {
        var account1 = TestAccount.Create("test", DateTime.MinValue);
        var account2 = TestAccount.Create("test", DateTime.MaxValue);
        var account3 = TestAccount.Create("test2");
        var collection = new XboxGameAccountCollection()
        {
            account1, account2, account3
        };

        collection.RemoveAccount("test");
        Assert.That(collection.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestGetAccount()
    {
        var account = TestAccount.Create("test");
        var collection = new XboxGameAccountCollection();
        collection.Add(account);

        var actualAccount = collection.GetAccount("test");
        Assert.That(actualAccount, Is.EqualTo(account));
    }

    [Test]
    public void TestTryGetAccount()
    {
        var account = TestAccount.Create("test");
        var collection = new XboxGameAccountCollection();
        collection.Add(account);

        var result = collection.TryGetAccount("test", out var actualAccount);
        Assert.True(result);
        Assert.That(actualAccount, Is.EqualTo(account));
    }

    [Test]
    public void TestTryGetAccountForNonExisting()
    {
        var account = TestAccount.Create("test");
        var collection = new XboxGameAccountCollection();
        collection.Add(account);

        var result = collection.TryGetAccount("1234", out var actualAccount);
        Assert.False(result);
        Assert.That(actualAccount, Is.Not.EqualTo(account));
    }

    [Test]
    public void TestEnumerate()
    {
        var account1 = TestAccount.Create("z", DateTime.MinValue.AddDays(8));
        var account2 = TestAccount.Create("z", DateTime.MinValue.AddDays(1));
        var account3 = TestAccount.Create("a", DateTime.MinValue.AddDays(3));
        var account4 = TestAccount.Create("a", DateTime.MinValue.AddDays(6));
        var collection = new XboxGameAccountCollection()
        {
            account1, account2, account3, account4
        };

        IXboxGameAccount[] enumerated = collection.ToArray();
        Assert.That(enumerated, Is.EqualTo(new IXboxGameAccount[]
        {
            account1, // account1 and account2 have a same identifier, choose more recent one
            account4 // accounts are ordered by access time with descending order
        }));
    }
}