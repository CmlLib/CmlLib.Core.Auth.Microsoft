using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public class InMemorySessionSource<T> : ISessionSource<T> where T : class
    {
        private T? session;

        public ValueTask<T?> GetAsync() => 
            new ValueTask<T?>(session);

        public ValueTask SetAsync(T? obj)
        {
            session = obj;
            return new ValueTask();
        }
    }
}