using XboxAuthNet.OAuth;
using XboxAuthNet.OAuth.Models;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthBuilder
{
    private readonly MicrosoftOAuthClientInfo _clientInfo;

    public MicrosoftOAuthBuilder(MicrosoftOAuthClientInfo clientInfo)
         => _clientInfo = clientInfo;
    
    private ISessionSource<MicrosoftOAuthResponse>? _sessionSource;
    public ISessionSource<MicrosoftOAuthResponse> SessionSource
    {
        get => _sessionSource ??= MicrosoftOAuthSessionSource.Default;
        set => _sessionSource = value;
    }

    public ISessionValidator Validator() =>
        new MicrosoftOAuthValidator(SessionSource);

    public IAuthenticator Silent() =>
        new SilentMicrosoftOAuth(_clientInfo, SessionSource);

    public IAuthenticator Interactive() =>
        Interactive(builder => {}, new MicrosoftOAuthParameters());

    public IAuthenticator Interactive(MicrosoftOAuthParameters parameters) =>
        Interactive(builder => {}, parameters);

    public IAuthenticator Interactive(Action<MicrosoftOAuthCodeFlowBuilder> builderInvoker) =>
        Interactive(builderInvoker, new MicrosoftOAuthParameters());

    public IAuthenticator Interactive(
        Action<MicrosoftOAuthCodeFlowBuilder> builderInvoker,
        MicrosoftOAuthParameters parameters)
    {
        return new InteractiveMicrosoftOAuth(
            _clientInfo, 
            builderInvoker, 
            parameters,
            SessionSource);
    }

    public IAuthenticator Signout() => 
        Signout(builder => {});

    public IAuthenticator Signout(Action<MicrosoftOAuthCodeFlowBuilder> builderInvoker)
    {
        return new MicrosoftOAuthSignout(
            _clientInfo, 
            builderInvoker, 
            SessionSource);
    }

    public IAuthenticator ClearSession() =>
        new SessionCleaner<MicrosoftOAuthResponse>(SessionSource);

    public IAuthenticator FromMicrosoftOAuthResponse(MicrosoftOAuthResponse response) => 
        new StaticSessionAuthenticator<MicrosoftOAuthResponse>(response, SessionSource);
}