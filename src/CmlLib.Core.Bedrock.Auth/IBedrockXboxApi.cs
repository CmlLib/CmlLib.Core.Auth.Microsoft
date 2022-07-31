using CmlLib.Core.Bedrock.Auth.Models;

namespace CmlLib.Core.Bedrock.Auth
{
    public interface IBedrockXboxApi
    {
        Task<BedrockToken[]> LoginWithXbox(string uhs, string xsts);
    }
}
