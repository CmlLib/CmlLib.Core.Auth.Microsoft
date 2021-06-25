using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft
{
    public class MicrosoftOAuthException : Exception
    {
        public MicrosoftOAuthException()
        {

        }

        public MicrosoftOAuthException(MicrosoftOAuthResponse oauth)
        {
            this.Error = oauth.Error;
            this.ErrorDescription = oauth.ErrorDescription;
            this.ErrorCodes = oauth.ErrorCodes;
        }

        public string Error { get; private set; }
        public string ErrorDescription { get; private set; }
        public int[] ErrorCodes { get; private set; }
    }
}
