using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public sealed class MicrosoftOAuthCacheStorageAdapter : ICacheStorage<MicrosoftOAuthResponse>
    {
        private readonly ICacheStorage<XboxGameSession> _gameSessionStorage;

        public MicrosoftOAuthCacheStorageAdapter(ICacheStorage<XboxGameSession> gameSessionStorage)
        {
            this._gameSessionStorage = gameSessionStorage;
        }

        public MicrosoftOAuthResponse? Get()
        {
            return _gameSessionStorage.Get()?.OAuthSession;
        }

        public void Set(MicrosoftOAuthResponse? value)
        {
            var cachedSession = _gameSessionStorage.Get() ?? new XboxGameSession();
            cachedSession.OAuthSession = value;
            _gameSessionStorage.Set(cachedSession);
        }

        public void Clear()
        {
            _gameSessionStorage.Clear();
        }
    }
}