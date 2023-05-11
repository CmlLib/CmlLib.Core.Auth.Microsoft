using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Builders;

public interface IBuilderWithSessionStorage<T>
{
    T WithSessionStorage(ISessionStorage sessionStorage);
}