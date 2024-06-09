using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using CmlLib.Core.Auth.Microsoft.Sessions;
using System.Threading.Tasks.Sources;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;

namespace CmlLib.Core.Auth.Microsoft;

public class JELoginHandlerBuilder : 
    XboxGameLoginHandlerBuilderBase<JELoginHandlerBuilder>
{
    public static JELoginHandler BuildDefault() => 
        new JELoginHandlerBuilder().Build();

    private IAuthenticationProvider? oauth;
    public IAuthenticationProvider OAuthProvider 
    {
        get => oauth ??= new MicrosoftOAuthCodeFlowProvider(JELoginHandler.DefaultMicrosoftOAuthClientInfo);
        set => oauth = value;
    }

    private IAuthenticationProvider? xboxAuth;
    public IAuthenticationProvider XboxAuthProvider 
    {
        get => xboxAuth ??= new BasicXboxProvider(JELoginHandler.RelyingParty);
        set => xboxAuth = value;
    }

    public JELoginHandlerBuilder WithOAuthProvider(IAuthenticationProvider provider)
    {
        OAuthProvider = provider;
        return this;
    }

    public JELoginHandlerBuilder WithXboxAuthProvider(IAuthenticationProvider provider)
    {
        XboxAuthProvider = provider;
        return this;
    }

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
        return new JsonXboxGameAccountManager(
            filePath, 
            JEGameAccount.FromSessionStorage, 
            JsonXboxGameAccountManager.DefaultSerializerOption);
    }

    public JELoginHandler Build()
    {
        var parameters = BuildParameters();
        return new JELoginHandler(parameters, OAuthProvider, XboxAuthProvider);
    }
}
