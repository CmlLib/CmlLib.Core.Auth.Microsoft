namespace XboxAuthNet.Game.SessionStorages
{
    public class SessionFromStorage<T> : ISessionSource<T>
    {
        private readonly string _keyName;
        private readonly ISessionStorage _sessionStorage;

        public SessionFromStorage(string keyName, ISessionStorage sessionStorage) =>
            (_keyName, _sessionStorage) = (keyName, sessionStorage);

        public T? Get() =>
            _sessionStorage.GetOrDefault<T?>(_keyName, default);

        public void Set(T? obj) =>
            _sessionStorage.Set(_keyName, obj);

        public void Clear() =>
            Set(default);
    }
}