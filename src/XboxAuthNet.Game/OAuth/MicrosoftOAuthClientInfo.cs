using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.OAuth;

public record class MicrosoftOAuthClientInfo(string ClientId, string Scopes)
{
    public MicrosoftOAuthCodeApiClient CreateApiClientForOAuthCode(HttpClient httpClient)
    {
        if (string.IsNullOrEmpty(ClientId))
            throw new InvalidOperationException("ClientId was empty");
        if (string.IsNullOrEmpty(Scopes))
            throw new InvalidCastException("Scopes was empty");

        return new MicrosoftOAuthCodeApiClient(ClientId, Scopes, httpClient);
    }
}
