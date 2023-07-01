using Microsoft.Extensions.Logging;

namespace XboxAuthNet.Game;

internal static partial class Log
{
    [LoggerMessage(
        EventId = 749001,
        Level = LogLevel.Trace,
        Message = "Save accounts")]
    public static partial void LogSaveAccounts(this ILogger logger);

    [LoggerMessage(
        EventId = 749002, 
        Level = LogLevel.Trace, 
        Message = "Write last access: {lastAccess}")]
    public static partial void LogLastAccess(this ILogger logger, string lastAccess);

    public static void LogFallbackAuthenticatorException(this ILogger logger, Exception exception)
    {
        logger.LogError(new EventId(749401), exception, "Catch exception by FallbackAuthenticator");
    }

    [LoggerMessage(
        EventId =749201, 
        Level = LogLevel.Information, 
        Message = "Clean {name} by SessionCleaner")]
    public static partial void LogSessionCleaner(this ILogger logger, string name);

    [LoggerMessage(
        EventId = 749202,
        Level = LogLevel.Information,
        Message = "Start InteractiveMicrosoftOAuth")]
    public static partial void LogInteractiveMicrosoftOAuth(this ILogger logger);

    [LoggerMessage(
        EventId = 749203,
        Level = LogLevel.Information,
        Message = "Start SilentMicrosoftOAuth")]
    public static partial void LogSilentMicrosoftOAuth(this ILogger logger);

    [LoggerMessage(
        EventId = 749204,
        Level = LogLevel.Information,
        Message = "Start MicrosoftOAuthSignout")]
    public static partial void LogMicrosoftOAuthSignout(this ILogger logger);

    [LoggerMessage(
        EventId = 749205,
        Level = LogLevel.Information,
        Message = "MicrosoftOAuthValidator result: {result}")]
    public static partial void LogMicrosoftOAuthValidation(this ILogger logger, bool result);

    [LoggerMessage(
        EventId = 749206,
        Level = LogLevel.Information,
        Message = "Start XboxDeviceTokenAuth")]
    public static partial void LogXboxDeviceToken(this ILogger logger);

    [LoggerMessage(
        EventId = 749207,
        Level = LogLevel.Information,
        Message = "Start XboxSignedUserTokenAuth")]
    public static partial void LogXboxSignedUserToken(this ILogger logger);

    [LoggerMessage(
        EventId = 749208,
        Level = LogLevel.Information,
        Message = "Start XboxSisuAuth, {relyingParty}")]
    public static partial void LogXboxSisu(this ILogger logger, string relyingParty);

    [LoggerMessage(
        EventId = 749209,
        Level = LogLevel.Information,
        Message = "Start XboxUserTokenAuth")]
    public static partial void LogXboxUserTokenAuth(this ILogger logger);

 
    [LoggerMessage(
        EventId = 749210,
        Level = LogLevel.Information,
        Message = "Start XstsTokenAuth, {relyingParty}")]
    public static partial void LogXboxXstsTokenAuth(this ILogger logger, string relyingParty);

    [LoggerMessage(
        EventId = 749211,
        Level = LogLevel.Information,
        Message = "XboxXuiClaimsValidatorResult: {result}")]
    public static partial void LogXboxXuiClaimsValidation(this ILogger logger, bool result);

    [LoggerMessage(
        EventId = 749212,
        Level = LogLevel.Information,
        Message = "Start XboxXuiClaimsAuth")]
    public static partial void LogXboxXuiClaims(this ILogger logger);

    [LoggerMessage(
        EventId = 749213,
        Level = LogLevel.Information,
        Message = "XboxSessionValidatorResult: {result}")]
    public static partial void LogXboxValidation(this ILogger logger, bool result);
}