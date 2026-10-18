using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
#if NET10_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace XboxAuthNet.Game.Serialization;

public static class XboxGameJson
{
#if NET10_0_OR_GREATER
    [FeatureGuard(typeof(RequiresDynamicCodeAttribute))]
    [FeatureGuard(typeof(RequiresUnreferencedCodeAttribute))]
#endif
    internal static bool ReflectionEnabled
    {
#if NET10_0_OR_GREATER
        // 표준 JSON 기능 스위치는 AOT 게시 시 false로 고정되지만 분석기는 래퍼의 계약을 자동 추론하지 못한다.
        [UnconditionalSuppressMessage("Trimming", "IL4000", Justification = "System.Text.Json의 리플렉션 기능 스위치가 false일 때만 생성 메타데이터 경로를 사용하며 두 모드 모두 검증한다.")]
#endif
        get
        {
#if NET5_0_OR_GREATER
            if (!System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported)
                return false;
#endif
            return JsonSerializer.IsReflectionEnabledByDefault;
        }
    }

    /// <summary>호출자 옵션을 보존하면서 기본 세션 모델의 생성 메타데이터를 추가한다.</summary>
    public static JsonSerializerOptions CreateOptions(JsonSerializerOptions? source = null)
    {
        var options = new JsonSerializerOptions(source ?? new JsonSerializerOptions());
        var resolvers = new List<IJsonTypeInfoResolver>();
        if (source?.TypeInfoResolver is { } resolver)
            resolvers.Add(resolver);

        resolvers.Add(XboxGameJsonContext.Default);
        if (ReflectionEnabled)
            resolvers.Add(new DefaultJsonTypeInfoResolver());

        options.TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers.ToArray());
        return options;
    }

    internal static readonly JsonSerializerOptions DefaultOptions = CreateOptions();
}
