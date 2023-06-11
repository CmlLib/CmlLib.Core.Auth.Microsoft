using XboxAuthNet.Game;
using XboxAuthNet.Game.OAuth;
using XboxAuthNet.Game.XboxAuth;
using XboxAuthNet.Game.Authenticators;
using CmlLib.Core.Auth.Microsoft.Sessions;
using CmlLib.Core.Auth.Microsoft.Authenticators;

namespace CmlLib.Core.Auth.Microsoft;

public static class Extensions
{
    public static void AddMicrosoftOAuthForJE(
        this ICompositeAuthenticator self,
        Func<MicrosoftOAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static void AddForceMicrosoftOAuthForJE(
        this ICompositeAuthenticator self,
        Func<MicrosoftOAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddForceMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static void AddXboxAuthForJE(
        this ICompositeAuthenticator self,
        Func<XboxAuthBuilder, IAuthenticator> builderInvoker) =>
        self.AddXboxAuth(builder => 
        {
            builder.WithRelyingParty(JELoginHandler.RelyingParty);
            return builderInvoker(builder);
        });

    public static void AddJEAuthenticator(this ICompositeAuthenticator self) =>
        self.AddJEAuthenticator(builder => builder.Build());

    public static void AddJEAuthenticator(
        this ICompositeAuthenticator self,
        Func<JEAuthenticatorBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new JEAuthenticatorBuilder();
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticator(builder.TokenValidator(), builder.Build());
    }

    public static void AddForceJEAuthenticator(this ICompositeAuthenticator self) =>
        self.AddForceJEAuthenticator(builder => builder.Build());

    public static void AddForceJEAuthenticator(
        this ICompositeAuthenticator self,
        Func<JEAuthenticatorBuilder, IAuthenticator> builderInvoker)
    {
        var builder = new JEAuthenticatorBuilder();
        var authenticator = builderInvoker.Invoke(builder);
        self.AddAuthenticator(StaticValidator.Invalid, authenticator);
    }

    public static async Task<MSession> ExecuteForLauncherAsync(this CompositeAuthenticator self)
    {
        var session = await self.ExecuteAsync();
        var account = JEGameAccount.FromSessionStorage(session);
        return account.ToLauncherSession();
    }
}