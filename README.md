# CmlLib.Core.Auth.Microsoft

<img src='https://raw.githubusercontent.com/CmlLib/CmlLib.Core/master/icon.png' width=128>

[![Nuget Badge](https://img.shields.io/nuget/v/CmlLib.Core.Auth.Microsoft)](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft)
[![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/CmlLib/CmlLib.Core.Auth.Microsoft/blob/master/LICENSE)

[![Discord](https://img.shields.io/discord/795952027443527690?label=discord\&logo=discord\&style=for-the-badge)](https://discord.gg/cDW2pvwHSc)

<img src="https://user-images.githubusercontent.com/17783561/120596686-02b15980-c47f-11eb-96e9-4ea03f451352.png" width=500/>

Minecraft login with Microsoft Xbox account

## Features

- Multi Account Management
- Microsoft OAuth with WebView2
- Microsoft OAuth with [MSAL.NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet)
- Xbox Authentication
- Minecraft: JE Authentication
- Minecraft: BE Authentication
- Crossplatform (with MSAL.NET)

## Install

Install NuGet package: [CmlLib.Core.Auth.Microsoft](https://www.nuget.org/packages/CmlLib.Core.Auth.Microsoft)

## Documentation

[Documentation](https://alphabs.gitbook.io/cmllib/auth.microsoft/cmllib.core.auth.microsoft)

[한국어](https://alphabs.gitbook.io/cmllib/v/ko/auth.microsoft/cmllib.core.auth.microsoft)

## Example

[Sample Launcher](https://github.com/CmlLib/CmlLib-Minecraft-Launcher)

## Native AOT 검증

- XboxAuthNet.Game과 CmlLib.Core.Auth.Microsoft는 기존 `netstandard2.0` 자산을 유지하고 AOT 분석용 `net10.0` 자산을 추가합니다. 테스트 프로젝트도 .NET 10으로 실행합니다.
- 세션 JSON, Minecraft 토큰·프로필·스킨·망토와 JWT 페이로드에 생성 메타데이터를 사용합니다. 기본 JE 계정 관리자에는 Minecraft와 Xbox 세션 컨텍스트를 함께 전달합니다.
- 사용자 정의 세션 모델은 생성 컨텍스트를 `JsonSerializerOptions.TypeInfoResolver`로 전달할 수 있습니다. `XboxGameJson.CreateOptions`는 호출자 옵션과 resolver·converter를 보존하면서 기본 Xbox 모델을 추가합니다. `MicrosoftAuthJsonContext`와 `XboxGameJsonContext`도 공개하므로 외부에서 조합할 수 있습니다.
- JWT 사용자 정의 페이로드는 `JwtDecoder.DecodePayload(jwt, jsonTypeInfo)`를 사용할 수 있습니다. JIT에서는 기존 리플렉션 모델 처리도 유지합니다. 기능 guard 추론의 IL4000 한 곳만 이유를 명시해 억제하며, 실제 직렬화·리플렉션 호출의 IL2026·IL3050은 억제하지 않습니다.

### XboxAuthNet 의존성

- XboxAuthNet 자체의 AOT 개선은 [AlphaBs/XboxAuthNet PR #14](https://github.com/AlphaBs/XboxAuthNet/pull/14)에 있습니다. 기본 NuGet 의존성은 3.0.4를 유지하므로, 이 변경만으로 전체 인증 의존성이 AOT 호환되는 것은 아닙니다.
- 의존 PR의 병합·NuGet 릴리스 후에는 XboxAuthNet 패키지 참조도 새 버전으로 갱신해야 합니다. 검토·통합 검증을 위해 `XboxAuthNetSourceProject`에 해당 csproj의 절대 경로를 전달하면 NuGet 대신 소스를 참조합니다. 지정하지 않으면 기존 NuGet 참조를 사용합니다.
- CI는 XboxAuthNet의 검증한 커밋 `62816c9ed2cff770f951da0f24ffb1a39089b115`를 별도 checkout하고 같은 소스 참조 방식으로 검증합니다.

### 실행

.NET 10 SDK와 Native AOT 도구 체인이 필요합니다. 아래 명령의 경로는 AOT 개선 XboxAuthNet 소스의 실제 절대 경로로 바꿉니다. mock HTTP와 테스트가 만든 임시 파일만 사용하며 실제 계정 로그인은 하지 않습니다.

```powershell
$xboxProject = "C:/path/to/XboxAuthNet/src/XboxAuthNet.csproj"
dotnet build src/CmlLib.Core.Auth.Microsoft -c Release
dotnet test tests/XboxAuthNet.Game.Test -c Release
dotnet test tests/CmlLib.Core.Auth.Microsoft.Test -c Release
dotnet run --project smoke/NativeAotSmoke -c Release -p:XboxAuthNetSourceProject=$xboxProject
dotnet run --project smoke/NativeAotSmoke -c Release -p:XboxAuthNetSourceProject=$xboxProject -p:PublishAot=false -p:JsonSerializerIsReflectionEnabledByDefault=true
dotnet publish smoke/NativeAotSmoke -c Release -r win-x64 -p:XboxAuthNetSourceProject=$xboxProject -o artifacts/native-smoke
./artifacts/native-smoke/NativeAotSmoke.exe
```
