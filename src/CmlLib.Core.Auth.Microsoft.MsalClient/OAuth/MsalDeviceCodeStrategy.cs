using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft.MsalClient.OAuth
{
    public class MsalDeviceCodeStrategy : IMicrosoftOAuthStrategy
    {
        private readonly IPublicClientApplication _app;
        private readonly Func<DeviceCodeResult, Task> _deviceCodeResultCallback;
        public string[] Scopes { get; set; } = MsalClientHelper.XboxScopes;

        public MsalDeviceCodeStrategy(
            IPublicClientApplication app,
            Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            this._app = app;
            this._deviceCodeResultCallback = deviceCodeResultCallback;
        }

        public async Task<MicrosoftOAuthResponse> Authenticate()
        {
            var result = await _app.AcquireTokenWithDeviceCode(Scopes, _deviceCodeResultCallback)
                .ExecuteAsync();

            return MsalClientHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}
