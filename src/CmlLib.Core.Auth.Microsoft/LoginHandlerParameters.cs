using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.XboxLive;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginHandlerParameters
    {
        public IMicrosoftOAuthApi? MicrosoftOAuthApi { get; set; }
        public IXboxLiveApi? XboxLiveApi { get; set; }
        public string? RelyingParty { get; set; }

        public void Validate()
        {
            if (MicrosoftOAuthApi == null)
                throw new ArgumentNullException(nameof(MicrosoftOAuthApi));

            if (XboxLiveApi == null)
                throw new ArgumentNullException(nameof(XboxLiveApi));

            if (string.IsNullOrEmpty(RelyingParty))
                throw new ArgumentNullException(nameof(RelyingParty));
        }
    }
}
