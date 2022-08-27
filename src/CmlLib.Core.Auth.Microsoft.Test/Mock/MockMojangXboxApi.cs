using CmlLib.Core.Auth.Microsoft.Mojang;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Test.Mock
{
    internal class MockMojangXboxApi : IMojangXboxApi
    {
        public Task<bool> CheckGameOwnership(string bearerToken)
        {
            return Task.FromResult(true);
        }

        public string MockProfileUsername = "MockMojangXboxApi_ProfileUsername";
        public string MockProfileUUID = "MockMojangXboxApi_ProfileUUID";

        public Task<MSession> GetProfileUsingToken(string bearerToken)
        {
            return Task.FromResult(new MSession(
                username: MockProfileUsername, 
                accessToken: bearerToken, 
                uuid: MockProfileUUID));
        }

        public string MockAccessToken = "MockMojangXboxApi_AccessToken";
        public string MockUsername = "MockMojangXboxApi_Username";

        public Task<MojangXboxLoginResponse> LoginWithXbox(string uhs, string xsts)
        {
            return Task.FromResult(new MojangXboxLoginResponse
            {
                AccessToken = MockAccessToken,
                Username = MockUsername
            });
        }
    }
}
