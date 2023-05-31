namespace XboxAuthNet.Game.Authenticators;

public interface ICompositeAuthenticator : IAuthenticator
{
    void AddAuthenticator(ISessionValidator validator, IAuthenticator authenticator);
    void AddPostAuthenticator(IAuthenticator authenticator);
    void Clear();
}