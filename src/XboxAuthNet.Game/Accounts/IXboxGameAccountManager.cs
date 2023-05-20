
namespace XboxAuthNet.Game.Accounts;

public interface IXboxGameAccountManager
{
    XboxGameAccountCollection Accounts { get; }
    IXboxGameAccount GetDefaultAccount();
    IXboxGameAccount NewAccount();
    void LoadAccounts();
    void SaveAccounts();
}