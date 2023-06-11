namespace XboxAuthNet.Game.Authenticators;

public class FallbackAuthenticator : CompositeAuthenticatorBase
{
    public override async ValueTask ExecuteAsync(AuthenticateContext context)
    {
        await tryAuth(context);
        await ExecutePostAuthenticators(context);
    }

    private async ValueTask tryAuth(AuthenticateContext context)
    {
        var exceptions = new List<Exception>();
        var count = Authenticators.Count();

        for (int i = 0; i < count; i++)
        {
            var authenticator = Authenticators.ElementAt(i);
            var validator = Validators.ElementAt(i);

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

                if (i == count - 1) // failed at last authenticator
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}