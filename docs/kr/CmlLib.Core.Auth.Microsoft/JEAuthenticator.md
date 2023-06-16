# JEAuthenticator

JE 인증 시 옵션을 지정할 수 있습니다. 

## Example

```csharp
var session = await loginHandler.AuthenticateInteractively()
    .WithJEAuthenticator(builder => builder
        .WithCache(true)
        .WithSessionSource(new InMemorySessionSource())
        .WithSessionStorage(new InMemorySessionStorage())
        .WithCheckingGameOwnership(false)
        .WithSilentAuthenticator(true))
    .ExecuteForLauncherAsync();
```

## WithCache(bool value)

로그인 결과를 캐시에 저장할 지 설정합니다. 기본 값: `true`

## WithSessionSource(ISessionSource<JESession> sessionSource)

로그인 결과를 `sessionSource` 에 저장합니다. 

## WithSessionStorage(ISessionStorage sessionStorage)

로그인 결과를 `sessionStorage` 에 저장합니다. 기본 키 이름은 `JESession` 입니다. 

기본 값: `loginHandler` 에서 설정한 `account` 의 `SessionStorage`

## WithCheckingGameOwnership(bool value)

게임을 구매하여 소유하고 있는 지 검사하는 여부를 설정합니다. 기본 값: `false`

*note: 이 메서드는 공식 사이트에서 게임을 구매했는지만 검사할 수 있습니다. 엑스박스 게임패스로 정품 계정을 가지고 있는 유저를 이 메서드로 검사하면 계정을 가지고 있지 않는 것으로 판단하기 때문에 기본값 `false` 를 바꾸지 않는 것을 추천합니다.*

## WithSilentAuthenticator(bool value)

`true` 로 설정하면 먼저 캐시된 토큰을 검사해 토큰이 만료된 경우에만 인증을 진행합니다. 토큰이 만료되지 않은 경우라면 캐시된 토큰을 즉시 반환합니다. 

`AuthenticateSilently()` 메서드를 호출한 경우 기본 값은 `true`, `AuthenticateInteractively()` 메서드를 호출한 경우 기본 값은 `false` 입니다. 

## JEAuthException

마인크래프트 JE 로그인 도중 문제가 발생한 경우 이 예외가 발생합니다. `ErrorType`, `Error`, `ErrorMessage` 속성으로 자세한 에러 내용을 확인할 수 있습니다. 

### NOT_FOUND

유저가 게임을 가지고 있지 않은 경우. (데모 버전) 