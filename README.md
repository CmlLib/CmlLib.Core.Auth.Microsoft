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


## Native AOT

- XboxAuthNet.Game과 CmlLib.Core.Auth.Microsoft는 기존 `netstandard2.0` 자산을 유지하고 AOT 분석용 `net10.0` 자산을 제공합니다.
- 세션 JSON, Minecraft 토큰·프로필·스킨·망토와 JWT 페이로드에 생성 메타데이터를 사용합니다. 기본 JE 계정 관리자에는 Minecraft와 Xbox 세션 컨텍스트를 함께 전달합니다.
- 사용자 정의 세션 모델은 생성 컨텍스트를 `JsonSerializerOptions.TypeInfoResolver`로 전달할 수 있습니다. `XboxGameJson.CreateOptions`는 호출자 옵션과 resolver·converter를 보존하면서 기본 Xbox 모델을 추가합니다. `MicrosoftAuthJsonContext`와 `XboxGameJsonContext`도 공개하므로 외부에서 조합할 수 있습니다.
- JWT 사용자 정의 페이로드는 `JwtDecoder.DecodePayload(jwt, jsonTypeInfo)`를 사용할 수 있습니다. JIT에서는 기존 리플렉션 모델 처리도 유지합니다. 기능 guard 추론의 IL4000 한 곳만 이유를 명시해 억제하며, 실제 직렬화·리플렉션 호출의 IL2026·IL3050은 억제하지 않습니다.
- XboxAuthNet 자체의 AOT 개선은 [AlphaBs/XboxAuthNet PR #14](https://github.com/AlphaBs/XboxAuthNet/pull/14)에 있습니다. 기본 NuGet 의존성은 3.0.4를 유지하므로, 해당 개선 버전의 병합·릴리스 후 패키지 참조를 갱신해야 전체 인증 의존성의 AOT 호환성을 제공할 수 있습니다.
