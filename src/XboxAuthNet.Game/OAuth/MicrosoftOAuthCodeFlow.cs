using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game.OAuth;

public class MicrosoftOAuthCodeFlow : IAuthenticationProvider
{
    private readonly MicrosoftOAuthBuilder _builder;

    public MicrosoftOAuthCodeFlow(MicrosoftOAuthClientInfo clientInfo)
    {
        _builder = new MicrosoftOAuthBuilder(clientInfo);
    }

    public IAuthenticator CreateInteractiveAuthenticator() => 
        _builder.Interactive();

    public ISessionValidator CreateSessionValidatorForInteractiveAuthenticator() =>
        StaticValidator.Invalid;

    public IAuthenticator CreateSilentAuthenticator() => 
        _builder.Silent();

    public ISessionValidator CreateSessionValidatorForSilentAuthenticator() =>
        _builder.Validator();

    public IAuthenticator Signout() =>
        _builder.SignoutWithBrowser();
}