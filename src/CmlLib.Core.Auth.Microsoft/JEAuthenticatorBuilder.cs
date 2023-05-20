using System;
using System.Net.Http;
using XboxAuthNet.Game;
using XboxAuthNet.Game.SessionStorages;
using XboxAuthNet.Game.GameAuthenticators;
using CmlLib.Core.Auth.Microsoft.Sessions;
using CmlLib.Core.Auth.Microsoft.GameAuthenticators;

namespace CmlLib.Core.Auth.Microsoft;

public class JEAuthenticatorBuilder
{
    public HttpClient? HttpClient { get; set; }
    public bool UseCaching { get; set; } = true;
    public bool CheckGameOwnership { get; set; } = false;
    public ISessionStorage? SessionStorage { get; set; }
    public ISessionSource<JESession>? SessionSource { get; set; }
    public bool UseSilentAuthenticator { get; set; } = false;

    public JEAuthenticatorBuilder WithHttpClient(HttpClient httpClient)
    {
        this.HttpClient = httpClient;
        return this;
    }

    public JEAuthenticatorBuilder WithCaching() => WithCaching(true);
    public JEAuthenticatorBuilder WithCaching(bool useCaching)
    {
        this.UseCaching = useCaching;
        return this;
    }

    public JEAuthenticatorBuilder WithCheckingGameOwnership() => WithCheckingGameOwnership(true);
    public JEAuthenticatorBuilder WithCheckingGameOwnership(bool checkGameOwnership)
    {
        this.CheckGameOwnership = checkGameOwnership;
        return this;
    }

    public JEAuthenticatorBuilder WithSessionStorage(ISessionStorage sessionStorage)
    {
        this.SessionStorage = sessionStorage;
        return this;
    }

    public JEAuthenticatorBuilder WithSessionSource(ISessionSource<JESession> sessionSource)
    {
        this.SessionSource = sessionSource;
        return this;
    }

    public JEAuthenticatorBuilder WithSilentAuthenticator() => WithSilentAuthenticator(true);
    public JEAuthenticatorBuilder WithSilentAuthenticator(bool value)
    {
        this.UseSilentAuthenticator = value;
        return this;
    }

    private ISessionSource<JESession> GetOrCreateSessionSource()
    {
        if (SessionStorage == null)
            return new InMemorySessionSource<JESession>();
        else
            return SessionSource ??= new JESessionSource(SessionStorage);
    }

    private IXboxGameAuthenticator<JESession> CreateDefaultGameAuthenticator()
    {
        var httpClient = HttpClient ?? HttpHelper.DefaultHttpClient.Value;
        var authenticator = new JEAuthenticator(httpClient, GetOrCreateSessionSource());
        authenticator.CheckGameOwnership = CheckGameOwnership;
        if (UseCaching)
            return new CachingGameSession<JESession>(authenticator, SessionSource!);
        else
            return authenticator;
    }

    public IXboxGameAuthenticator<JESession> Build()
    {
        var authenticator = CreateDefaultGameAuthenticator();
        if (UseSilentAuthenticator)
            return new SilentXboxGameAuthenticator<JESession>(authenticator, GetOrCreateSessionSource());
        else
            return authenticator;
    }

}