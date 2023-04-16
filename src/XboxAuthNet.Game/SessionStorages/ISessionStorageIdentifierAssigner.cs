
namespace XboxAuthNet.Game.SessionStorages
{
    public interface ISessionStorageKeyAssigner
    {
        string? GetStorageKey(ISessionStorage sessionStorage);
    }
}