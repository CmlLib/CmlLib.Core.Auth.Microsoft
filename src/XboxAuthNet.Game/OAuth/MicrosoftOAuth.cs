using XboxAuthNet.Game.Authenticators;
using XboxAuthNet.OAuth;

namespace XboxAuthNet.Game.OAuth;

public abstract class MicrosoftOAuth : SessionAuthenticator<MicrosoftOAuthResponse>
{
    private MicrosoftOAuthParameters _parameters;

    public MicrosoftOAuth(MicrosoftOAuthParameters parameters) : base(parameters.SessionSource)
    {
        this._parameters = parameters;
    }

    protected override async ValueTask<MicrosoftOAuthResponse?> Authenticate(AuthenticateContext context)
    {
        var response = await Authenticate(context, _parameters);
        var loginHint = response?.DecodeIdTokenPayload()?.Subject;
        _parameters.LoginHintSource.Set(context.SessionStorage, loginHint);
        return response;
    }

    protected abstract ValueTask<MicrosoftOAuthResponse?> Authenticate(
        AuthenticateContext context, MicrosoftOAuthParameters parameters);
}
