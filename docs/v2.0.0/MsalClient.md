# CmlLib.Core.Auth.Microsoft.MsalClient
Provides wrapper class for minecraft login with MSAL.NET library.  
All Microsoft OAuth process is handled by MSAL.NET. You can authenticate with three ways: embedded webview, system browser, device code.  

## Install

Install Nuget package:
- [CmlLib.Core.Auth.Microsoft.MsalClient](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.MsalClient)

## Usage

### Example
Before to use MsalClient, You should acquire your own ClientID.
[Create Client ID for MsalClient](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/wiki/Create-Client-ID-for-MsalClient)

```csharp
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.MsalClient;
using Microsoft.Identity.Client;

// Session variable to store minecraft login result
MSession? session = null;

// Create IPublicClientApplication
// https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/wiki/Create-Client-ID-for-MsalClient
var app = MsalMinecraftLoginHelper.CreateDefaultApplicationBuilder("CLIENT-ID")
    .Build();

// Initialize MsalMinecraftLoginHelper
var handler = new MsalMinecraftLoginHandler(app);

try
{
    // Try login with cached session
    Console.WriteLine("Start LoginSilent");
    session = await handler.LoginSilent();
}
catch (MsalUiRequiredException)
{
    // Choose login method
    Console.WriteLine("Choose login method: ");
    Console.WriteLine("1. DeviceCode   2. WebBrowser   3. EmbeddedWebView");
    int.TryParse(Console.ReadLine(), out int loginMode);

    if (loginMode == 1)
    {
        // Login with DeviceCode
        Console.WriteLine("Start LoginDeviceCode");
        session = await handler.LoginDeviceCode(result =>
        {
            Console.WriteLine($"Code: {result.UserCode}, ExpiresOn: {result.ExpiresOn.LocalDateTime}");
            Console.WriteLine(result.Message);
            return Task.CompletedTask;
        });
    }
    else if (loginMode == 2)
    {
        // Login with WebBrowser
        Console.WriteLine("Start LoginInteraction");
        session = await handler.LoginInteractive();
    }
    else if (loginMode == 3)
    {
        // Login with EmbeddedWebView
        Console.WriteLine("Start LoginInteraction(useEmbeddedWebView: true)");
        session = await handler.LoginInteractive(useEmbeddedWebView: true);
    }
    else
    {
        return;
    }
}

// Use 'session' variable
Console.WriteLine("Login Success: " + session.Username);
Console.ReadLine();
```

### Logout

```csharp
// Logout
await handler.RemoveAccounts();
```

### LoginSilent

Login with cached session. If cached session is invalid or expired, it throws `MsalUiRequiredException`.  
```csharp
MSession session = await handler.LoginSilent();
```

### LoginInteractive

Login interactively using web browser.  
```csharp
MSession session = await handler.LoginInteractive(useEmbeddedWebView: false);
```
If `useEmbeddedWebView` is `false`, it open system default web browser and request user to login.  
Example:  

<img src="https://user-images.githubusercontent.com/17783561/154945056-2f0d961b-f69b-4cea-a08a-9c3b050995f6.png" width="500">  

If `useEmbeddedWebView` is `true`, it uses `WebView2`.  
To use this mode, Your project is should targeting classic .NET Framework or `net5.0-windows`.  
Example:  

<img src="https://user-images.githubusercontent.com/17783561/154946636-960d3673-bb51-4f3a-ae92-f36940b8e3ad.png" width="500">

### LoginDeviceToken

If you don't want to use web browser in launcher, you can use DeviceCodeFlow. DeviceCodeFlow allows users to use another device to login.  
```csharp
MSession session = await handler.LoginDeviceToken(result => 
{
    // result.UserCode : code to authenticate
    // result.ExpiresOn : expire time  
    Console.WriteLine(result.Message);
});
```

WinForm:  

<img src="https://user-images.githubusercontent.com/17783561/154950501-4ffbd21f-b780-4217-bd83-641ae3ac5e95.png" width="500">

Console:  

<img src="https://user-images.githubusercontent.com/17783561/154950743-823d5ecf-b303-4caf-a9cc-a1167007dd7c.png" width="500">

## WinForm Example Project

[MsalClientTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/blob/master/src/samples/MsalClientTest/Form1.cs)

<img src="https://user-images.githubusercontent.com/17783561/154950992-70c40ca5-fd5f-4058-b2cb-269ee0b6d537.png" width="500">

