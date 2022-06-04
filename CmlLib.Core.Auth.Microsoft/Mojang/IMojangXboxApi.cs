using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft.Mojang
{
    public interface IMojangXboxApi
    {
        Task<MojangXboxLoginResponse> LoginWithXbox(string uhs, string xsts);
        Task<bool> CheckGameOwnership(string bearerToken);
        Task<MSession> GetProfileUsingToken(string bearerToken);
    }
}
