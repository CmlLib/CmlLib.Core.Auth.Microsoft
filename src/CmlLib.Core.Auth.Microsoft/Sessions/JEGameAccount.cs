using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.SessionStorages;

namespace CmlLib.Core.Auth.Microsoft.Sessions;

public class JEGameAccount : XboxGameAccount
{
    public new static JEGameAccount FromSessionStorage(ISessionStorage sessionStorage)
    {
        return new JEGameAccount(sessionStorage);
    }

    public JEGameAccount(ISessionStorage sessionStorage) : base(sessionStorage)
    {

    }

    public JEProfile? Profile => JEProfileSource.Default.Get(SessionStorage);
    public JEToken? Token => JETokenSource.Default.Get(SessionStorage);

    public MSession ToLauncherSession()
    {
        return new MSession
        {
            Username = Profile?.Username,
            UUID = Profile?.UUID,
            AccessToken = Token?.AccessToken,
            UserType = "msa",
            Xuid = XboxTokens?.XstsToken?.XuiClaims?.XboxUserId
        };
    }
}