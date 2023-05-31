using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public class AuthenticateContext
{
    public AuthenticateContext(
        ISessionStorage sessionStorage, 
        HttpClient httpClient,
        CancellationToken cancellationToken) =>
        (CancellationToken, SessionStorage, HttpClient) = 
        (cancellationToken, sessionStorage, httpClient);

    public CancellationToken CancellationToken { get; }
    public ISessionStorage SessionStorage { get; }
    public HttpClient HttpClient { get; }
}