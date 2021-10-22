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

비고
- 아래 메소드들을 직접 사용할 수도 있지만 위 명시된 인터페이스를 사용하기를 권장함.

```c#
public void CreatePlayInfo(Message msg) {}
// 입력 = 이름, 값, [최소값, 최대값]. 
// 기능 = `PlayInfo` 생성.
public void CreateInstance(Message message) {}
// 입력 = 이름, [아이디 = 0]. 
// 기능 = 해당 이름과 일치하는 `PlayInfo`를 복사하여 `Instance`로 만들어 추가함.
public void RemoveInstance(Message message) {}
// 입력 = 이름, 아이디.
// 기능 = 해당 이름과 일치하는 `PlayInfo-Instance`를 제거.
public void SetData(Message msg) {}
// 입력 = 이름
// 기능 = `PlayInfo` 혹은 `PlayInfo-Instance`의 값을 설정.
public void ChangeData(Message msg) {}
// 입력 = 이름
// 기능 = `PlayInfo` 혹은 `PlayInfo-Instance`의 값을 변경.
public void GetPlayInfo(Message msg) {}
// 입력 = 이름
// 반환 = 일치하는 PlayInfo
public void GetPlayInfoValue(Message message) {}
// 입력 = 이름, [인덱스 = 0]
// 반환 = 일치하는 `PlayInfo`의 특정 순번 데이터 반환.
```

## 정보 입력 양식 (일반)
정석 :
```
code = $code
value = $value
min = $min
max = $max
end
```

숏컷
- `short = $code, $value, $min, $max`

비고
- 정석과 숏컷 모두 `min, max`는 생략 가능. 
    - 둘 중 하나만 생략할 수는 없음.

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

### 21-09-18
- `PlayInfo` 구조 변경 : `단일 자료형` -> `트리형`
- `PlayInfo`의 처리 규격을 `PlayInfoManager`를 경유하지 않고 `BaseSystem`에서 바로 처리.
- `Creature`에 대한 별도의 정의 규격을 제거.
- `PlayInfo-Instance` 생성.