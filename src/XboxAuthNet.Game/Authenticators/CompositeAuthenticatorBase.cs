namespace XboxAuthNet.Game.Authenticators;

public abstract class CompositeAuthenticatorBase : ICompositeAuthenticator
{
    private readonly List<ISessionValidator> _validators = new();
    private readonly List<IAuthenticator> _authenticators = new();
    private readonly List<IAuthenticator> _posts = new();

    protected IEnumerable<ISessionValidator> Validators => _validators;
    protected IEnumerable<IAuthenticator> Authenticators => _authenticators;
    protected IEnumerable<IAuthenticator> PostAuthenticators => _posts;

    public void AddAuthenticator(ISessionValidator validator, IAuthenticator authenticator)
    {
        _validators.Add(validator);
        _authenticators.Add(authenticator);
    }

    public void AddPostAuthenticator(IAuthenticator authenticator)
    {
        _posts.Add(authenticator);
    }

    public void Clear()
    {
        _validators.Clear();
        _authenticators.Clear();
        _posts.Clear();
    }

    public abstract ValueTask ExecuteAsync(AuthenticateContext context);

    protected async ValueTask ExecutePostAuthenticators(AuthenticateContext context)
    {
        foreach (var authenticator in PostAuthenticators)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            await authenticator.ExecuteAsync(context);
        }
    }
}