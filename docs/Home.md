# Home

Welcome to the CmlLib.Core.Auth.Microsoft.UI wiki!

This is library for new minecraft login feature which uses Microsoft Xbox account instead of classic mojang account.
It provides two library that authenticate in different ways: `CmlLib.Core.Auth.Microsoft.UI.WinForm`, and `CmlLib.Core.Auth.Microsoft.MsalClient`.

## CmlLib.Core.Auth.Microsoft

This library has common features of other libraries.  
You can extend this library to make your own login flows. 

## [CmlLib.Core.Auth.Microsoft.UI.WinForm](WinForm.md)

WinForm library for minecraft login. Using embeded WebView2 It shows Windows Form displaying Microsoft OAuth web page.

<img src="https://user-images.githubusercontent.com/17783561/154854388-38c473f1-7860-4a47-bdbe-622de37eef8b.png" width="500">

- Integration with WinForm
- No Client ID required (Mojang's Client ID can be used)
- WebView2 runtime required (end-user should install WebView2 runtime)
- Only Windows 7+ supported

This library also works with the WPF app. 

## [CmlLib.Core.Auth.Microsoft.MsalClient](MsalClient.md)

Provides wrapper class for minecraft login with MSAL.NET library.
All Microsoft OAuth process is handled by MSAL.NET. You can authenticate with embedded webview, system browser, or DeviceCode.

<img src="https://user-images.githubusercontent.com/17783561/154854531-8d4557b2-9221-4021-9fa0-93d6c8fd2497.png" width="500">

<img src="https://user-images.githubusercontent.com/17783561/154854603-abb98727-543b-45e4-b4ea-ff6bd772aa67.png" width="500">

- Support all platform (Windows, Linux, macOS)
- No additional runtime required (no WebView2 runtime required)
- Client ID required (All developer should obtain their own Client ID)