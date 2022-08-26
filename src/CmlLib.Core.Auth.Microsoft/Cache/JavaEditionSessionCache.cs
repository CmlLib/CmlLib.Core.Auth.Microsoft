using CmlLib.Core.Auth.Microsoft.Mojang;
using System.Text.Json.Serialization;
using XboxAuthNet.XboxLive;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    public class JavaEditionSessionCache : SessionCacheBase
    {
        /// <summary>
        /// Minecraft tokens, issued by api.minecraftservices.com
        /// </summary>
        [JsonPropertyName("xboxSession")] // to keep backwards compatibility, it keeps old name. (not `mojangXboxToken`)
        public MojangXboxLoginResponse? MojangXboxToken { get; set; }

        /// <summary>
        /// MSession
        /// </summary>
        [JsonPropertyName("gameSession")]
        public MSession GameSession { get; set; } = new MSession();

        public override bool CheckValidation()
        {
            if (!base.CheckValidation())
                return false;

            if (GameSession == null)
                return false;

            if (MojangXboxToken == null)
                return false;

            return MojangXboxToken.CheckValidation();
        }
    }
}
