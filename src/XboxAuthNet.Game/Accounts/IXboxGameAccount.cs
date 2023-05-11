using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game;

public interface IXboxGameAccount
{
    string? Identifier { get; }
    ISessionStorage SessionStorage { get; }
}