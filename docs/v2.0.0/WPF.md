# CmlLib.Core.Auth.Microsoft.UI.Wpf

Provides Wpf Window that display Microsoft OAuth login page and create the Minecraft session.

## Install

Install Nuget package:
- [CmlLib.Core.Auth.Microsoft.UI.Wpf](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.UI.Wpf)

## WebView2

**Important!** All users (including end-user) should install WebView2 runtime.
- [WebView2 runtime download page](https://developer.microsoft.com/en-us/microsoft-edge/webview2/#download-section)
- [WebView2 runtime evergreen installer direct download link](https://go.microsoft.com/fwlink/p/?LinkId=2124703)


## Usage

```csharp
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;
```

### Login

```csharp
MicrosoftLoginWindow loginWindow = new MicrosoftLoginWindow();
MSession session = await loginWindow.ShowLoginDialog();
MessageBox.Show("Login success : " + session.Username);
```

### Logout

```csharp
MicrosoftLoginWindow loginForm = new MicrosoftLoginWindow();
loginWindow.ShowLogoutDialog();
```

### Example

[WpfTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/blob/master/src/samples/WpfTest/MainWindow.xaml.cs)

## UI Customization

[UI Customization](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/wiki/UI-Customization)