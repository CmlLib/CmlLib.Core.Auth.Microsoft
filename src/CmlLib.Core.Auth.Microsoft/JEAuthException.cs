using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft;

public class JEAuthException : Exception
{
    public JEAuthException(string? message) : base(message)
    {

    }

    public JEAuthException(string? error, string? errorType, string? errorMessage, int statusCode) : base(CreateMessageFromError(error, errorType, errorMessage)) =>
        (Error, ErrorType, ErrorMessage, StatusCode) = (error, errorType, errorMessage, statusCode);

    public JEAuthException(string? message, Exception ex) : base(message, ex)
    {

    }

    public int StatusCode { get; private set; }
    public string? ErrorType { get; private set; }
    public string? Error { get; private set; }
    public string? ErrorMessage { get; private set; }

    private static string CreateMessageFromError(string? error, string? errorType, string? errorMessage)
    {
        if (string.IsNullOrEmpty(error))
            error = errorType;
        if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(errorMessage))
            return $"{error}, {errorMessage}";
        if (!string.IsNullOrEmpty(error))
            return error!;
        if (!string.IsNullOrEmpty(errorMessage))
            return errorMessage!;
        return "";
    }

    public static JEAuthException FromResponseBody(string responseBody, int statusCode)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            string? error = null;
            string? errorType = null;
            string? errorMessage = null;

            if (root.TryGetProperty("error", out var errorProp) &&
                errorProp.ValueKind == JsonValueKind.String)
                error = errorProp.GetString();
            if (root.TryGetProperty("errorType", out var errorTypeProp) &&
                errorTypeProp.ValueKind == JsonValueKind.String)
                errorType = errorTypeProp.GetString();
            if (root.TryGetProperty("errorMessage", out var errorMessageProp) &&
                errorMessageProp.ValueKind == JsonValueKind.String)
                errorMessage = errorMessageProp.GetString();

            if (string.IsNullOrEmpty(error) && string.IsNullOrEmpty(errorType))
                throw new FormatException();

            return new JEAuthException(error, errorType, errorMessage, statusCode);
        }
        catch (JsonException)
        {
            throw new FormatException();
        }
    }
}
