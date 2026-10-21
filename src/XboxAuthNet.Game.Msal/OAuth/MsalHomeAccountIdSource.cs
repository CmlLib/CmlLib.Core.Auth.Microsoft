using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Msal.OAuth;

/// <summary>MSAL 캐시의 계정과 저장된 게임 계정을 연결하는 식별자를 제공한다.</summary>
public class MsalHomeAccountIdSource : SessionFromStorage<string>
{
    public static MsalHomeAccountIdSource Default { get; } = new();

    public const string KeyName = "MsalHomeAccountId";

    public MsalHomeAccountIdSource() : base(KeyName)
    {
    }
}
