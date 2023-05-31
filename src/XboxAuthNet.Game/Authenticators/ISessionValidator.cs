namespace XboxAuthNet.Game.Authenticators;

public interface ISessionValidator
{
    ValueTask<bool> Validate(AuthenticateContext context);
}