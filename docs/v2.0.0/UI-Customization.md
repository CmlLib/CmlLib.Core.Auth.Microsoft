# UI Customization

## WebView2

### Set `CoreWebView2Environment`
```csharp
var login = new MicrosoftLoginForm(); // or new MicrosoftLoginWindow();
login.WebView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: "mydatafolder");
```

### Override `InitializeWebView2`
```csharp
class MyLoginForm : MicrosoftLoginForm
{
    // This method is called after WebView2 instance is added to form
    protected override Task InitializeWebView2(WebView2 wv)
    {
        return base.InitializeWebView2(wv);
    }
}

var login = new MyLoginForm();
```

## Texts

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

*note: you can't set error messages after v2.0.0. all error message will be thrown as exception*

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

## Exceptions

*note: you can't override this after v2.0.0, since LoginForm and LoginWindow does not handle exception anymore*

Override `OnException`
```csharp
class MyLoginForm : MicrosoftLoginForm
{
    // This is called when Exception is thrown
    protected override bool OnException(Exception ex)
    {
        return base.OnException(ex);
    }
}
```