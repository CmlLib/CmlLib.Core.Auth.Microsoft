# JavaEditionLoginHandler

Microsoft Xbox login handler for Minecraft: Java Edition

### LoginFromCache

`public async Task<T> LoginFromCache(CancellationToken cancellationToken = default)`

Get tokens from cache file. if the token is expired, it try to refresh tokens.

### LoginFromOAuth()

`public async Task<T> LoginFromOAuth(CancellationToken cancellationToken = default)`

Get tokens from the OAuth result. If you don't specify the OAuth result, then the MicrosoftOAuthApi will get an OAuth token.

With [WebView2WebUI](WinForm.md), It shows form displaying Microsoft OAuth web page.

```csharp
this._loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMicrosoftOAuthApi(builder => builder
        .WithWebUI(new WebView2WebUI()))
    .Build();

loginHandler.LoginWithOAuth(); // open WinForm and display Microsoft login page
```

With [MsalClient](MsalClient.md), All Microsoft OAuth token is handled by Msal.NET.

```csharp
this._loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMsalOAuth(app, factory => factory.CreateInteractiveApi())
    .Build();

loginHandler.LoginWithOAuth(); // MSAL.NET open Microsoft login page using system web browser
```

### LoginFromOAuth(msToken)

`public async Task<T> LoginFromOAuth(MicrosoftOAuthResponse msToken, CancellationToken cancellationToken = default)`

Specify Microsoft OAuth result. 

### ClearCache

Clear all caches.

### Error handling

Login methods can throw these exception: `MicrosoftOAuthException`, `XboxAuthException`, `MinecraftAuthException`.

You can identify the error by error codes: [Errors](Errors.md)

