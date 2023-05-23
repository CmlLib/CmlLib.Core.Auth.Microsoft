using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Test.SessionStorages
{
    public class MockSessionStorageKeyAssigner : ISessionStorageKeyAssigner
    {
        public string? GetStorageKey(ISessionStorage sessionStorage)
        {
            var key = sessionStorage.Get<string>("key");
            return key;
        }
    }
}