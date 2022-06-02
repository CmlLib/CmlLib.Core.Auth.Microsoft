using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Mojang
{
    public class MinecraftAuthException : Exception
    {
        public MinecraftAuthException(string message) : base(message)
        {

        }

        public MinecraftAuthException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
