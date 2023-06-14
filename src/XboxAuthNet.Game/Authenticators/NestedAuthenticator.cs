using XboxAuthNet.Game.SessionStorages;

namespace XboxAuthNet.Game.Authenticators;

public class NestedAuthenticator : CompositeAuthenticatorBase
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
        await auth(Authenticators.Count() - 1, context); // starts from last one
        await ExecutePostAuthenticators(context);
    }

    private async ValueTask auth(int index, AuthenticateContext context)
    {
        if (index < 0)
            return;

        var valid = await Validators.ElementAt(index).Validate(context);
        if (!valid)
        {
            await auth(index - 1, context);
            await Authenticators.ElementAt(index).ExecuteAsync(context);
        }
    }
}