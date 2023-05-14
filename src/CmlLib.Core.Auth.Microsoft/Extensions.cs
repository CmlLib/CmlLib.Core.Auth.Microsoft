using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth.Microsoft.Builders;
using XboxAuthNet.Game.Builders;
using XboxAuthNet.Game.Executors;

namespace CmlLib.Core.Auth.Microsoft;

public static class Extensions
{
    public static XboxGameAuthenticationBuilder<JESession> WithInteractiveMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<JESession> self) =>
        self.WithInteractiveMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo);

    public static XboxGameAuthenticationBuilder<JESession> WithInteractiveMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<JESession> self,
        Action<MicrosoftXboxBuilder> builderInvoker) =>
        self.WithInteractiveMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static XboxGameAuthenticationBuilder<JESession> WithSilentMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<JESession> self) =>
        self.WithSilentMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo);

    public static XboxGameAuthenticationBuilder<JESession> WithSilentMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<JESession> self,
        Action<MicrosoftXboxBuilder> builderInvoker) =>
        self.WithSilentMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static XboxGameAuthenticationBuilder<JESession> WithMicrosoftOAuth(
        this XboxGameAuthenticationBuilder<JESession> self,
        Action<MicrosoftXboxBuilder> builderInvoker) =>
        self.WithMicrosoftOAuth(JELoginHandler.DefaultMicrosoftOAuthClientInfo, builderInvoker);

    public static XboxGameAuthenticationBuilder<JESession> WithJEAuthenticator(
        this XboxGameAuthenticationBuilder<JESession> self,
        Action<JEAuthenticatorBuilder> builderInvoker)
    {
        return self.WithGameAuthenticator(self => 
        {
            var builder = new JEAuthenticatorBuilder();
            builder.WithHttpClient(self.HttpClient);
            builder.WithSessionStorage(self.SessionStorage);
            builderInvoker.Invoke(builder);
            return builder.Build();
        });
    }

    public static async Task<MSession> ExecuteForLauncherAsync(this XboxGameAuthenticationBuilder<JESession> self)
    {
        var session = await self.ExecuteAsync();
        return session.ToLauncherSession();
    }
}