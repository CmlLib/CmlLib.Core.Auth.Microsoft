using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalHomeAccountIdSource : SessionFromStorage<string>
{
    public static MsalHomeAccountIdSource Default { get; } = new();

    public const string KeyName = "MsalHomeAccountId";

    public MsalHomeAccountIdSource() : base(KeyName)
    {
    }
}
