using Microsoft.Identity.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalDeviceCodeStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IPublicClientApplication _app;
        private readonly Func<DeviceCodeResult, Task> _deviceCodeResultCallback;
        private readonly string[] scopes;

        public MsalDeviceCodeStrategy(
            IPublicClientApplication app,
            Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            this._app = app;
            this._deviceCodeResultCallback = deviceCodeResultCallback;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _app.AcquireTokenWithDeviceCode(scopes, _deviceCodeResultCallback)
                .ExecuteAsync();

            return MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}
