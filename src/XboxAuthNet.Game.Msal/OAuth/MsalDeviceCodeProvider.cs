using Microsoft.Identity.Client;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalDeviceCodeProvider : IAuthenticationProvider
{
    private readonly MsalOAuthBuilder _builder;
    private readonly Func<DeviceCodeResult, Task> _callback;

    public MsalDeviceCodeProvider(IPublicClientApplication app, Func<DeviceCodeResult, Task> callback)
    {
        _builder = new MsalOAuthBuilder(app);
        _callback = callback;
    }

    public MsalDeviceCodeProvider(MsalOAuthBuilder builder, Func<DeviceCodeResult, Task> callback)
    {
        _builder = builder;
        _callback = callback;
    }

    public IAuthenticator Authenticate()
    {
        var authenticator = new FallbackAuthenticator();
        authenticator.AddAuthenticatorWithoutValidator(_builder.Silent());
        authenticator.AddAuthenticatorWithoutValidator(_builder.DeviceCode(_callback));
        return authenticator;
    }

    public IAuthenticator AuthenticateInteractively() => _builder.DeviceCode(_callback);
    public IAuthenticator AuthenticateSilently() => _builder.Silent();
    public ISessionValidator CreateSessionValidator() => StaticValidator.Invalid;
    public IAuthenticator ClearSession() => _builder.ClearSession();
    public IAuthenticator Signout() => _builder.ClearSession();
}
