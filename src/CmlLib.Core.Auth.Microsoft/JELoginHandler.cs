using XboxAuthNet.XboxLive;
using XboxAuthNet.Game;
using XboxAuthNet.Game.Accounts;
using XboxAuthNet.Game.OAuth;

namespace CmlLib.Core.Auth.Microsoft;

public class JELoginHandler : XboxGameLoginHandler
{
    public readonly static MicrosoftOAuthClientInfo DefaultMicrosoftOAuthClientInfo = new(
        ClientId: XboxGameTitles.MinecraftJava,
        Scopes: XboxAuthConstants.XboxScope);

    public readonly static string RelyingParty = "rp://api.minecraftservices.com/";

    private readonly IAuthenticationProvider _defaultOAuthProvider;
    private readonly IAuthenticationProvider _defaultXboxAuthProvider;

    public JELoginHandler(
        LoginHandlerParameters parameters,
        IAuthenticationProvider defaultOAuthProvider,
        IAuthenticationProvider defaultXboxAuthProvider) :
        base(parameters)
    {
        _defaultOAuthProvider = defaultOAuthProvider;
        _defaultXboxAuthProvider = defaultXboxAuthProvider;
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

        // OAuth
        authenticator.AddAuthenticator(
            _defaultOAuthProvider.CreateSessionValidator(),
            _defaultOAuthProvider.Authenticate());

        // XboxAuth
        authenticator.AddAuthenticator(
            _defaultXboxAuthProvider.CreateSessionValidator(),
            _defaultXboxAuthProvider.Authenticate());

        // JEAuth
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

        // OAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultOAuthProvider.AuthenticateInteractively());

        // XboxAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultXboxAuthProvider.AuthenticateInteractively());

        // JEAuth
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

        // OAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultOAuthProvider.AuthenticateSilently());

        // XboxAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultXboxAuthProvider.AuthenticateSilently());

        // JEAuth
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

        // OAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultOAuthProvider.ClearSession());

        // XboxAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultXboxAuthProvider.ClearSession());

        // JEAuth
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

        // OAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultOAuthProvider.Signout());

        // XboxAuth
        authenticator.AddAuthenticatorWithoutValidator(
            _defaultXboxAuthProvider.Signout());

        // JEAuth
        authenticator.AddJESignout();
        await authenticator.ExecuteAsync();
    }
}