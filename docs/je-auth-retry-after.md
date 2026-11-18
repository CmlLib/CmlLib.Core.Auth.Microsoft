# Minecraft Java 인증의 Retry-After

Snowfrost는 Minecraft 프로필 조회가 429로 실패하면 서버가 준 대기 시간을 표시한다. CmlLib 3.5.0이 응답을 `JEAuthException`으로 바꾸면서 헤더를 버려서 런처에서 HTTP 핸들러로 헤더를 읽어야 했다. CmlLib에서 헤더를 보존해 달라는 요청에 따라 예외 변환 단계에서 `Retry-After`를 기록한다.

Minecraft Java 프로필 조회와 토큰 발급에서 발생한 `JEAuthException`의 `RetryAfter` 속성으로 상대 시간(`Delta`)이나 절대 시각(`Date`)을 읽을 수 있다. 헤더가 없거나 형식이 잘못되면 `null`이다. 재시도 여부와 사용자 안내는 호출 측에서 결정한다.

검토 후 변경 범위를 헤더 보존으로 한정했다. 초기 제안에 포함됐던 오류 본문 검증과 상태 코드 처리 변경은 제외했다.

이 변경은 `CmlLib.Core.Auth.Microsoft` 3.5.1에 포함한다. 기존 패키지 의존성을 유지하고, GitHub 릴리스에 해당 `.nupkg`를 첨부한다.
