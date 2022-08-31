# CmlLib.Core.Auth.Microsoft.MsalClient
Provides wrapper class for minecraft login with MSAL.NET library.  
All Microsoft OAuth process is handled by MSAL.NET. You can authenticate with three ways: embedded webview, system browser, device code.  

## Install

Install Nuget package:
- [CmlLib.Core.Auth.Microsoft.MsalClient](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.MsalClient)

## Usage

### Prepare

Before to use MsalClient, You should acquire your own ClientID.
[Create Client ID for MsalClient](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/wiki/Create-Client-ID-for-MsalClient)

Initialize MSAL application:

```csharp
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;

var app = await MsalMinecraftLoginHelper.BuildApplicationWithCache("CLIENT-ID");
```

### Example

(link)

### LoginInteractive

MSAL.NET will open system web browser or open embedded webview to process Microsoft OAuth.

```csharp
var loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMsalOAuth(app, factory => factory.CreateInteractiveApi())
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

System web browser mode:

<img src="https://user-images.githubusercontent.com/17783561/154945056-2f0d961b-f69b-4cea-a08a-9c3b050995f6.png" width="500">  

### LoginEmbeddedWebView

```csharp
var loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMsalOAuth(app, factory => factory.CreateWithEmbeddedWebView())
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

Embedded WebView mode:

<img src="https://user-images.githubusercontent.com/17783561/154946636-960d3673-bb51-4f3a-ae92-f36940b8e3ad.png" width="500">

### LoginDeviceToken

If you don't want to use web browser in launcher, you can use DeviceCodeFlow. DeviceCodeFlow allows users to use another device to login.  

```csharp
var loginHandler = new LoginHandlerBuilder()
    .ForJavaEdition()
    .WithMsalOAuth(app, factory => factory.CreateDeviceCodeApi(result =>
    {
        Console.WriteLine($"Code: {result.UserCode}, ExpiresOn: {result.ExpiresOn.LocalDateTime}");
        Console.WriteLine(result.Message);
        return Task.CompletedTask;
    }))
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

WinForm:  

<img src="https://user-images.githubusercontent.com/17783561/154950501-4ffbd21f-b780-4217-bd83-641ae3ac5e95.png" width="500">

Console:  

<img src="https://user-images.githubusercontent.com/17783561/154950743-823d5ecf-b303-4caf-a9cc-a1167007dd7c.png" width="500">

## WinForm Example Project

[MsalClientTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/blob/master/src/samples/MsalClientTest/Form1.cs)

<img src="https://user-images.githubusercontent.com/17783561/154950992-70c40ca5-fd5f-4058-b2cb-269ee0b6d537.png" width="500">

