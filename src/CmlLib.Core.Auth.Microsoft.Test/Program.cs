// THIS FILE SHOULD NOT BE COMPILED!
// I can't debug unit test project in some reason. (maybe my cloud development environment setting has some problem)
// So whenever I need to debug unit test I just change project type and manually call unit test method
// It works

#if !TEST_SDK

using System;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var test = new TestJELoginBuilder();
            test.Setup();
            await test.TestSessionSourceReverse();

            This line sure that Program.cs is not compiled if TEST_SDK is enabled. 
            Just remove this if you need to compile Program.cs and debug some unit test methods.
        }
    }
}

#endif