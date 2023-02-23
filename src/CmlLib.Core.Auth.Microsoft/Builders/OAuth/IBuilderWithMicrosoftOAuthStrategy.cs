using CmlLib.Core.Auth.Microsoft.OAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public interface IBuilderWithMicrosoftOAuthStrategy<T>
    {
        MicrosoftOAuthStrategyFactoryContext MicrosoftOAuthBuilderContext { get; }
        T WithMicrosoftOAuth(IMicrosoftOAuthStrategy strategy);
    }
}