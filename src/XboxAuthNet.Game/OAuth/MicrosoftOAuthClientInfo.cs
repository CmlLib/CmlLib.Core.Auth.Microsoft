using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.CodeFlow;

namespace XboxAuthNet.Game.OAuth;

public record class MicrosoftOAuthClientInfo(string ClientId, string Scopes)
{
    public ICodeFlowApiClient CreateApiClientForOAuthCode(HttpClient httpClient)
    {
        if (string.IsNullOrEmpty(ClientId))
            throw new InvalidOperationException("ClientId was empty");
        if (string.IsNullOrEmpty(Scopes))
            throw new InvalidCastException("Scopes was empty");

        return new CodeFlowLiveApiClient(ClientId, Scopes, httpClient);
    }
}
