# Minecraft Java 인증의 Retry-After

Snowfrost는 Minecraft 프로필 조회가 429로 실패하면 서버가 준 대기 시간을 표시한다. CmlLib 3.5.0이 응답을 `JEAuthException`으로 바꾸면서 헤더를 버려서 런처에서 HTTP 핸들러로 헤더를 읽어야 했다. CmlLib에서 헤더를 보존해 달라는 요청에 따라 예외 변환 단계에서 `Retry-After`를 기록한다.

Minecraft Java 프로필 조회와 토큰 발급에서 발생한 `JEAuthException`의 `RetryAfter` 속성으로 상대 시간(`Delta`)이나 절대 시각(`Date`)을 읽을 수 있다. 헤더가 없거나 형식이 잘못되면 `null`이다. 재시도 여부와 사용자 안내는 호출 측에서 결정한다.

검토 후 변경 범위를 헤더 보존으로 한정했다. 초기 제안에 포함됐던 오류 본문 검증과 상태 코드 처리 변경은 제외했다.

이 변경은 `CmlLib.Core.Auth.Microsoft` 3.5.1에 포함한다. 기존 패키지 의존성을 유지하고, GitHub 릴리스에 해당 `.nupkg`를 첨부한다.

3.5.1로 프로필 조회를 재현한 결과, 빈 본문의 HTTP 429 응답에서 `RetryAfter`는 보존되지만 `StatusCode`는 0이었다. 후속 수정에서는 본문이 비어 있거나 Minecraft 오류 형식에 맞지 않아도 `JEAuthException.StatusCode`에 실제 HTTP 상태 코드를 담는다. 이를 통해 런처가 헤더가 없는 응답에서도 `StatusCode == 429`로 판단할 수 있다.

JSON 파싱의 `JsonException`과 객체가 아닌 JSON에서 속성을 조회할 때 발생하는 `InvalidOperationException`은 같은 `catch`에서 `FormatException`으로 처리한다. 필드는 기존대로 각각 검사한다. 예를 들어 `error`가 숫자여도 문자열인 `errorType`을 읽을 수 있다. 필드별 검사를 없애면 정상인 다른 필드까지 버려지므로 기존 검사를 유지한다.

HTTP 상태 코드 보존 수정은 3.5.2에 포함한다. 기존 패키지 의존성을 유지하고 GitHub 릴리스에 `.nupkg`를 첨부한다.
