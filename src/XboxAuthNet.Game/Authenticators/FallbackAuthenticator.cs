namespace XboxAuthNet.Game.Authenticators;

public class FallbackAuthenticator : CompositeAuthenticatorBase
{
    public override async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        await auth(context);
        foreach (var authenticator in PostAuthenticators)
        {
            await authenticator.ExecuteAsync(context);
        }
    }

    private async ValueTask auth(AuthenticateContext context)
    {
        var exceptions = new List<Exception>();
        var count = Authenticators.Count();

        for (int i = 0; i < count; i++)
        {
            var authenticator = Authenticators.ElementAt(i);
            var validator = Validators.ElementAtOrDefault(i);
            if (validator == default)
                validator = StaticValidator.Invalid;

            var valid = await validator.Validate(context);
            if (valid)
                continue;

            try
            {
                await authenticator.ExecuteAsync(context);
                return;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        throw new AggregateException(exceptions);
    }
}