using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthSessionSource : SessionFromStorage<MicrosoftOAuthResponse>
{
    private static MicrosoftOAuthSessionSource? _default;
    public static MicrosoftOAuthSessionSource Default => _default ??= new();

    public static string KeyName { get; } = "MicrosoftOAuth";
    public MicrosoftOAuthSessionSource()
     : base(KeyName)
    {

    }
}