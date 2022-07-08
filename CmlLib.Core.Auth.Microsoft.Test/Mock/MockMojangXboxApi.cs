using CmlLib.Core.Auth.Microsoft.Mojang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockMojangXboxApi : IMojangXboxApi
    {
        public Task<bool> CheckGameOwnership(string bearerToken)
        {
            return Task.FromResult(true);
        }

        public Task<MSession> GetProfileUsingToken(string bearerToken)
        {
            return Task.FromResult(new MSession("MockMojangXboxApi_ProfileUsername", bearerToken, "MockMojangXboxApi_ProfileUUID"));
        }

        public Task<MojangXboxLoginResponse> LoginWithXbox(string uhs, string xsts)
        {
            return Task.FromResult(new MojangXboxLoginResponse
            {
                AccessToken = "MockMojangXboxApi_AccessToken",
                Username = "MockMojangXboxApi_Username"
            });
        }
    }
}
