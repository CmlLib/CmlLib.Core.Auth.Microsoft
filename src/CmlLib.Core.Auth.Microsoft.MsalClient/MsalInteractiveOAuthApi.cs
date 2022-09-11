using Microsoft.Identity.Client;
using System;
using System.Threading;
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

        public override async Task<MicrosoftOAuthResponse> RequestNewTokens(CancellationToken cancellationToken)
        {
            var builder = MsalApplication.AcquireTokenInteractive(Scopes);
            if (builderFunc != null)
                builder = builderFunc?.Invoke(builder);

            if (builder == null)
                throw new InvalidOperationException("builderFunc returns null");

            var result = await builder.ExecuteAsync(cancellationToken);
            return MsalMinecraftLoginHelper.ToMicrosoftOAuthResponse(result);
        }
    }
}
