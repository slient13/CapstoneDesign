## 개요
- 플레이 관련된 정보를 관리하는 곳.
- 단순 플레이 정보 불러오기, 변경, 열람 제공.
- 타 정보에 영향을 받는 정보 최신화.

## 기능
- 플레이 정보 등록, 저장, 불러오기.
- 플레이 정보 변경 : 특정 정보는 지정한 범위 내로 변화됨.
- 플레이 정보 열람.

## 이용 방법
- Message 객체 이용.

## 인터페이스

```c#
public void SavePlayData(Message message) {}    // 동작(현재 플레이 정보 저장)
public void ChangeData(Message message) {}      // 입력(정보 코드, 변화값) 
public void GetData(Message message) {}         
    // 입력(정보 코드), 출력(정보 값)
    // 입력(정보 타입, 정보 코드)
        // ? 정보 타입 == "Creature" : 출력(Creature 인스턴스)
```

## BaseSystem 에 포함된 코드.
비고
- 아래 메소드들을 직접 사용할 수도 있지만 위 명시된 인터페이스를 사용하기를 권장함.

```c#
public void CreatePlayInfo(Message msg) {}      // 입력(코드, 자료형, 초기값[, 최소값, 최대값])
public void SetPlayInfo(Message msg) {}         // 입력(코드, 설정.값)
public void ChangePlayInfo(Message msg) {}      // 입력(코드, 변경.값)
public void GetPlayInfoValue(Message msg) {}    // 입력(코드), 출력(값)
public void GetPlayInfoList(Message msg) {}     // 출력(플레이 정보 리스트)
```

## 정보 입력 양식 (일반)
정석 :
```
type = $type
code = $code
value = $value
min = $min
max = $max
end
```

숏컷
- `short = $type, $code, $value, $min, $max`

비고
- 정석과 숏컷 모두 `min, max`는 생략 가능. 
    - 둘 중 하나만 생략할 수는 없음.

## 정보 입력 양식 (Creature)
```
type = $type-string
code = $code-string
name = $name-string
hp = $hp-int
attack = $attack-int
defense = $defense-int
skill = $code-string, $name-string, $effect-double-(0~1)
drop = $code-string, $rate-double-(0~1)
end
```

비고 
- 약식은 지원되지 않음.
- `skill, drop` 생략 가능. 나머지도 생략은 가능하나 string 형은 `None`으로 int 형은 `0`으로 초기화 됨.
## update log

### 21-05-21
`CreatePlayInfo`
- 인수 변경 반영.

비고
- 누락되어있던 함수 이름 변경 반영 : `NewPlayInfo` -> `CreatePlayInfo`.

### 21-05-22
비고 
- 누락되어 있던 정보 입력 양식 추가.

### 21-05-30 
`Creature` 관련된 코드 추가.