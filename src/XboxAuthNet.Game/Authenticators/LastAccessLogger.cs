namespace XboxAuthNet.Game.Authenticators;

public class LastAccessLogger : IAuthenticator
{
    private static LastAccessLogger? _default;
    public static LastAccessLogger Default => _default ??= new();

    public ValueTask ExecuteAsync(AuthenticateContext context)
    {
        var str = LastAccessSource.Default.SetToNow(context.SessionStorage);
        context.Logger.LogLastAccess(str);
        return new ValueTask();
    }
}