# Accounts

## ISessionStorage

Sessions are stored in `ISessionStorage`, along with the various tokens obtained during the login process. One account is stored in one instance of `ISessionStorage`. For example, when a user named `Notch` logs in, they get a Microsoft OAuth token, an Xbox token, and a Minecraft JE token. All three would be stored in a single instance of `ISessionStorage`, which would only contain login information specific to the user `Notch`.  

There are three implementations of `ISessionStorage`: `InMemorySessionStorage`, which stores all information in memory; `JsonSessionStorage`, which manages it as an in-memory Json object; and `JsonFileSessionStorage`, which manages it as a Json file.

### Example

```csharp
var sessionStorage = new InMemorySessionStorage();

// save data
sessionStorage.Set<string>("myData", "HelloWorld");

// load data
var myData = sessionStorage.Get<string>("myData");

// save and load data via ISessionSource
var sessionSource = MicrosoftOAuthSessionSource.Default;
sessionSource.Set(sessionStorage, new MicrosoftOAuthResponse());
var oauth = sessionSource.Get(sessionStorage);
```

## XboxGameAccount

XboxGameAccount has an ISessionStorage internally and provides additional functionality.

- Provides an identifier to distinguish between ISessionStorages.
- Provides properties to easily access the session information held by the ISessionStorage (e.g. LastAccess, XboxTokens).

### Identifier

To manage multiple accounts, you need to manage multiple ISessionStorages, and you need an identifier to distinguish each ISessionStorage. 

If two accounts have the same identifier, they are considered to be the same account, even if the ISessionStorage contains different data. 

For example, `JEGameAccount`, which represents a Minecraft Java Edition account, uses the user's UUID as the identifier.

### LastAccess

Indicates the last time this account was accessed.

### XboxTokens
