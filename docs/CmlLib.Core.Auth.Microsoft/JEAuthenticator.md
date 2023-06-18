# JEAuthenticator

Set the options for JE authentication.

## AddJEAuthenticator

```csharp
authenticator.AddJEAuthenticator();
```

The cached JE session is validated first, and if the session is expired or invalid, the JE authentication is attempted with the Xbox session.

## AddForceJEAuthenticator

```csharp
authenticator.AddForceJEAuthenticator();
```

Proceeds with the JE sign-in to the Xbox session, without validating the cached JE session.

## WithGameOwnershipChecker(bool value)

```csharp
authenticator.AddJEAuthenticator(je => je
    .WithGameOwnershipChecker(false)
    .Build());
```

Sets whether to check if the game is purchased and owned. 기본 값: `false`

*note: This method can only check if the game was purchased from the official site. It is recommended that you do not change the default value of `false` because if a user with a genuine Xbox GamePass account is checked with this method, it will be determined that they do not have an account.*

## JEAuthException

This exception is thrown if something went wrong while logging into Minecraft JE. The `ErrorType`, `Error`, and `ErrorMessage` properties provide detailed error information.

### NOT_FOUND

The user doesn't have the game. (Demo version)