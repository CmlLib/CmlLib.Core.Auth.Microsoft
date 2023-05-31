using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public class CompositeAuthenticator : CompositeAuthenticatorBase
{
    public AuthenticateContext? Context { get; set; }

    public async ValueTask<ISessionStorage> ExecuteAsync()
    {
        if (Context == null)
            throw new InvalidOperationException("Context was not set");
        await ExecuteAsync(Context);
        return Context.SessionStorage;
    }

    public override async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        var startFrom = await validate(context);
        await auth(Authenticators, context, startFrom);
        await auth(PostAuthenticators, context, 0);
    }

    private async ValueTask<int> validate(AuthenticateContext context)
    {
        int startFrom = 0;
        foreach (var validator in Validators.AsEnumerable().Reverse())
        {
            var result = await validator.Validate(context);
            if (result)
                return startFrom;
            else
                startFrom++;
        }
        return startFrom;
    }

    private async ValueTask auth(
        IEnumerable<IAuthenticator> authenticators, 
        AuthenticateContext context, 
        int startFrom)
    {
        foreach (var authenticator in authenticators.Skip(startFrom))
        {
            await authenticator.ExecuteAsync(context);
        }
    }
}