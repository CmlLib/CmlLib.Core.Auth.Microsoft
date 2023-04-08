using System;

namespace CmlLib.Core.Bedrock.Auth
{
    public class MinecraftAuthException : Exception
    {
        public MinecraftAuthException(string message) : base(message)
        {
            
        }
    }
}