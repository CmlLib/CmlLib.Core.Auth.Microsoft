# JELoginHandlerBuilder

[JELoginHandler](./JELoginHandler.md) 객체를 만드는 방법을 제공합니다. 

```csharp
var loginHandler = new JELoginHandlerBuilder()
    .WithHttpClient(httpClient)
    .WithAccountManager("accounts.json")
    //.WithAccountManager(new InMemoryXboxGameAccountManager(JEGameAccount.FromSessionStorage))
    .Build()
var session = await loginHandler.Authenticate();
```

### WithHttpClient

HTTP 요청에 사용될 `HttpClient` 객체를 설정합니다. 모든 HTTP 요청은 여기서 설정한 `HttpClient` 를 통해 이루어집니다. 

### WithAccountManager

`JELoginHandler` 에서 사용할 `IXboxGameAccountManager` 객체를 설정합니다. 기본값은 `<마인크래프트_폴더>/cml_accounts.json` 파일을 사용하는 `JsonXboxGameAccountManager` 객체입니다. 

`string` 인수를 넘겨준 경우 `WithAccountManager(new JsonXboxGameAccountManager(filePath, JEGameAccount.FromSessionStorage))` 를 호출합니다. 

### WithLogger

로깅 기능을 위한 [ILogger]() 설정을 합니다. 