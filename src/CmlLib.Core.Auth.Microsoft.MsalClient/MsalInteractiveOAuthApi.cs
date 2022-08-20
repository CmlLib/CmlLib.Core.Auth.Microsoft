using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalInteractiveOAuthApi : MsalOAuthApiBase
    {
        private readonly Func<AcquireTokenInteractiveParameterBuilder, AcquireTokenInteractiveParameterBuilder>? builderFunc;

        public MsalInteractiveOAuthApi(
            IPublicClientApplication app, 
            Func<AcquireTokenInteractiveParameterBuilder, AcquireTokenInteractiveParameterBuilder>? builderFunc)
            : base(app)
        {
            this.builderFunc = builderFunc;
        }

        public override async Task<MicrosoftOAuthResponse> RequestNewTokens()
        {
            var builder = MsalApplication.AcquireTokenInteractive(MsalMinecraftLoginHelper.DefaultScopes);
            if (builderFunc != null)
                builder = builderFunc?.Invoke(builder);

            if (builder == null)
                throw new InvalidOperationException();

            var result = await builder.ExecuteAsync();
            return MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}
