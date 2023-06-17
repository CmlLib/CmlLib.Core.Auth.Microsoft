using Microsoft.Extensions.Logging;

namespace XboxAuthNet.Game;

internal static partial class Log
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Trace,
        Message = "Save accounts")]
    public static partial void LogSaveAccounts(this ILogger logger);

    [LoggerMessage(
        EventId = 1002, 
        Level = LogLevel.Trace, 
        Message = "Write last access: {lastAccess}")]
    public static partial void LogLastAccess(this ILogger logger, string lastAccess);

    public static void LogFallbackAuthenticatorException(this ILogger logger, Exception exception)
    {
        logger.LogError("Catch exception by FallbackAuthenticator", exception);
    }

    [LoggerMessage(
        EventId = 51, 
        Level = LogLevel.Information, 
        Message = "Clean {name} by SessionCleaner")]
    public static partial void LogSessionCleaner(this ILogger logger, string name);

    [LoggerMessage(
        EventId = 52,
        Level = LogLevel.Information,
        Message = "Start InteractiveMicrosoftOAuth")]
    public static partial void LogInteractiveMicrosoftOAuth(this ILogger logger);

    [LoggerMessage(
        EventId = 53,
        Level = LogLevel.Information,
        Message = "Start SilentMicrosoftOAuth")]
    public static partial void LogSilentMicrosoftOAuth(this ILogger logger);

    [LoggerMessage(
        EventId = 54,
        Level = LogLevel.Information,
        Message = "Start MicrosoftOAuthSignout")]
    public static partial void LogMicrosoftOAuthSignout(this ILogger logger);

    [LoggerMessage(
        EventId = 55,
        Level = LogLevel.Information,
        Message = "MicrosoftOAuthValidator result: {result}")]
    public static partial void LogMicrosoftOAuthValidation(this ILogger logger, bool result);

    [LoggerMessage(
        EventId = 56,
        Level = LogLevel.Information,
        Message = "Start XboxDeviceTokenAuth")]
    public static partial void LogXboxDeviceToken(this ILogger logger);

    [LoggerMessage(
        EventId = 57,
        Level = LogLevel.Information,
        Message = "Start XboxSignedUserTokenAuth")]
    public static partial void LogXboxSignedUserToken(this ILogger logger);

    [LoggerMessage(
        EventId = 58,
        Level = LogLevel.Information,
        Message = "Start XboxSisuAuth")]
    public static partial void LogXboxSisu(this ILogger logger);

    [LoggerMessage(
        EventId = 59,
        Level = LogLevel.Information,
        Message = "Start XboxUserTokenAuth")]
    public static partial void LogXboxUserTokenAuth(this ILogger logger);

 
    [LoggerMessage(
        EventId = 60,
        Level = LogLevel.Information,
        Message = "Start XstsTokenAuth")]
    public static partial void LogXboxXstsTokenAuth(this ILogger logger);

    [LoggerMessage(
        EventId = 61,
        Level = LogLevel.Information,
        Message = "XboxSessionValidatorResult: {result}")]
    public static partial void LogXboxValidation(this ILogger logger, bool result);
}