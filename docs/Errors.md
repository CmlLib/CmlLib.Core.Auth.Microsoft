# Errors

## MicrosoftOAuthException

When Microsoft OAuth fails.

## LoginCancelledException

When the user cancel the login or get the signal from `CancellationToken`.

## XboxAuthException

The error code is set in `Error` property.

#### 0x8015dc03

The device or user was banned.

#### 0x8015dc04

The device or user was banned.

#### 0x8015dc0b

This resource is not available in the country associated with the user.

#### 0x8015dc0c

Access to this resource requires age verification. [Solution](XboxLiveApi.md)

#### 0x8015dc0d

Access to this resource requires age verification. [Solution](XboxLiveApi.md)

#### 0x8015dc0e

ACCOUNT_CHILD_NOT_IN_FAMILY [Solution](XboxLiveApi.md)

#### 0x8015dc09

ACCOUNT_CREATION_REQUIRED

#### 0x8015dc10

ACCOUNT_MAINTENANCE_REQUIRED

All error codes: [here](https://github.com/microsoft/xbox-live-api/blob/730f579d41b64df5b57b52e629d12f23c6fb64ac/Source/Shared/errors_legacy.h#L924)

## MinecraftAuthException

#### NOT_FOUND

User does not have the game. (demo version)