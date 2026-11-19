using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

internal static class ExceptionHelper
{
    public static Exception CreateException(Exception ex, string resBody, HttpResponseMessage res)
    {
        if (ex is JsonException || ex is HttpRequestException)
        {
            var statusCode = (int)res.StatusCode;
            JEAuthException authException;
            try
            {
                authException = JEAuthException.FromResponseBody(resBody, statusCode);
            }
            catch (FormatException)
            {
                authException = new JEAuthException($"{statusCode}: {res.ReasonPhrase}", statusCode);
            }

            authException.RetryAfter = res.Headers.RetryAfter;
            return authException;
        }
        else
            return ex;
    }
}
