using System.Text.Json.Serialization;
using XboxAuthNet.Game;

namespace CmlLib.Core.Auth.Microsoft.Sessions
{
    public class JESession : ISession
    {
        [JsonPropertyName("profile")]
        public JEProfile? Profile { get; set; }

        [JsonPropertyName("token")]
        public JEAuthenticationToken? Token { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(Profile?.Username))
                return false;
            if (string.IsNullOrEmpty(Profile?.UUID))
                return false;
            if (Token == null)
                return false;
            
            return Token.Validate();
        }

        public MSession ToLauncherSession()
        {
            return new MSession
            {
                Username = Profile?.Username,
                UUID = Profile?.UUID,
                AccessToken = Token?.AccessToken,
                UserType = "msa"
            };
        }
    }
}