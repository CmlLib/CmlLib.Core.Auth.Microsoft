using NUnit.Framework;
using XboxAuthNet.Game.Accounts;

namespace XboxAuthNet.Game.Test.Accounts;

[TestFixture]
public class XboxGameAccountCollectionTest
{

    [Test]
    public void TestGetAccount()
    {
        var account = TestAccount.Create("test");
        var collection = XboxGameAccountCollection.FromAccounts([account]);

        var actualAccount = collection.GetAccount("test");
        Assert.That(actualAccount, Is.EqualTo(account));
    }

    [Test]
    public void TestTryGetAccount()
    {
        var account = TestAccount.Create("test");
        var collection = XboxGameAccountCollection.FromAccounts([account]);

        var result = collection.TryGetAccount("test", out var actualAccount);
        Assert.True(result);
        Assert.That(actualAccount, Is.EqualTo(account));
    }

    [Test]
    public void TestTryGetAccountForNonExisting()
    {
        var account = TestAccount.Create("test");
        var collection = XboxGameAccountCollection.FromAccounts([account]);

        var result = collection.TryGetAccount("1234", out var actualAccount);
        Assert.False(result);
        Assert.That(actualAccount, Is.Not.EqualTo(account));
    }

    [Test]
    public void FromAccounts_EmptyCollection_ReturnsEmptyCollection()
    {
        var collection = XboxGameAccountCollection.FromAccounts([]);

        Assert.That(collection.Count, Is.EqualTo(0));
    }

    [Test]
    public void FromAccounts_SingleAccount_ReturnsCollectionWithAccount()
    {
        var account = TestAccount.Create("test");
        var collection = XboxGameAccountCollection.FromAccounts([account]);

        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(collection.GetAccount("test"), Is.EqualTo(account));
    }

    [Test]
    public void FromAccounts_MultipleAccountsWithDifferentIdentifiers_ReturnsAllAccounts()
    {
        var account1 = TestAccount.Create("account1");
        var account2 = TestAccount.Create("account2");
        var account3 = TestAccount.Create("account3");
        var collection = XboxGameAccountCollection.FromAccounts([account1, account2, account3]);

        Assert.That(collection.Count, Is.EqualTo(3));
        Assert.That(collection.GetAccount("account1"), Is.EqualTo(account1));
        Assert.That(collection.GetAccount("account2"), Is.EqualTo(account2));
        Assert.That(collection.GetAccount("account3"), Is.EqualTo(account3));
    }

    [Test]
    public void FromAccounts_AccountsWithNullIdentifiers_FiltersOutNullAccounts()
    {
        var validAccount = TestAccount.Create("valid");
        var nullAccount = TestAccount.CreateNull();
        var collection = XboxGameAccountCollection.FromAccounts([validAccount, nullAccount]);

        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(collection.GetAccount("valid"), Is.EqualTo(validAccount));
    }

    [Test]
    public void FromAccounts_AccountsWithEmptyIdentifiers_FiltersOutEmptyAccounts()
    {
        var validAccount = TestAccount.Create("valid");
        var emptyAccount = TestAccount.Create("");
        var collection = XboxGameAccountCollection.FromAccounts([validAccount, emptyAccount]);

        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(collection.GetAccount("valid"), Is.EqualTo(validAccount));
    }

    [Test]
    public void FromAccounts_AllAccountsHaveEmptyIdentifiers_ReturnsEmptyCollection()
    {
        var emptyAccount1 = TestAccount.Create("");
        var emptyAccount2 = TestAccount.Create("");
        var nullAccount = TestAccount.CreateNull();
        var collection = XboxGameAccountCollection.FromAccounts([emptyAccount1, emptyAccount2, nullAccount]);

        Assert.That(collection.Count, Is.EqualTo(0));
    }

    [Test]
    public void FromAccounts_DuplicateIdentifiers_KeepsFirstAccountBasedOnOrdering()
    {
        // Account with later LastAccess should be kept (ordered desc by LastAccess)
        var account1 = TestAccount.Create("duplicate", DateTime.MinValue.AddDays(1));
        var account2 = TestAccount.Create("duplicate", DateTime.MinValue.AddDays(0));
        var collection = XboxGameAccountCollection.FromAccounts([account1, account2]);

        Assert.That(collection.Count, Is.EqualTo(1));
        // The account with later LastAccess should be kept
        Assert.That(collection.GetAccount("duplicate"), Is.EqualTo(account1));
    }

    [Test]
    public void FromAccounts_DuplicateIdentifiersWithSameLastAccess_KeepsFirstAlphabetically()
    {
        // When LastAccess is same, identifier comparison matters, but since they are duplicates,
        // first one encountered after ordering should be kept
        var account1 = TestAccount.Create("duplicate", DateTime.MinValue);
        var account2 = TestAccount.Create("duplicate", DateTime.MinValue);
        var collection = XboxGameAccountCollection.FromAccounts([account1, account2]);

        Assert.That(collection.Count, Is.EqualTo(1));
        Assert.That(collection.GetAccount("duplicate"), Is.Not.Null);
    }

    [Test]
    public void FromAccounts_AccountsAreOrderedByLastAccessThenIdentifier()
    {
        var account1 = TestAccount.Create("b", DateTime.MinValue.AddDays(2));
        var account2 = TestAccount.Create("a", DateTime.MinValue.AddDays(2));
        var account3 = TestAccount.Create("c", DateTime.MinValue.AddDays(1));
        var collection = XboxGameAccountCollection.FromAccounts([account3, account2, account1]);

        var accounts = collection.ToArray();
        // Should be ordered by LastAccess desc, then by Identifier
        // account1 and account2 have LastAccess = MinValue + 2 days, ordered by identifier: a then b
        // account3 has LastAccess = MinValue + 1 day
        Assert.That(accounts[0], Is.EqualTo(account2)); // a, LastAccess +2
        Assert.That(accounts[1], Is.EqualTo(account1)); // b, LastAccess +2
        Assert.That(accounts[2], Is.EqualTo(account3)); // c, LastAccess +1
    }

    [Test]
    public void FromAccounts_MixedValidAndInvalidAccounts_FiltersAndOrdersCorrectly()
    {
        var valid1 = TestAccount.Create("valid1", DateTime.MinValue.AddDays(2));
        var valid2 = TestAccount.Create("valid2", DateTime.MinValue.AddDays(1));
        var nullAccount = TestAccount.CreateNull();
        var emptyAccount = TestAccount.Create("");
        var collection = XboxGameAccountCollection.FromAccounts([nullAccount, valid2, emptyAccount, valid1]);

        Assert.That(collection.Count, Is.EqualTo(2));
        var accounts = collection.ToArray();
        Assert.That(accounts[0], Is.EqualTo(valid1)); // LastAccess +2
        Assert.That(accounts[1], Is.EqualTo(valid2)); // LastAccess +1
    }

    [Test]
    public void FromAccounts_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => XboxGameAccountCollection.FromAccounts(null!));
    }
}