namespace XboxAuthNet.Game.Authenticators;

public class AuthenticatorCollection : CompositeAuthenticatorBase
{
    public override async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        var count = Authenticators.Count();
        if (count != Validators.Count())
            throw new InvalidOperationException("count not match");

        for (int i = 0; i < count; i++)
        {
            var valid = await Validators.ElementAt(i).Validate(context);
            if (!valid)
                await Authenticators.ElementAt(i).ExecuteAsync(context);
        }

        await ExecutePostAuthenticators(context);
    }
}