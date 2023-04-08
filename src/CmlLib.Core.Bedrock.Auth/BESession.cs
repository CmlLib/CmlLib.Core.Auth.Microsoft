using CmlLib.Core.Auth.Microsoft;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BESession : ISession
    {
        public BEToken[]? Tokens { get; set; }

        public bool Validate()
        {
            return (Tokens != null)
                && (Tokens.Length > 0);
        }
    }
}