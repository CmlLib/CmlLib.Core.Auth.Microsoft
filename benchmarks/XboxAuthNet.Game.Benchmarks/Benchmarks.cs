using System.Text.Json.Nodes;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Benchmarks
{
    public class Benchmarks
    {
        private InMemorySessionStorage inMemorySessionStorage;
        private JsonSessionStorage jsonSessionStorage;

        private string testData;

        [GlobalSetup]
        public void GlobalSetup()
        {
            inMemorySessionStorage = new InMemorySessionStorage();
            jsonSessionStorage = JsonSessionStorage.CreateEmpty();

            testData = "1234567890";

            this.inMemorySessionStorage.Set<string>("key", testData);
            this.jsonSessionStorage.Set<string>("key", testData);
        }

        [Benchmark]
        public string GetInMemorySessionStorage()
        {
            return this.inMemorySessionStorage.Get<string>("key");
        }

        [Benchmark]
        public void SetInMemorySessionStorage()
        {
            this.inMemorySessionStorage.Set<string>("key", testData);
        }

        [Benchmark]
        public string GetJsonSessionStorage()
        {
            return this.jsonSessionStorage.Get<string>("key");
        }

        [Benchmark]
        public void SetJsonSessionStorage()
        {
            this.jsonSessionStorage.Set<string>("key", testData);
        }
    }
}
