# OAuth

Describes about Microsoft OAuth.

## Usage

Add `Authenticator` through the extension methods of `ICompositeAuthenticator`. 

```csharp
using XboxAuthNet.Game;

var clientInfo = new MicrosoftOAuthClientInfo("<MICROSOFT_OAUTH_CLIENT_ID>", "<MICROSOFT_OAUTH_SCOPES>");

// example 1
authenticator.AddForceMicrosoftOAuth(clientInfo, oauth => oauth.Interactive());

// example 2
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Silent());
```

## AddMicrosoftOAuth / AddForceMicrosoftOAuth

`AddMicrosoftOAuth` validates the cached Microsoft OAuth session and, if the session is valid, doesn't proceed authentication and moves on to the next authenticator.

The Force method does not validate the Microsoft OAuth session and proceeds authentication unconditionally.

## Setting OAuth Mode

### Interactive

```csharp
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Interactive());
```

```csharp
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Interactive(new MicrosoftOAuthParameters
{
    // OAuth setting
    // example: set prompt mode
    Prompt = MicrosoftOAuthPromptModes.SelectAccount
}));
```

<img src="https://user-images.githubusercontent.com/17783561/154854388-38c473f1-7860-4a47-bdbe-622de37eef8b.png" width="500">

A window will pop up prompting the user to enter the email and password for their Microsoft account and proceed to sign in.

### Silent

```csharp
authenticator.AddMicrosoftOAuth(clientInfo, oauth => oauth.Silent());
```

Proceed with the login without prompting the user for a login. If the cached session hasn't expired, the token will be used; if it has, it will attempt to refresh it. If the refresh fails, a `MicrosoftOAuthException` exception is thrown.

### Signout

1.
```csharp
authenticator.AddMicrosoftOAuthSignout(clientInfo);
```

2. 
```csharp
authenticator.AddForceMicrosoftOAuth(clientInfo, oauth => oauth.Signout(codeFlow =>
{
    // set more options like UI title, UI parents, etc... 
    codeFlow.WithUITitle("My Window");
}));
```

Displays the OAuth sign out page and clears the session.

### Signout without UI

Clears only cached OAuth sessions. The browser on user may still have user's login information. 
