using XboxAuthNet.Game.Authenticators;

namespace XboxAuthNet.Game;

public interface IAuthenticationProvider
{
    IAuthenticator Authenticate();
    ISessionValidator CreateSessionValidator();
    IAuthenticator AuthenticateSilently();
    IAuthenticator AuthenticateInteractively();
    IAuthenticator ClearSession();
    IAuthenticator Signout();
}