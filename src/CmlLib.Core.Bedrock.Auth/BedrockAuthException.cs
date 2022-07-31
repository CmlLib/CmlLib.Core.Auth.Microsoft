using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CmlLib.Core.Bedrock.Auth
{
    public class BedrockAuthException : Exception
    {
        public BedrockAuthException(string? message) : base(message)
        {

        }

        public BedrockAuthException(string? error, string? errorMessage) : base($"{error} {errorMessage}")
        {

        }

        public static BedrockAuthException FromResponseBody(string responseBody, int statusCode)
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

                return new BedrockAuthException(error, errorMessage);
            }
            catch (JsonException)
            {
                throw new FormatException();
            }
        }
    }
}
