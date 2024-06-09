using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game;

public interface IAuthenticationProvider
{
    IAuthenticator CreateSilentAuthenticator();
    ISessionValidator CreateSessionValidatorForInteractiveAuthenticator();
    IAuthenticator CreateInteractiveAuthenticator();
    ISessionValidator CreateSessionValidatorForSilentAuthenticator();
    IAuthenticator Signout();
}