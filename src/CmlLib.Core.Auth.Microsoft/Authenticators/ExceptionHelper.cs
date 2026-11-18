using System.Text.Json;

namespace CmlLib.Core.Auth.Microsoft.Authenticators;

internal static class ExceptionHelper
{
    public static Exception CreateException(Exception ex, string resBody, HttpResponseMessage res)
    {
        if (ex is JsonException || ex is HttpRequestException)
        {
            JEAuthException authException;
            try
            {
                authException = JEAuthException.FromResponseBody(resBody, (int)res.StatusCode);
            }
            catch (FormatException)
            {
                authException = new JEAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}");
            }

            authException.RetryAfter = res.Headers.RetryAfter;
            return authException;
        }
        else
            return ex;
    }
}
