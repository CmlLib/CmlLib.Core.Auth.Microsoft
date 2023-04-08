using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using XboxAuthNet.Game.OAuthStrategies;
using XboxAuthNet.OAuth.Models;

namespace XboxAuthNet.Game.Msal.OAuth
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
