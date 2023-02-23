using CmlLib.Core.Auth.Microsoft.XboxAuthStrategies;

namespace CmlLib.Core.Auth.Microsoft.Builders
{
    public interface IBuilderWithXboxAuthStrategy<T>
    {
        XboxAuthStrategyFactoryContext XboxAuthStrategyBuilderContext { get; }
        T WithXboxAuth(IXboxAuthStrategy strategy);
    }
}