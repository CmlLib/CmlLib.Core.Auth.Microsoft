using System;

namespace CmlLib.Core.Auth.Microsoft
{
    public class SessionException : Exception
    {
        public SessionException()
        {

        }

        public SessionException(string message) : base(message)
        {

        }

        public SessionException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}