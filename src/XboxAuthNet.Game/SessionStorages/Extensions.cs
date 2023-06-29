namespace XboxAuthNet.Game.SessionStorages;

public static class Extensions
{
    public static IEnumerable<string> GetKeysForStoring(this ISessionStorage sessionStorage) => 
        sessionStorage.Keys
            .Where(k => sessionStorage.GetKeyMode(k) != SessionStorageKeyMode.NoStore);
}
