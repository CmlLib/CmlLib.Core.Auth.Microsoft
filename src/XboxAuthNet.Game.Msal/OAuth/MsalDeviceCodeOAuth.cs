using XboxAuthNet.Game.Authenticators;
using Microsoft.Identity.Client;

namespace XboxAuthNet.Game.Msal.OAuth;

public class MsalDeviceCodeOAuth : MsalOAuth
{
    private readonly Func<DeviceCodeResult, Task> _deviceCodeResultCallback;

    public MsalDeviceCodeOAuth(
        MsalOAuthParameters parameters,
        Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        : base(parameters) => 
        _deviceCodeResultCallback = deviceCodeResultCallback;

    protected override async ValueTask<AuthenticationResult> AuthenticateWithMsal(
        AuthenticateContext context, MsalOAuthParameters parameters)
    {
        context.Logger.LogMsalDeviceCode();
        var result = await parameters.MsalApplication
            .AcquireTokenWithDeviceCode(parameters.Scopes, _deviceCodeResultCallback)
            .ExecuteAsync(context.CancellationToken);
        return result;
    }
}
