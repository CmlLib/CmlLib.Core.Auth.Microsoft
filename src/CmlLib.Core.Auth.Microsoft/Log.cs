using Microsoft.Extensions.Logging;

namespace CmlLib.Core.Auth.Microsoft;

internal static partial class Log
{
    [LoggerMessage(
        EventId = 1234, 
        Level = LogLevel.Information, 
        Message = "Start JEGameOwnershipChecker")]
    public static partial void LogJEGameOwnershipChecker(this ILogger logger);

    [LoggerMessage(
        EventId = 1235, 
        Level = LogLevel.Information, 
        Message = "Start JEProfileAuthenticator")]
    public static partial void LogJEProfileAuthenticator(this ILogger logger);

    [LoggerMessage(
        EventId = 1236, 
        Level = LogLevel.Information, 
        Message = "JEProfileValidator result: {result}")]
    public static partial void LogJEProfileValidator(this ILogger logger, bool result);

    [LoggerMessage(
        EventId = 1237, 
        Level = LogLevel.Information, 
        Message = "Start JETokenAuthenticator")]
    public static partial void LogJETokenAuthenticator(this ILogger logger);

    [LoggerMessage(
        EventId = 1238, 
        Level = LogLevel.Information, 
        Message = "JETokenValidator result: {result}")]
    public static partial void LogJETokenValidator(this ILogger logger, bool result);
}