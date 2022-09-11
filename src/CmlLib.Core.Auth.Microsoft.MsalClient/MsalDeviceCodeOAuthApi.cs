using Microsoft.Identity.Client;
using System;
using System.Threading;
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

        public override async Task<MicrosoftOAuthResponse> RequestNewTokens(CancellationToken cancellationToken)
        {
            var result = await MsalApplication.AcquireTokenWithDeviceCode(Scopes, deviceCodeResultCallback)
                .ExecuteAsync(cancellationToken);

            return MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}
