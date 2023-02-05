using XboxAuthNet.OAuth.Models;

namespace CmlLib.Core.Auth.Microsoft
{
    public class XboxGameSession
    {
        public MicrosoftOAuthResponse? OAuthSession { get; set; }
        public XboxAuthTokens? XboxAuthSession { get; set; }

        public virtual bool Validate()
        {
            // check OAuthSession.Validate()
            return OAuthSession != null && true;
        }
    }
}