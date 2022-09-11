# CmlLib.Core.Auth.Microsoft.UI.WinForm

Provides WinForm Form that display Microsoft OAuth login page and create the Minecraft session.

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
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
```

### Login

```csharp
MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
MSession session = await loginForm.ShowLoginDialog();
MessageBox.Show("Login success : " + session.Username);
```

### Logout

```csharp
MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
loginForm.ShowLogoutDialog();
```

### Example

[WinFormTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/blob/master/src/samples/WinFormTest/Form1.cs)

## UI Customization

[UI Customization](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/wiki/UI-Customization)