using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalDeviceCodeOAuthApi : MsalOAuthApiBase
    {
        private readonly Func<DeviceCodeResult, Task> deviceCodeResultCallback;

        public MsalDeviceCodeOAuthApi(
            IPublicClientApplication app,
            Func<DeviceCodeResult, Task> deviceCodeResultCallback) : base(app)
        {
            this.deviceCodeResultCallback = deviceCodeResultCallback;
        }

        public override async Task<MicrosoftOAuthResponse> RequestNewTokens()
        {
            var result = await MsalApplication.AcquireTokenWithDeviceCode(MsalMinecraftLoginHelper.DefaultScopes, deviceCodeResultCallback)
                .ExecuteAsync();

            return MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}
