using NUnit.Framework;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class LastAccessSourceTests
{
    [Test]
    public void Get()
    {
        var test = DateTime.Now;

        var sessionStorage = new InMemorySessionStorage();
        sessionStorage.Set(LastAccessSource.KeyName, test.ToString("o"));

        var lastAccessSource = new LastAccessSource();
        var actual = lastAccessSource.Get(sessionStorage);

        Assert.That(actual, Is.EqualTo(test).Within(1).Seconds);
    }

    [Test]
    public void GetInvalidFormat()
    {
        var test = "random_text_it_is_invalid_date_format";

        var sessionStorage = new InMemorySessionStorage();
        sessionStorage.Set(LastAccessSource.KeyName, test);

        var lastAccessSource = new LastAccessSource();
        var actual = lastAccessSource.Get(sessionStorage);

        Assert.AreEqual(DateTime.MinValue, actual);
    }

    [Test]
    public void GetNoKey()
    {
        var sessionStorage = new InMemorySessionStorage();
        var lastAccessSource = new LastAccessSource();
        var actual = lastAccessSource.Get(sessionStorage);

        Assert.AreEqual(DateTime.MinValue, actual);
    }
}