using System.Threading.Tasks;

namespace XboxAuthNet.Game.SessionStorages
{
    public class SessionFromStorage<T> : ISessionSource<T>
    {
        private readonly string _keyName;
        private readonly ISessionStorage _sessionStorage;

        public SessionFromStorage(string keyName, ISessionStorage sessionStorage) =>
            (_keyName, _sessionStorage) = (keyName, sessionStorage);

        public ValueTask<T?> GetAsync() =>
            _sessionStorage.GetAsync<T>(_keyName);

        public ValueTask SetAsync(T? obj) =>
            _sessionStorage.SetAsync(_keyName, obj);

        public ValueTask Clear() =>
            SetAsync(default(T?));
    }
}