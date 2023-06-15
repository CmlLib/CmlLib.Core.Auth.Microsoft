using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft;

public class JELoginHandlerBuilder : 
    XboxGameLoginHandlerBuilderBase<JELoginHandlerBuilder>
{
    public static JELoginHandler BuildDefault() => 
        new JELoginHandlerBuilder().Build();

    public JELoginHandlerBuilder WithAccountManager(string filePath)
    {
        return WithAccountManager(createAccountManager(filePath));
    }

    protected override IXboxGameAccountManager CreateDefaultAccountManager()
    {
        var defaultFilePath = Path.Combine(MinecraftPath.GetOSDefaultPath(), "cml_accounts.json");
        return createAccountManager(defaultFilePath);
    }

    private IXboxGameAccountManager createAccountManager(string filePath)
    {
        return new JsonXboxGameAccountManager(filePath, JEGameAccount.FromSessionStorage);
    }

    public JELoginHandler Build()
    {
        var parameters = BuildParameters();
        return new JELoginHandler(parameters);
    }
}
