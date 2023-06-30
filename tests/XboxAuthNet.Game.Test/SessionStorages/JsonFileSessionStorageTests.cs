using System.Diagnostics;
using NUnit.Framework;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test;

[TestFixture]
public class JsonFileSessionStorageTests
{
    private ISessionStorage? sessionStorage;
    private string? filePath;

    [OneTimeSetUp]
    public void Setup()
    {
        filePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        sessionStorage = createSessionStorage();
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    private ISessionStorage createSessionStorage()
    {
        if (string.IsNullOrEmpty(filePath))
            throw new InvalidOperationException();
        return new JsonFileSessionStorage(filePath);
    }

    [Test]
    [TestCase(1)]
    [TestCase(int.MaxValue)]
    [TestCase(int.MinValue)]
    public void TestIntValue(int value)
    {
        Assert.NotNull(sessionStorage);
        sessionStorage!.Set<int>(nameof(TestIntValue), value);
        var savedValue = sessionStorage!.Get<int>(nameof(TestIntValue));

        Assert.AreEqual(value, savedValue);
    }

    [Test]
    public void TestObject()
    {
        Assert.NotNull(sessionStorage);

        var mockObject = new MockObject()
        {
            Name = "Hello",
            Age = 1234,
            Type = 'c',
            IsObject = true,
            InnerData1 = new InnerMockObject("T1", 1),
            InnerData2 = new InnerMockObject("T2", 2)
        };

        sessionStorage!.Set<MockObject>(nameof(TestObject), mockObject);
        var savedValue = sessionStorage.Get<MockObject>(nameof(TestObject));

        // compare the value of object, not a reference.
        var copiedMockObject = mockObject with { }; // same value, new reference
        Assert.True(!Object.ReferenceEquals(mockObject, copiedMockObject));
        Assert.AreEqual(copiedMockObject, savedValue);
    }

    [Test]
    public void TestWithNewInstance()
    {
        Assert.NotNull(sessionStorage);

        var testKey = nameof(TestWithNewInstance);
        var testData = "test_data_for_" + testKey;

        sessionStorage!.Set<string>(testKey, testData);

        var newSessionStorage = createSessionStorage();
        var savedValue = newSessionStorage.Get<string>(testKey);

        Assert.AreEqual(testData, savedValue);
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        if (string.IsNullOrEmpty(filePath))
            return;

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        Trace.Flush();
    }
}