using NUnit.Framework;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.Test.Accounts;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class XboxGameAccountTest
{
    public static XboxGameAccount NullIdentifier = TestAccount.CreateNull();
    public static XboxGameAccount EmptyIdentifier = TestAccount.Create("");

    public static XboxGameAccount FirstAccount = TestAccount.Create("a");
    public static XboxGameAccount SecondAccount = TestAccount.Create("b");
    public static XboxGameAccount ThirdAccount = TestAccount.Create("c");

    public static XboxGameAccount FirstSameAccount = TestAccount.Create("d", DateTime.MinValue);
    public static XboxGameAccount SecondSameAccount = TestAccount.Create("d", DateTime.MinValue.AddSeconds(10));
    public static XboxGameAccount ThirdSameAccount = TestAccount.Create("d", DateTime.MinValue.AddSeconds(20));

    public static XboxGameAccount[] TestCase1 = new[]
    {
        NullIdentifier, EmptyIdentifier, FirstAccount, FirstSameAccount
    };

    public static XboxGameAccount[] TestCase2 = new[]
    {
        NullIdentifier, EmptyIdentifier,
        FirstAccount, SecondAccount, ThirdAccount,
        FirstSameAccount, SecondSameAccount, ThirdSameAccount
    };

    public static XboxGameAccount[] NullOrEmptyAccounts = new[]
    {
        NullIdentifier, EmptyIdentifier
    };

    public static XboxGameAccount[] AccountsExceptNullOrEmpty = 
        TestCase2.Except(NullOrEmptyAccounts).ToArray();

    [Test]
    public void TestEqualOne()
    {
        foreach (var t in TestCase1)
        {
            TestEqualOne(t);
        }
    }

    public void TestEqualOne(XboxGameAccount a)
    {
        Assert.That(a.CompareTo(a), Is.Zero);
    }

    [Test]
    public void TestEqualTwo()
    {
        foreach (var t in TestCase1)
        {
            TestEqualTwo(t, t);
        }
    }

    public void TestEqualTwo(XboxGameAccount a, XboxGameAccount b)
    {
        Assert.That(a.CompareTo(b), Is.Zero);
        Assert.That(b.CompareTo(b), Is.Zero);
    }

    [Test]
    public void TestEqualThree()
    {
        foreach (var t in TestCase1)
        {
            TestEqualThree(t, t, t);
        }
    }

    public void TestEqualThree(XboxGameAccount a, XboxGameAccount b, XboxGameAccount c)
    {
        Assert.That(a.CompareTo(b), Is.Zero);
        Assert.That(b.CompareTo(c), Is.Zero);
        Assert.That(a.CompareTo(c), Is.Zero);
    }

    [Test]
    public void TestNotEqualTwo()
    {
        foreach (var a in TestCase2)
            foreach (var b in TestCase2)
            {
                // If a.CompareTo(b) returns a value other than zero,
                var x = a.CompareTo(b);
                if (x != 0)
                {
                    // then b.CompareTo(b) is required to return a value of the opposite sign
                    var y = b.CompareTo(a);
                    Assert.False(isSameSign(x, y));
                }
            }
    }

    [Test]
    public void TestNotEqualThree()
    {
        foreach (var a in TestCase2)
            foreach (var b in TestCase2)
                foreach (var c in TestCase2)
                {
                    // If a.CommpareTo(b) returns a value x that is not equal to zero,
                    // and b.CompareTo(c) returns a value y of the same sign as x,
                    var x = a.CompareTo(b);
                    var y = b.CompareTo(c);
                    if (x != 0 && isSameSign(x, y))
                    {
                        // then a.CompareTo(c) is required to return a value of the same sign
                        // as x and y
                        var z = a.CompareTo(c);
                        Assert.True(isSameSign(y, z));
                    }
                }
    }

    [Test]
    public void TestLess()
    {
        int length = AccountsExceptNullOrEmpty.Length;
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                Assert.That(
                    AccountsExceptNullOrEmpty[i], 
                    Is.LessThan(AccountsExceptNullOrEmpty[j]));
            }
        }
    }

    [Test]
    public void TestWithNullOrEmpty()
    {
        foreach (var nullOrEmptyAccount in NullOrEmptyAccounts)
        {
            foreach (var account in AccountsExceptNullOrEmpty)
            {
                Assert.That(nullOrEmptyAccount, Is.LessThan(account));
            }
        }
    }

    private bool isSameSign(int a, int b)
    {
        return a * b > 0;
    }
}