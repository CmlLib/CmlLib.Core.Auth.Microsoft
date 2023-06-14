namespace XboxAuthNet.Game.Authenticators;

public class StaticValidator : ISessionValidator
{
    public static StaticValidator Valid = new(true);
    public static StaticValidator Invalid = new(false);

    private readonly bool _valid;

    public StaticValidator(bool valid) => _valid = valid;

    public ValueTask<bool> Validate(AuthenticateContext context)
    {
        return new ValueTask<bool>(_valid);
    }
}