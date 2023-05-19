# XboxAuthStrategies

Xbox 인증을 진행하는 방법을 나타냅니다.

## BasicXboxAuthStrategy

가장 기본적인 방식입니다. 로그인에 필요한 최소 정보(UserToken, XstsToken)만 받아옵니다.

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMicrosoftOAuth(builder => builder
        .MicrosoftOAuth.UseInteractiveStrategy()
        .XboxAuth.UseBasicStrategy())
    .ExecuteForLauncherAsync();
```

## FullXboxAuthStrategy

UserToken, DeviceToken, XstsToken 를 받아옵니다. 

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMicrosoftOAuth(builder => builder
        .MicrosoftOAuth.UseInteractiveStrategy()
        .XboxAuth.UseFullStrategy())
    .ExecuteForLauncherAsync();
```

## SisuXboxAuthStrategy

SISU 로그인 방식을 사용합니다. UserToken, DeviceToken, TitleToken, XstsToken 모든 토큰을 받아옵니다. 

이 방식은 ClientId 이 XboxGameTitles.MinecraftJE 인 경우에만 작동합니다. [MSAL](../XboxAuthNet.Game.Msal/Home.md) 와 함께 사용할 수 없습니다. 

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMicrosoftOAuth(builder => builder
        .MicrosoftOAuth.UseInteractiveStrategy()
        .XboxAuth.UseSisuStrategy(XboxGameTitles.MinecraftJava))
    .ExecuteForLauncherAsync();
```

## Device Options

DeviceToken 을 받아오는 로그인 방식을 사용할 때, Device 설정을 적용할 수 있습니다. 

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithMicrosoftOAuth(builder => builder
        .MicrosoftOAuth.UseInteractiveStrategy()
        .XboxAuth.WithDeviceType(XboxDeviceTypes.Win32)
        .XboxAuth.WithDeviceVersion("0.0.0")
        .XboxAuth.UseFullStrategy())
    .ExecuteForLauncherAsync();
```

## 오류 처리

Xbox 인증 시 다양한 오류 시나리오가 있습니다. 인증 시 오류가 발생하면 `XboxAuthException` 이 발생하며 ErrorCode 와 ErrorMessage 를 얻을 수 있습니다.

모든 ErrorCode 는 [여기서](./XboxAuthErrors.md) 확인할 수 있습니다. 

```csharp
try
{
    var session = await loginHandler.AuthenticateInteractively()
        .WithMicrosoftOAuth(builder => builder
            .MicrosoftOAuth.UseInteractiveStrategy()
            .XboxAuth.UseFullStrategy())
        .ExecuteForLauncherAsync();
}
catch (XboxAuthException ex)
{
    Console.WriteLine(ex.ErrorCode);
    Console.WriteLine(ex.ErrorMessage);
}
```

