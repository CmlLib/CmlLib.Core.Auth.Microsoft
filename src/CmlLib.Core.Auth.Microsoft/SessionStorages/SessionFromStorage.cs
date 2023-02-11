using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.SessionStorages
{
    public class SessionFromStorage<T> : ISessionSource<T>
        where T : class
    {
        private readonly string _keyName;
        private readonly ISessionStorage _sessionStorage;

        public SessionFromStorage(string keyName, ISessionStorage sessionStorage) =>
            (_keyName, _sessionStorage) = (keyName, sessionStorage);

        public ValueTask<T?> GetAsync() =>
            _sessionStorage.GetAsync<T>(_keyName);

        public ValueTask SetAsync(T? obj) =>
            _sessionStorage.SetAsync(_keyName, obj);
    }
}