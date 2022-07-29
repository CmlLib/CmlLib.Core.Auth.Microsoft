using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginCancelledException : Exception
    {
        public LoginCancelledException(string message) : base(message)
        {
            
        }
    }
}
