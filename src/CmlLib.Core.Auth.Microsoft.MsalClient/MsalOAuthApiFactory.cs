using CmlLib.Core.Auth.Microsoft.OAuth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.MsalClient
{
    public class MsalOAuthApiFactory
    {
        private readonly IPublicClientApplication app;

        public MsalOAuthApiFactory(IPublicClientApplication app)
        {
            this.app = app;
        }

        public IMicrosoftOAuthApi CreateInteractiveApi()
        {
            return new MsalInteractiveOAuthApi(app, null);
        }

        public IMicrosoftOAuthApi CreateInteractiveApi(Func<AcquireTokenInteractiveParameterBuilder, AcquireTokenInteractiveParameterBuilder> builder)
        {
            return new MsalInteractiveOAuthApi(app, builder);
        }

        public IMicrosoftOAuthApi CreateWithEmbeddedWebView()
        {
            return new MsalInteractiveOAuthApi(app, builder =>
                builder.WithUseEmbeddedWebView(true));
        }

        public IMicrosoftOAuthApi CreateDeviceCodeApi(Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            return new MsalDeviceCodeOAuthApi(app, deviceCodeResultCallback);
        }
    }
}
