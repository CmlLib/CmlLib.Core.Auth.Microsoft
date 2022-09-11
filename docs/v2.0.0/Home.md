Welcome to the CmlLib.Core.Auth.Microsoft.UI wiki!

This is library for new minecraft login feature which uses Microsoft Xbox account instead of classic mojang account.  
It provides two library that authenticate in different ways: **CmlLib.Core.Auth.Microsoft.UI.\***, **CmlLib.Core.Auth.Microsoft.MsalClient**.

## CmlLib.Core.Auth.Microsoft
Common library for all another library.  
Provides common login logic, session caching and error handling.  

## CmlLib.Core.Auth.Microsoft.UI
<img src="https://user-images.githubusercontent.com/17783561/154854388-38c473f1-7860-4a47-bdbe-622de37eef8b.png" width="500">

UI library for minecraft login with microsoft xbox account.  
It shows Form/Window displaying Microsoft OAuth web page using WebView2.  
- Integration with WinForm/WPF
- No Client ID required (Mojang's Client ID is used)
- WebView2 runtime required (end-user should install WebView2 runtime)
- Only Windows 7+ supported

#### [CmlLib.Core.Auth.Microsoft.UI.WinForm](WinForm)
Provides WinForm [Form]() that display Microsoft OAuth login page and create the Minecraft session.

#### [CmlLib.Core.Auth.Microsoft.UI.Wpf](Wpf)
Provides Wpf [Window]() that display Microsoft OAuth login page and create the Minecraft session.

## [CmlLib.Core.Auth.Microsoft.MsalClient](MsalClient)
<img src="https://user-images.githubusercontent.com/17783561/154854531-8d4557b2-9221-4021-9fa0-93d6c8fd2497.png" width="500">

<img src="https://user-images.githubusercontent.com/17783561/154854603-abb98727-543b-45e4-b4ea-ff6bd772aa67.png" width="500">

Provides wrapper class for minecraft login with [MSAL.NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) library.  
All Microsoft OAuth process is handled by MSAL.NET. You can authenticate with embedded webview, system browser, or DeviceCode.
- Support all platform (Windows, Linux, macOS)
- No additional runtime required (no WebView2 runtime required)
- Client ID required (All developer should obtain their own Client ID)