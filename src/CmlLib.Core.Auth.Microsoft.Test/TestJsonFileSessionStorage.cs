using System;
using System.IO;    
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit;
using NUnit.Framework;
using CmlLib.Core.Auth.Microsoft.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    [TestFixture]
    public class TestJsonFileSessionStorage
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
        public async Task TestEmpty()
        {
            var result = await sessionStorage!.GetAsync<string>("empty");
            Assert.IsNull(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public async Task TestIntValue(int value)
        {
            Assert.NotNull(sessionStorage);
            await sessionStorage!.SetAsync<int>(nameof(TestIntValue), value);
            var savedValue = await sessionStorage!.GetAsync<int>(nameof(TestIntValue));

            Assert.AreEqual(value, savedValue);
        }

        [Test]
        public async Task TestNonExistsKey()
        {
            Assert.NotNull(sessionStorage);
            var returnValue = await sessionStorage!.GetAsync<object>("qwerqwerqwerqwer");
            Assert.Null(returnValue);
        }

        [Test]
        public async Task TestObject()
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

            await sessionStorage!.SetAsync<MockObject>(nameof(TestObject), mockObject);
            var savedValue = await sessionStorage.GetAsync<MockObject>(nameof(TestObject));

            // compare the value of object, not a reference.
            var copiedMockObject = mockObject with {}; // same value, new reference
            Assert.True(!Object.ReferenceEquals(mockObject, copiedMockObject));
            Assert.AreEqual(copiedMockObject, savedValue);
        }

        [Test]
        public async Task TestWithNewInstance()
        {
            Assert.NotNull(sessionStorage);

            var testKey = nameof(TestWithNewInstance);
            var testData = "test_data_for_" + testKey;

            await sessionStorage!.SetAsync<string>(testKey, testData);

            var newSessionStorage = createSessionStorage();
            var savedValue = await newSessionStorage.GetAsync<string>(testKey);

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
}