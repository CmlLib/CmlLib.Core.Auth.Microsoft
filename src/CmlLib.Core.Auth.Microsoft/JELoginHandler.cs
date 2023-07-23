using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;
using CmlLib.Core.Auth.Microsoft.Sessions;

namespace CmlLib.Core.Auth.Microsoft;

public class JELoginHandler : XboxGameLoginHandler
{
    public readonly static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new(
        ClientId: XboxGameTitles.MinecraftJava,
        Scopes: XboxAuthConstants.XboxScope);

    public readonly static string RelyingParty = "rp://api.minecraftservices.com/";

    public JELoginHandler(
        LoginHandlerParameters parameters) :
        base(parameters)
    {

    }

    public Task<MSession> Authenticate(CancellationToken cancellationToken = default)
    {
        var account = AccountManager.GetDefaultAccount();
        return Authenticate(account, cancellationToken);
    }

    public async Task<MSession> Authenticate(
        IXboxGameAccount account,
        CancellationToken cancellationToken = default)
    {
        var authenticator = CreateAuthenticator(account, cancellationToken);
        authenticator.AddMicrosoftOAuthForJE(oauth => oauth.CodeFlow());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public Task<MSession> AuthenticateInteractively(
        CancellationToken cancellationToken = default) =>
        AuthenticateInteractively(AccountManager.NewAccount(), cancellationToken);

    public async Task<MSession> AuthenticateInteractively(
        IXboxGameAccount account,
        CancellationToken cancellationToken = default)
    {
        var authenticator = CreateAuthenticator(account, cancellationToken);
        authenticator.AddForceMicrosoftOAuthForJE(oauth => oauth.Interactive());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddForceJEAuthenticator();

        return await authenticator.ExecuteForLauncherAsync();
    }

    public Task<MSession> AuthenticateSilently(
        CancellationToken cancellationToken = default) =>
        AuthenticateSilently(AccountManager.GetDefaultAccount(), cancellationToken);

    public async Task<MSession> AuthenticateSilently(
        IXboxGameAccount account,
        CancellationToken cancellationToken = default)
    {
        var authenticator = CreateAuthenticator(account, cancellationToken);
        authenticator.AddMicrosoftOAuthForJE(oauth => oauth.Silent());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();

        return await authenticator.ExecuteForLauncherAsync();
    }

    public Task Signout(CancellationToken cancellationToken = default) =>
        Signout(AccountManager.GetDefaultAccount(), cancellationToken);

    public async Task Signout(
        IXboxGameAccount account,
        CancellationToken cancellationToken = default)
    {
        var authenticator = CreateAuthenticator(account, cancellationToken);
        authenticator.AddMicrosoftOAuthSignout(DefaultMicrosoftOAuthClientInfo);
        authenticator.AddXboxAuthSignout();
        authenticator.AddJESignout();
        await authenticator.ExecuteAsync();
    }

    public Task SignoutWithBrowser(CancellationToken cancellationToken = default) =>
        SignoutWithBrowser(AccountManager.GetDefaultAccount(), cancellationToken);

    public async Task SignoutWithBrowser(
        IXboxGameAccount account,
        CancellationToken cancellationToken = default)
    {
        var authenticator = CreateAuthenticator(account, cancellationToken);
        authenticator.AddMicrosoftOAuthBrowserSignout(DefaultMicrosoftOAuthClientInfo);
        authenticator.AddXboxAuthSignout();
        authenticator.AddJESignout();
        await authenticator.ExecuteAsync();
    }
}