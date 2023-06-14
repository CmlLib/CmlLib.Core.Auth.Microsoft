namespace XboxAuthNet.Game.Authenticators;

public class AuthenticatorCollection : CompositeAuthenticatorBase
{
    public override async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        for (int i = 0; i < Authenticators.Count(); i++)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var valid = await Validators.ElementAt(i).Validate(context);
            if (!valid)
                await Authenticators.ElementAt(i).ExecuteAsync(context);
        }

        await ExecutePostAuthenticators(context);
    }
}