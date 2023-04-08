using System;
using System.Threading.Tasks;

#if !TEST_SDK

namespace CmlLib.Core.Auth.Microsoft.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var sample = new MsalSample();
            await sample.Setup();

            var result = await sample.Silently();

            Console.WriteLine(result.AccessToken);
            Console.WriteLine(result.UUID);
            Console.WriteLine(result.Username);
            Console.WriteLine(result.UserType);
        }
    }
}

#endif