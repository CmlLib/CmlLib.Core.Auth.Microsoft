using System;
using System.Collections.Generic;
using System.Text;

namespace CmlLib.Core.Auth.Microsoft
{
    public class LoginCancelledException : Exception
    {
        public LoginCancelledException(string? message) : base(message)
        {

        }

        public LoginCancelledException(string? errorCode, string? message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public string? ErrorCode { get; set; }
    }
}
