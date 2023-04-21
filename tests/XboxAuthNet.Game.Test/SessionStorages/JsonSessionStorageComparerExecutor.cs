using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XboxAuthNet.Game.Test.SessionStorages
{
    [TestFixture]
    [Category("long")]
    public class JsonSessionStorageComparerExecutor
    {
        private JsonSessionStorageComparer comparer = null!;

        [SetUp]
        public void Setup()
        {
            comparer = new JsonSessionStorageComparer(Random.Shared);
        }

        [Test]
        public void Test()
        {
            foreach (var operations in generateOperations())
            {
                //Console.WriteLine(operations);
                Test(operations);
            }
        }

        private IEnumerable<string> generateOperations()
        {
            var availableOps = comparer.Operations.Keys.ToArray();
            return M(6, 0, availableOps.Length, availableOps)
                .Select(chars => new string(chars.ToArray()));
        }

        public void Test(string ops)
        {
            foreach (var op in ops.ToCharArray())
            {
                assertEqualResult(op);
            }
        }

        public void assertEqualResult(char op)
        {
            comparer.Do(op);
            assertCollectionIsEqual();
        }

        public void assertCollectionIsEqual()
        {
            var count = 0;
            foreach (var key in comparer.SessionStorage.Keys)
            {
                Assert.That(comparer.SessionStorage.Get<string>(key), Is.EqualTo(comparer.Dictionary[key]));
                count++;
            }

            Assert.That(count, Is.EqualTo(comparer.Dictionary.Count));
        }

        static IEnumerable<T> Prepend<T>(T first, IEnumerable<T> rest)
        {
            yield return first;
            foreach (var item in rest)
                yield return item;
        }

        static IEnumerable<IEnumerable<T>> M<T>(int p, int t1, int t2, T[] arr)
        {
            if (p == 0)
                yield return Enumerable.Empty<T>();
            else
                for (int first = t1; first < t2; ++first)
                    foreach (var rest in M(p - 1, t1, t2, arr))
                        yield return Prepend(arr[first], rest);
        }
    }
}
