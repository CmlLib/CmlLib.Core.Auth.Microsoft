using XboxAuthNet.Game.Msal;
using Microsoft.Identity.Client;

namespace CmlLib.Core.Auth.Microsoft.Test;

public class MsalSample
{
    IPublicClientApplication app = null!;

    public async Task Setup()
    {
        app = await MsalClientHelper.BuildApplicationWithCache("499c8d36-be2a-4231-9ebd-ef291b7bb64c");
    }

    public async Task<MSession> Silently()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();

        var authenticator = loginHandler.CreateAuthenticatorWithDefaultAccount();
        authenticator.AddMsalOAuth(app, msal => msal.Silent());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public async Task<MSession> Interactively()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();

        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(app, msal => msal.Interactive());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public async Task<MSession> DeviceCode()
    {
        var loginHandler = JELoginHandlerBuilder.BuildDefault();

        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(deviceCodeHandler));
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    private Task deviceCodeHandler(DeviceCodeResult deviceCode)
    {
        Console.WriteLine(deviceCode.Message);
        return Task.CompletedTask;
    }
}