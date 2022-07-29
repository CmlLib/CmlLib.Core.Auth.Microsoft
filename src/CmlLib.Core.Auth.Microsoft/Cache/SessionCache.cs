using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using System.Text.Json.Serialization;
using CmlLib.Core.Auth.Microsoft.Mojang;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    /// <summary>
    /// Contains sessions to be stored
    /// </summary>
    public class SessionCache
    {
        /// <summary>
        /// Microsoft OAuth tokens
        /// </summary>
        [JsonPropertyName("microsoftOAuthSession")]
        public MicrosoftOAuthResponse? MicrosoftOAuthToken { get; set; }
        
        /// <summary>
        /// XSTS tokens, issued by xsts.auth.xboxlive.com
        /// </summary>
        [JsonPropertyName("xstsSession")]
        public XboxAuthResponse? XstsToken { get; set; }

        /// <summary>
        /// Minecraft tokens, issued by api.minecraftservices.com
        /// </summary>
        [JsonPropertyName("xboxSession")] // to keep backwards compatibility, it keeps old name. (not `mojangXboxToken`)
        public MojangXboxLoginResponse? MojangXboxToken { get; set; }

        /// <summary>
        /// MSession
        /// </summary>
        [JsonPropertyName("gameSession")]
        public MSession? GameSession { get; set; }
    }
}
