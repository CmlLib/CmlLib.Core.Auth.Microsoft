namespace XboxAuthNet.Game.SessionStorages;
public enum SessionStorageKeyMode
{
    Default,

    /// <summary>
    /// Do not store the key value pair to permanent storage (ex: disk)
    /// After ISessionStorage is disposed, The key is not accessible anymore.
    /// </summary>
    NoStore
}
