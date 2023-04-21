using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.SessionStorages
{
    internal class JsonSessionStorageComparer
    {
        private readonly Random _rnd;

        public ISessionStorage SessionStorage { get; }
        public Dictionary<string, string> Dictionary { get; }
        public Dictionary<char, Action<int>> Operations { get; }

        public JsonSessionStorageComparer(Random rnd)
        {
            Operations = new()
            {
                ['g'] = get,
                ['r'] = remove,
                ['s'] = set,
                ['c'] = containsKey,
            };
            SessionStorage = JsonSessionStorage.CreateEmpty();
            Dictionary = new();
            _rnd = rnd;
        }

        public void Do(char op)
        {
            var param = _rnd.Next(0, 2);
            Operations[op].Invoke(param);
        }

        void get(int param)
        {
            var key = param.ToString();
            assertSameResult(
                () => SessionStorage.Get<string>(key),
                () => Dictionary[key]);
        }

        void remove(int param)
        {
            var key = param.ToString();
            assertSameResult(
                () => SessionStorage.Remove(key),
                () => Dictionary.Remove(key));
        }

        void set(int param)
        {
            var key = param.ToString();
            var value = "test_string";
            assertSameResult(
                () => SessionStorage.Set(key, value),
                () => Dictionary[key] = value);
        }

        void containsKey(int param)
        {
            var key = param.ToString();
            assertSameResult(
                () => SessionStorage.ContainsKey<string>(key),
                () => Dictionary.ContainsKey(key));
        }

        void assertSameResult(Action action1, Action action2)
        {
            Exception? ex1 = null;
            Exception? ex2 = null;

            try
            {
                action1();
            }
            catch (Exception e)
            {
                ex1 = e;
            }

            try
            {
                action2();
            }
            catch (Exception e)
            {
                ex2 = e;
            }

            Assert.That(ex1, Is.EqualTo(ex2));
        }

        void assertSameResult<T>(Func<T> action1, Func<T> action2)
        {
            T? result1 = default;
            T? result2 = default;
            Exception? ex1 = null;
            Exception? ex2 = null;

            try
            {
                result1 = action1();
            }
            catch (Exception e)
            {
                ex1 = e;
            }

            try
            {
                result2 = action2();
            }
            catch (Exception e)
            {
                ex2 = e;
            }

            Assert.That(result1, Is.EqualTo(result2));
            Assert.That(ex1?.GetType(), Is.EqualTo(ex2?.GetType()));
        }
    }
}
