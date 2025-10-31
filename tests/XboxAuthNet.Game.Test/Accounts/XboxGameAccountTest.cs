using NUnit.Framework;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Test.Accounts;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class XboxGameAccountTest
{
    [Test]
    public void compare_account_and_null_left()
    {
        var a = TestAccount.Create("a");
        Assert.AreEqual(1, a.CompareTo(null));
    }

    [Test]
    public void compare_only_last_access()
    {
        var left = TestAccount.Create("a", DateTime.MinValue);
        var right = TestAccount.Create("a", DateTime.MaxValue);

        // left > right
        Assert.AreEqual(1, left.CompareTo(right));
        Assert.AreEqual(-1, right.CompareTo(left));
    }

    [Test]
    public void compare_only_identifier()
    {
        var left = TestAccount.Create("a", DateTime.MinValue);
        var right = TestAccount.Create("b", DateTime.MinValue);

        // left < right
        Assert.AreEqual(-1, left.CompareTo(right));
        Assert.AreEqual(1, right.CompareTo(left));
    }

    [Test]
    public void compare_last_access_and_identifier()
    {
        var left = TestAccount.Create("a", DateTime.MinValue);
        var right = TestAccount.Create("b", DateTime.MaxValue);

        // left > right
        Assert.AreEqual(1, left.CompareTo(right));
        Assert.AreEqual(-1, right.CompareTo(left));
    }

    [Test]
    public void test_sort()
    {
        TestAccount? a0 = null;
        var a1 = TestAccount.Create("a", DateTime.MinValue.AddDays(1));
        var a2 = TestAccount.Create("b", DateTime.MinValue.AddDays(1));
        var a3 = TestAccount.Create("",  DateTime.MinValue.AddDays(0));
        var a4 = TestAccount.Create("a", DateTime.MinValue.AddDays(0));
        var a5 = TestAccount.Create("b", DateTime.MinValue.AddDays(0));
        var a6 = new object();

        var actual = new object?[] 
        {
            a6, a3, a0, a1, a4, a2, a5
        };
        Array.Sort(actual);

        var expected = new object?[]
        {
            a0, a1, a2, a3, a4, a5, a6
        };

        CollectionAssert.AreEqual(expected, actual);
    }
}