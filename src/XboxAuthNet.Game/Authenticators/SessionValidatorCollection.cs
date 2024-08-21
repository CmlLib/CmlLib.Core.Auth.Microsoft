
using System.Collections;

namespace XboxAuthNet.Game.Authenticators;

public class SessionValidatorCollection : ISessionValidator, IEnumerable<ISessionValidator>
{
    private readonly List<ISessionValidator> _validators = new();

    public void Add(ISessionValidator validator)
    {
        _validators.Add(validator);
    }

    public async ValueTask<bool> Validate(AuthenticateContext context)
    {
        foreach (var validator in _validators)
        {
            var result = await validator.Validate(context);
            if (!result)
                return false;
        }
        return true;
    }

    public IEnumerator<ISessionValidator> GetEnumerator() => _validators.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _validators.GetEnumerator();
}
