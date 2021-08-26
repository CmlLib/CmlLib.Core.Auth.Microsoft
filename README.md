# CmlLib.Core.Auth.Microsoft.UI
<img src="https://user-images.githubusercontent.com/17783561/120596686-02b15980-c47f-11eb-96e9-4ea03f451352.png" width=500/>

UI library for minecraft login with Microsoft Xbox account

## Install
Install nuget package  

WinForm : [CmlLib.Core.Auth.Microsoft.UI.WinForm](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.UI.WinForm/)  
Wpf : [CmlLib.Core.Auth.Microsoft.UI.Wpf](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft.UI.Wpf/)

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

## UI Messages

### Loading Text

```csharp
// WinForm
var loginForm = new MicrosoftLoginForm();
loginForm.LoadingText = "Loading Loading Loading";

// WPF
var loginWindow = new MicrosoftLoginWindow();
loginWindow.LoadingText = "Hello Loading Wait plz";
```

### Error Messages

```csharp
// WinForm
var login = new MicrosoftLoginForm();

// WPF
var login = new MicrosoftLoginWindow();

// default
login.MessageStrings = new Dictionary<string, string>
{
    ["mslogin_fail"] = "Failed to microsoft login",
    ["mclogin_fail"] = "Failed to minecraft login",
    ["xbox_error_child"] = "Your account seems like a child. Verify your age or add your account into a Family.",
    ["xbox_error_noaccount"] = "Your account doens't have an Xbox account",
    ["mojang_nogame"] = "You don't have a Minecraft JE",
    ["empty_token"] = "Token was empty",
    ["empty_userhash"] = "UserHash was empty",
    ["no_error_msg"] = "No error message"
}

// localize (korean)
login.MessageStrings = new Dictionary<string, string>
{
    ["mslogin_fail"] = "마이크로소프트 로그인 실패",
    ["mclogin_fail"] = "마인크래프트 로그인 실패",
    ["xbox_error_child"] = "미성년자 계정인 것 같습니다. 성인인증을 하거나 가족 계정으로 추가하세요",
    ["xbox_error_noaccount"] = "Xbox 계정이 존재하지 않습니다",
    ["mojang_nogame"] = "마인크래프트 JE 를 구매하지 않았습니다",
    ["empty_token"] = "Token 이 빈 값입니다",
    ["empty_userhash"] = "UserHash 이 빈 값입니다",
    ["no_error_msg"] = "에러 메세지가 없습니다"
}
```