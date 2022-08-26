using CmlLib.Core.Auth.Microsoft.XboxLive;
using System.Text.Json.Serialization;
using XboxAuthNet.OAuth;
using XboxAuthNet.XboxLive;
using XboxAuthNet.XboxLive.Entity;

namespace CmlLib.Core.Auth.Microsoft.Cache
{
    /// <summary>
    /// Contains sessions to be stored
    /// </summary>
    public class SessionCacheBase
    {
        /// <summary>
        /// Microsoft OAuth tokens
        /// </summary>
        [JsonPropertyName("microsoftOAuthSession")]
        public MicrosoftOAuthResponse? MicrosoftOAuthToken { get; set; }

        /// <summary>
        /// Xbox live tokens
        /// </summary>
        [JsonPropertyName("xboxTokens")]
        public XboxAuthTokens? XboxTokens { get; set; }

        /// <summary>
        /// Checks if current cached session is valid. (example: not expired, not null)
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckValidation()
        {
            return true;
        }
    }
}
