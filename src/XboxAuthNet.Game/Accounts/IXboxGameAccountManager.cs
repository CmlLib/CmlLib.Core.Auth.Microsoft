namespace XboxAuthNet.Game.Accounts;

public interface IXboxGameAccountManager
{
    XboxGameAccountCollection GetAccounts();
    IXboxGameAccount GetDefaultAccount();
    IXboxGameAccount NewAccount();
    void ClearAccounts();
    void SaveAccounts();
}