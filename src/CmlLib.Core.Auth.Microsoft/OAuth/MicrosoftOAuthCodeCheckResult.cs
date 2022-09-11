using XboxAuthNet.OAuth;

namespace CmlLib.Core.Auth.Microsoft.OAuth
{
    public class MicrosoftOAuthCodeCheckResult
    {
        public bool IsSuccess { get; }
        public MicrosoftOAuthCode? OAuthCode { get; }

        public MicrosoftOAuthCodeCheckResult(bool result, MicrosoftOAuthCode code)
        {
            this.IsSuccess = result;
            this.OAuthCode = code;
        }
    }
}
