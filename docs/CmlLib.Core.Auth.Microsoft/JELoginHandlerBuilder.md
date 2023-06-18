# JELoginHandlerBuilder

Builder for initializing [JELoginHandler](./JELoginHandler.md).

```csharp
var loginHandler = new JELoginHandlerBuilder()
    .WithHttpClient(httpClient)
    .WithAccountManager("accounts.json")
    //.WithAccountManager(new InMemoryXboxGameAccountManager(JEGameAccount.FromSessionStorage))
    .Build();
var session = await loginHandler.Authenticate();
```

### WithHttpClient

Set `HttpClient`. All HTTP requests are handled by this.

### WithAccountManager

Set `IXboxGameAccount` which `JELoginHandler` will use. Default is `JsonXboxGameAccountManager` with the `<MINECRAFT-PATH>/cml_accounts.json` file.

If you pass a string type, this method will call `WithAccountManager(new JsonXboxGameAccountManager(filePath, JEGameAccount.FromSessionStorage))`.

### WithLogger

Set [ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-7.0) for logging. This library use [Microsoft.Extensions.Logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line) to logging.