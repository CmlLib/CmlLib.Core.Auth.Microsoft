using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Sessions;

public class JEProfileSource : SessionFromStorage<JEProfile>
{
    private static JEProfileSource? _default;
    public static JEProfileSource Default => _default ??= new();

    public static string KeyName { get; } = "JEProfile";
    public JEProfileSource() : base(KeyName)
    {

    }
}