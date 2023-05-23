using System;
using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Accounts;

public interface IXboxGameAccount : IComparable
{
    string? Identifier { get; }
    ISessionStorage SessionStorage { get; }
}