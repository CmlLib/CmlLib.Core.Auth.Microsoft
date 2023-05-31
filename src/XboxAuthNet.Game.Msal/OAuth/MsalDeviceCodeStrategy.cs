using XboxAuthNet.Game.SessionStorages;
using Microsoft.Identity.Client;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalDeviceCodeOAuth : MsalOAuth
{
    private readonly Func<DeviceCodeResult, Task> _deviceCodeResultCallback;

    public MsalDeviceCodeOAuth(
        IPublicClientApplication app,
        string[] scopes,
        ISessionSource<MicrosoftOAuthResponse> sessionSource,
        Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        : base(app, scopes, sessionSource)
    {
        this._deviceCodeResultCallback = deviceCodeResultCallback;
    }

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        IPublicClientApplication app, string[] scopes)
    {
        var result = await app.AcquireTokenWithDeviceCode(scopes, _deviceCodeResultCallback)
            .ExecuteAsync(Context.CancellationToken);
        return result;
    }
}
