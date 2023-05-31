using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Bedrock.Auth.Sessions;

public class BESessionSource : SessionFromStorage<BESession>
{
    private static BESessionSource? _default;
    public static BESessionSource Default => _default ??= new();

    public static readonly string KeyName = "BESession";
    public BESessionSource() : base(KeyName)
    {

    }
}