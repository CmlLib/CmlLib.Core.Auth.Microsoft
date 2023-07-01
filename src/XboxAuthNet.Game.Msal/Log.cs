using Microsoft.Extensions.Logging;

namespace XboxAuthNet.Game.Msal;

public static partial class Log
{
    [LoggerMessage(
        EventId = 750201, 
        Level = LogLevel.Information, 
        Message = "Start MsalDeviceCodeOAuth")]
    public static partial void LogMsalDeviceCode(this ILogger logger);

    [LoggerMessage(
        EventId = 750202, 
        Level = LogLevel.Information, 
        Message = "Start MsalInteractiveOAuth")]
    public static partial void LogMsalInteractiveOAuth(this ILogger logger);

    [LoggerMessage(
        EventId =750203, 
        Level = LogLevel.Information, 
        Message = "Start MsalSilentOAuth: {loginHint}")]
    public static partial void LogMsalSilentOAuth(this ILogger logger, string? loginHint);
}