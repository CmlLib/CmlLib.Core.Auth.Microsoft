
namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public static class Extensions
    {
        public static MicrosoftXboxBuilder<T> WithMicrosoftOAuth<T>(
            this T self, 
            MicrosoftOAuthClientInfo clientInfo)
            where T : IBuilderWithXboxAuthStrategy
        {
            return new MicrosoftXboxBuilder<T>(self, clientInfo);
        }
    }
}