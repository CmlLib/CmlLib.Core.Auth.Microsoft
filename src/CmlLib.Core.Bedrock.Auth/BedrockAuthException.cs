using System;
using System.Text.Json;

namespace CmlLib.Core.Bedrock.Auth;

public class BEAuthException : Exception
{
    public BEAuthException(string? message) : base(message)
    {

    }

    public BEAuthException(string? error, string? errorMessage) : base($"{error} {errorMessage}")
    {

    }

    public static BEAuthException FromResponseBody(string responseBody, int statusCode)
    {
        try
        {
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            string? error = null;
            string? errorMessage = null;

            if (root.TryGetProperty("error", out var errorProp) &&
                errorProp.ValueKind == JsonValueKind.String)
                error = errorProp.GetString();
            if (root.TryGetProperty("errorMessage", out var errorMessageProp) &&
                errorMessageProp.ValueKind == JsonValueKind.String)
                errorMessage = errorMessageProp.GetString();

            if (string.IsNullOrEmpty(error))
                throw new FormatException();

            return new BEAuthException(error, errorMessage);
        }
        catch (JsonException)
        {
            throw new FormatException();
        }
    }
}
