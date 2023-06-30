using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthLoginHintSource : SessionFromStorage<string>
{
    private static MicrosoftOAuthLoginHintSource? _instance;
    public static MicrosoftOAuthLoginHintSource Default => _instance ??= new MicrosoftOAuthLoginHintSource();

    public const string KeyName = "MicrosoftOAuthLoginHint";

    public MicrosoftOAuthLoginHintSource() : base(KeyName)
    {

    }
}
