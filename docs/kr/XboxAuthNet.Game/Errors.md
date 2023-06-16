# XboxAuthException

엑스박스 로그인 중 발생한 오류를 나타냅니다.

`ErrorCode` 와 `ErrorMessage` 속성으로 자세한 오류 내용을 알 수 있습니다. 

**마인크래프트를 구매한 계정이라면 아래 대부분의 오류는 발생하지 않습니다. 로그인을 시도하고 있는 계정이 마인크래프트를 구매한 계정이 맞는지 확인하세요.**

나이 관련 문제는 로그인 모드를 `Full` 이나 `Sisu` 로 바꾸어 문제를 해결할 수 있습니다. [자세히](./XboxAuth.md)

## ErrorCode

자주 발생하는 오류들의 `ErrorCode` 를 모아두었습니다. 

### 0x8015dc03

The device or user was banned.

### 0x8015dc04

The device or user was banned.

### 0x8015dc0b

This resource is not available in the country associated with the user.

### 0x8015dc0c

Access to this resource requires age verification. 

### 0x8015dc0d

Access to this resource requires age verification.

### 0x8015dc0e

ACCOUNT_CHILD_NOT_IN_FAMILY

가족 그룹에 추가되지 않은 미성년자 계정으로 로그인한 경우입니다. 

### 0x8015dc09

ACCOUNT_CREATION_REQUIRED

엑스박스 계정을 만들지 않은 계정으로 로그인한 경우입니다. 

### 0x8015dc10

ACCOUNT_MAINTENANCE_REQUIRED

### Others

모든 에러 코드: [here](https://github.com/microsoft/xbox-live-api/blob/730f579d41b64df5b57b52e629d12f23c6fb64ac/Source/Shared/errors_legacy.h#L924)
