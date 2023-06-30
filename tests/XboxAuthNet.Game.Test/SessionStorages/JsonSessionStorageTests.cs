using System.Diagnostics;
using NUnit.Framework;
using XboxAuthNet.Game.SessionStorages;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace XboxAuthNet.Game.Test.SessionStorages;

[TestFixture]
public class JsonSessionStorageTests
{
    private JsonSessionStorage? sessionStorage;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Trace.Listeners.Add(new ConsoleTraceListener());
    }

    [SetUp]
    public void Setup()
    {
        sessionStorage = JsonSessionStorage.CreateEmpty();
    }

    private ISessionStorage createEmptyStorage()
    {
        return JsonSessionStorage.CreateEmpty();
    }


    [Test]
    public void TestNonExistsKey()
    {
        Assert.NotNull(sessionStorage);
        Assert.Throws<KeyNotFoundException>(() =>
        {
            sessionStorage!.Get<object>("qwerqwerqwerqwer");
        });
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
        Assert.True(sessionStorage.ContainsKey(nameof(TestObject)));
        Assert.True(sessionStorage.ContainsKey<MockObject>(nameof(TestObject)));
        Assert.False(sessionStorage.ContainsKey<InnerMockObject>(nameof(TestObject)));
        Assert.That(copiedMockObject, Is.EqualTo(savedValue));
    }

    [Test]
    public void TestContainsKey()
    {
        Assert.NotNull(sessionStorage);

        sessionStorage!.Set(nameof(TestContainsKey), "1234");

        Assert.True(sessionStorage!.ContainsKey(nameof(TestContainsKey)));
        Assert.False(sessionStorage!.ContainsKey(string.Empty));
        Assert.False(sessionStorage!.ContainsKey("1234"));
    }

    [Test]
    public void TestContainsKeyGeneric()
    {
        Assert.NotNull(sessionStorage);

        sessionStorage!.Set(nameof(TestContainsKeyGeneric), Math.PI);

        Assert.True(sessionStorage!.ContainsKey<double>(nameof(TestContainsKeyGeneric)));
        Assert.False(sessionStorage!.ContainsKey<string>(nameof(TestContainsKeyGeneric)));
        Assert.False(sessionStorage!.ContainsKey<double>(string.Empty));
        Assert.False(sessionStorage!.ContainsKey<string>("1234"));
    }

    [Test]
    public void TestIteration()
    {
        Assert.NotNull(sessionStorage);

        var testData = new (string, object)[]
        {
            ("int", 1),
            ("string", "hello"),
            ("double", Math.PI),
            ("object", new MockObject())
        };

        foreach (var kv in testData)
        {
            sessionStorage!.Set(kv.Item1, kv.Item2);
        }

        testItems(testData, sessionStorage!);
    }

    [Test]
    public void TestFromJson()
    {
        var testData = new (string, object)[]
        {
            ("int", 1),
            ("string", "hello"),
            ("double", Math.PI),
            ("object", new MockObject())
        };

        var json = new JsonObject();
        foreach (var kv in testData)
        {
            json.Add(kv.Item1, JsonSerializer.SerializeToNode(kv.Item2));
        }

        var sessionStorage = new JsonSessionStorage(json);
        testItems(testData, sessionStorage);
    }

    [Test]
    public void TestConvertToJson()
    {
        var mockObject = new MockObject();
        var json = new JsonObject();
        json.Add("double", Math.PI);
        json.Add("object", JsonSerializer.SerializeToNode(mockObject));

        var sessionStorage = new JsonSessionStorage(json);

        sessionStorage.Set("int", 1);
        sessionStorage.Set("string", "hello");
        sessionStorage.Set("string2", "hi");
        sessionStorage.Remove("string2");
        sessionStorage.Get<object>("string");

        json = sessionStorage.ToJsonObject();

        Assert.That(json.Count, Is.EqualTo(4));
        Assert.That((double)json["double"]!, Is.EqualTo(Math.PI));
        Assert.That(json["object"].Deserialize<MockObject>(), Is.EqualTo(mockObject));
        Assert.That((int)json["int"]!, Is.EqualTo(1));
        Assert.That((string)json["string"]!, Is.EqualTo("hello"));
    }

    private void testItems((string, object)[] testData, ISessionStorage sessionStorage)
    {
        var iterationResult = sessionStorage!.Keys.ToArray();
        Assert.That(iterationResult.Length, Is.EqualTo(testData.Length));

        Assert.That(iterationResult[0], Is.EqualTo(testData[0].Item1));
        Assert.That(sessionStorage.Get<int>(iterationResult[0]), Is.EqualTo(testData[0].Item2));
        Assert.True(sessionStorage!.ContainsKey<int>(iterationResult[0]));

        Assert.That(iterationResult[1], Is.EqualTo(testData[1].Item1));
        Assert.That(sessionStorage.Get<string>(iterationResult[1]), Is.EqualTo(testData[1].Item2));
        Assert.True(sessionStorage!.ContainsKey<string>(iterationResult[1]));

        Assert.That(iterationResult[2], Is.EqualTo(testData[2].Item1));
        Assert.That(sessionStorage.Get<double>(iterationResult[2]), Is.EqualTo(testData[2].Item2));
        Assert.True(sessionStorage!.ContainsKey<double>(iterationResult[2]));

        Assert.That(iterationResult[3], Is.EqualTo(testData[3].Item1));
        Assert.That(sessionStorage.Get<MockObject>(iterationResult[3]), Is.EqualTo(testData[3].Item2));
        Assert.True(sessionStorage!.ContainsKey<MockObject>(iterationResult[3]));
    }

    [Test]
    public void TestRemove()
    {
        var sessionStorage = new JsonSessionStorage(new JsonObject());
        sessionStorage.Set("a", "1234");
        sessionStorage.Set("b", 1234);
        sessionStorage.Set("c", new object());
        Assert.That(sessionStorage.Keys, Is.EqualTo(new [] { "a", "b", "c" }));

        sessionStorage.Set<string?>("a", default(string?));
        sessionStorage.Set<int>("b", default(int));
        sessionStorage.Set<object?>("c", default(object?));
        Assert.That(sessionStorage.Keys.Count(), Is.EqualTo(0));
    }

    [OneTimeTearDown]
    public void Teardown()
    {
        Trace.Flush();
    }
}
