# CmlLib.Core.Auth.Microsoft.UI.WinForm

Provides Windows Forms to process Microsoft OAuth, with WebView2.

## Install

Install Nuget package:
- [CmlLib.Core.Auth.Microsoft.UI.WinForm](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.UI.WinForm)

## WebView2

**Important!** All users (including end-user) should install WebView2 runtime.
- [WebView2 runtime download page](https://developer.microsoft.com/en-us/microsoft-edge/webview2/#download-section)
- [WebView2 runtime evergreen installer direct download link](https://go.microsoft.com/fwlink/p/?LinkId=2124703)


## Usage

```csharp
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.OAuth;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
```

### Login

```csharp
var loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMicrosoftOAuthApi(builder => builder
        .builder.WithWebUI(new WebView2WebUI()))
    .Build();

try
{
    var session = await loginHandler.LoginFromCache();
    return session.GameSession;
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    var session = await loginHandler.LoginFromOAuth();
    return session.GameSession;
}
```

### With optional parameters

```csharp
var loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMicrosoftOAuthApi(builder => builder
        .builder.WithScope(XboxAuth.XboxScope) // set OAuth scope
        .builder.WithWebUI(new WebView2WebUI())) // set custom UI
        .builder.WithOAuthParameters(new MicrosoftOAuthParameters
        {
            // parameters
            Prompt = MicrosoftOAuthPromptModes.SelectAccount
        })
    .Build();
```

#### WtihScope(string scope)

Set Microsoft OAuth scope.

#### WithWebUI(IWebUI webUI)

Set custom interface.

#### WithOAuthParameters(MicrosoftOAuthParameters param)

Set OAuth parameters.

[parameters](https://github.com/AlphaBs/XboxAuthNet/blob/dev/XboxAuthNet/OAuth/MicrosoftOAuthParameters.cs)

For detail description of each properties, please reference [this](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-auth-code-flow#request-an-authorization-code)

### Example

[WinFormTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/blob/master/src/samples/WinFormTest/Form1.cs)