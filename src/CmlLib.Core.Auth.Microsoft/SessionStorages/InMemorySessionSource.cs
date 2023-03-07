using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public class InMemorySessionSource<T> : ISessionSource<T>
    {
        private T? session = default(T);

        public ValueTask<T?> GetAsync() => 
            new ValueTask<T?>(session);

        public ValueTask SetAsync(T? obj)
        {
            session = obj;
            return new ValueTask();
        }
    }
}