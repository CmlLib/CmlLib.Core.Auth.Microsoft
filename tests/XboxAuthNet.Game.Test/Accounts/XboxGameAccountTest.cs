using NUnit.Framework;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Test.Accounts;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class XboxGameAccountTest
{
    [Test]
    public void TestEqual()
    {
        var instance1 = TestAccount.Create("test1", DateTime.MaxValue);
        var instance2 = TestAccount.Create("test1", DateTime.MinValue);

        Assert.False(Object.ReferenceEquals(instance1, instance2));
        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Test]
    public void TestCompareLastAccess()
    {
        var instance1 = TestAccount.Create("a", DateTime.MinValue);
        var instance2 = TestAccount.Create("a", DateTime.MinValue.AddSeconds(10));

        // instance1 < instance2
        Assert.That(instance1, Is.LessThan(instance2));
        Assert.That(instance2, Is.GreaterThan(instance1));
    }

    [Test]
    public void TestCompareIdentifier()
    {
        var instance1 = TestAccount.Create("a", DateTime.MinValue);
        var instance2 = TestAccount.Create("b", DateTime.MinValue);

        // instance1 < instance2
        Assert.That(instance1, Is.LessThan(instance2));
        Assert.That(instance2, Is.GreaterThan(instance1));
    }

    [Test]
    public void TestCompareToEqual()
    {
        var instance1 = TestAccount.Create("a", DateTime.MinValue);
        var instance2 = TestAccount.Create("a", DateTime.MinValue);

        // instance1 == instance2
        Assert.That(instance1.CompareTo(instance2), Is.Zero);
    }
}