using CmlLib.Core.Auth.Microsoft;

namespace CmlLib.Core.Bedrock.Auth
{
    public static class Extensions
    {
        public static BedrockLoginHandlerBuilder ForBedrockEdition(this LoginHandlerBuilder builder)
        {
            var context = builder.Build();
            return new BedrockLoginHandlerBuilder(context);
        }
    }
}
