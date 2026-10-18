using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace XboxAuthNet.Game.Serialization;

public static class XboxGameJson
{
    public static JsonSerializerOptions CreateOptions(JsonSerializerOptions? source = null)
    {
        var options = new JsonSerializerOptions(source ?? new JsonSerializerOptions());
        var resolvers = new List<IJsonTypeInfoResolver>();
        if (source?.TypeInfoResolver is { } resolver)
            resolvers.Add(resolver);

        resolvers.Add(XboxGameJsonContext.Default);

        options.TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers.ToArray());
        return options;
    }

    internal static readonly JsonSerializerOptions DefaultOptions = CreateOptions();
}
