# CmlLib.Core.Auth.Microsoft.UI
<img src="https://user-images.githubusercontent.com/17783561/120596686-02b15980-c47f-11eb-96e9-4ea03f451352.png" width=500/>

UI library for minecraft login with Microsoft Xbox account

## Install
Install nuget package [CmlLib.Core.Auth.Microsoft.UI](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.UI/)

## [Microsoft.Web.WebView2](https://docs.microsoft.com/en-us/microsoft-edge/webview2/)

**Important!!!** All users (including end-user) using this library should install [WebView2 runtime](https://go.microsoft.com/fwlink/p/?LinkId=2124703)

## WinForm

#### Import namespaces
```csharp
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.WinForm;
```

#### Login
```csharp
MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
MSession session = loginForm.ShowLoginDialog();
if (session != null)
  MessageBox.Show("Login success : " + session.Username);
else
  MessageBox.show("Failed to login");
```

#### Logout
```csharp
MicrosoftLoginForm loginForm = new MicrosoftLoginForm();
loginForm.ShowLogoutDialog();
```

#### Example

[WinFormTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/blob/master/WinFormTest/Form1.cs)

## WPF

#### Import namespaces
```csharp
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;
```

#### Login
```csharp
MicrosoftLoginWindow loginWindow = new MicrosoftLoginWindow();
MSession session = loginWindow.ShowLoginDialog();
if (session != null)
  MessageBox.Show("Login success : " + session.Username);
else
  MessageBox.show("Failed to login");
```

#### Logout
```csharp
MicrosoftLoginWindow loginForm = new MicrosoftLoginWindow();
loginWindow.ShowLogoutDialog();
```

#### Example

[WpfTest project](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft.UI/blob/master/WpfTest/MainWindow.xaml.cs)
