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
    public class TestJsonSessionStorage
    {
        private ISessionStorage? sessionStorage;
        private string? filePath;

        [OneTimeSetUp]
        public void Setup()
        {
            filePath = $"tmp_test/{Path.GetTempFileName()}/{Path.GetTempFileName()}";
            sessionStorage = new JsonNodeStorage(filePath);

            Trace.Listeners.Add(new ConsoleTraceListener()); 
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
            Assert.AreEqual(copiedMockObject, savedValue);
        }

        [TearDown]
        public void Teardown()
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Directory.Delete("tmp_test", true);
            }

            Trace.Flush();
        }
    }
}