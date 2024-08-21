namespace XboxAuthNet.Game.Authenticators;

public class FallbackAuthenticator : CompositeAuthenticatorBase
{
    private readonly Type[] _catchExceptions;

    public FallbackAuthenticator() : this(new[] { typeof(Exception) })
    {
        
    }

    public FallbackAuthenticator(Type[] exceptions)
    {
        _catchExceptions = exceptions;
    }

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
            context.CancellationToken.ThrowIfCancellationRequested();

            var authenticator = Authenticators.ElementAt(i);
            var validator = Validators.ElementAt(i);

            try
            {
                var valid = await validator.Validate(context);
                if (valid)
                    continue;
                await authenticator.ExecuteAsync(context);
                return;
            }
            catch (Exception ex)
            {
                if (!checkToCatch(ex))
                    throw;
                    
                context.Logger.LogFallbackAuthenticatorException(ex);
                exceptions.Add(ex);

                if (i == count - 1) // failed at last authenticator
                {
                    throw new AggregateException(exceptions.AsEnumerable().Reverse());
                }
            }
        }
    }

    private bool checkToCatch(Exception ex)
    {
        foreach (var exType in _catchExceptions)
        {
            if (exType.IsAssignableFrom(ex.GetType()))
                return true;
        }
        return false;
    }
}