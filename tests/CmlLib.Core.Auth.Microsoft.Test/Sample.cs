using XboxAuthNet.Game;

namespace CmlLib.Core.Auth.Microsoft.Test;

public class Sample
{
    public static async Task<MSession> Simplest()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var session = await loginHandler.Authenticate();
        return session;
    }

    public static async Task<MSession> Interactively()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        return await loginHandler.AuthenticateInteractively();
    }

    public static async Task<MSession> Silently()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        return await loginHandler.AuthenticateSilently();
    }

    public static async Task<MSession> InteractivelyWithOptions()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMicrosoftOAuthForJE(oauth => oauth.Interactive());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public static async Task<MSession> SilentlyWithOptions()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount();
        authenticator.AddForceMicrosoftOAuthForJE(oauth => oauth.Silent());
        authenticator.AddForceXboxAuth(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public static async Task Signout()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        await loginHandler.Signout();
    }
}