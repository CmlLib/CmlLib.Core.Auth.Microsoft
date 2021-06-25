using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft;
using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft
{
    public class SessionCache
    {
        public MicrosoftOAuthResponse MicrosoftOAuthSession { get; set; }
        public AuthenticationResponse XboxSession { get; set; }
        public MSession GameSession { get; set; }
    }
}
