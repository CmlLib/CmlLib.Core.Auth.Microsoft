using XboxAuthNet.Game;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.Game.Authenticators;
using CmlLib.Core.Bedrock.Auth.Sessions;

namespace CmlLib.Core.Bedrock.Auth;

public static class Extensions
{
    public static void AddXboxAuthForBE(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddXboxAuth(builder => 
        {
            builder.RelyingParty = BEAuthenticator.RelyingParty;
            return builderInvoker.Invoke(builder);
        });

    public static void AddForceXboxAuthForBE(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddForceXboxAuth(builder =>
        {
            builder.RelyingParty = BEAuthenticator.RelyingParty;
            return builderInvoker.Invoke(builder);
        });

    public static void AddBEAuthenticator(this ICompositeAuthenticator self)
    {
        var authenticator = new BEAuthenticator(
            XboxSessionSource.Default,
            BESessionSource.Default);
        self.AddAuthenticatorWithoutValidator(authenticator);
    }
}