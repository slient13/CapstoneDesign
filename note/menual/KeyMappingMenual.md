# 키 매핑 메뉴얼

## 관련 클래스

### `ControlManager`

- 전반적인 키 매핑 과정을 관리, 중계한다.

```c#
InputChecker inputChecker;                  // 입력 상태 기록용.
public GameObject baseSystem;               // 함수 중계자 호출용.
public List<GameObject> objectLayer;        // 매핑 오브젝트 목록
public List<MappingInfo> mappingInfoList;   // 매핑 정보 목록
public int target;                          // 현재 타겟 번호
public void AddMapping(Message msg) {}      // 매핑 정보 추가. $1 = 오브젝트 이름, $2 = 매핑 정보, $3 = 그룹 이름.
public void RemoveMapping(Message msg) {}   // 매핑 정보 제거. $1 = 오브젝트 이름
    // 한 오브젝트에서 다수의 매핑 정보를 추가한 경우 전부 제거되니 주의
public void UpdateMapping(Message msg) {}   // 매핑 정보 변경. $1 = 오브젝트 이름, $2 = 매핑 정보, $3 = 그룹 이름.
public void LayerChanger(Message msg) {}    // 매핑 동작 대상 변경. $1 = 그룹 이름
```

### `InputChecker`

- 입력 상태를 검사, 상태를 추적한다. 보통 `ControlManager`에 소속되어 작동한다.
- 관련 동작은 `ControlManager`에서 전담한다.

### `MappingInfo`

- 각 오브젝트들이 가지는 자기 자신의 키 매핑 정보이다.

```c#
public List<Info> infoList;                     // 매핑 정보 리스트이다.
public string objectName;                       // 해당 오브젝트의 이름 문자열이다.
public void AddMapping(string command, string keyPattern) {}    // 매핑 정보 추가
public void RemoveMapping(int index) {}         // 매핑 정보 제거
public List<string> getMappingInfo() {}         // 매핑 정보 목록 확인
public void Reset() {}                          // 매핑 정보 초기화
public MappingInfo Add(MappingInfo other) {}    // 매핑 정보 추가. (기존 정보 보존)
public MappingInfo Copy(MappingInfo other) {}   // 매핑 정보 복사. (기존 정보 초기화)
public void Enroll(string group) {}             // 매핑 정보 등록
    // group = 그룹 이름. 생략 시 자동으로 "general" 삽입.
public void MappingUpdate() {}                  // 매핑 정보 업데이트. (기존 등록 정보가 있는 경우에만)
```

위에서 `command`는 통신 중계자 함수에서 사용하는 양식에서 타겟 오브젝트를 뺀 것이며, `keyPattern`은 ControlManager 에서 쓰는 양식이다.

아래와 같이 사용한다.

```c#
mappinginfo.AddMapping("move : up", "_w");
```

`keyPattern`의 양식은 후술한다.

### `Info`

`MappingInfo` 에서 사용하는 사용자 정의 객체이다.

```c#
public string command; // 실행을 원하는 함수의 명령어 문자열이다.
public string keyPattern; // 실행 조건이 되는 키 조합 문자열이다.
```

## 사용법

### 사용 준비

- 키 매핑을 사용하기 위해선 `ControlManager`스크립트가 들어간 "ControlManager"라는 오브젝트를 미리 배치해야 한다.
- 보통은 다른 시스템과 같이 자동으로 포함된다.

### `MappingInfo` 객체 이용법

- `AddMapping` 메소드를 이용하여 함수의 이름이랑 인수가 적인 커멘드와 키 조합을 저장한다.
- `Enroll` 메소드를 이용하여 매핑을 등록한다.
- 기존 매핑 정보를 고쳐야될 일이 있다면 `RemoveMapping` 메소드를 이용한다.
- 완전히 동일한 매핑 구조를 가진다면 타 오브젝트의 `MappingInfo`객체를 불러와 `Copy` 메소드를 이용해 복사할수도 있다.

### 타겟 레이어 변경 방법

- `ControlManager.LayerChanger`를 호출한다. 이 때 인수는 타겟 오브젝트의 이름 문자열이다.

### 키 매핑 양식

키 매핑 문자열은 아래 양식을 따르며, 다수의 조합을 원하는 경우 콤마로 구분하면 된다.

```c#
"a"     // a 입력 1회 감지.
"_a"    // a 입력 지속 감지.
"^a"    // a 입력 중단 감지.
"!a"    // a 비입력 감지.
```

아래는 조합에 관한 예시이다.

```c#
"a, b"              // 'a'와 'b'가 동시에 입력되는 경우를 첫 1회만 감지한다.
"_w, !a, !s, !d"    // 'w'는 눌리고, 'a, s, d'는 눌리지 않은 상태를 지속적으로 감지한다.
"z, _x"             // 첫 예시처럼 1회만 감지하지만 x가 먼저 눌리고 있다가 뒤이어 z가 눌린 경우에도 인식한다.
"_r, ^t"            // 'r'는 눌리고 있고, 그와 동시에 't'가 눌렀다가 떼지는 순간을 1회 감지한다.
```

기타 어떤 키를 인식할 수 있고, 어떻게 입력해야 인식하는지에 대한 내용은 하단 부록을 참고하라.

### 다수의 오브젝트의 맵핑을 한 레이어에 연결하는 방법

- 동일한 그룹으로 등록한다. 그룹간 전환은 `LayerChanger` 메소드를 이용한다.

### 마우스 매핑

- 키 매핑과 동일하게 적용한다.
- 마우스의 위치는 인수 리스트 맨 끝에 추가되어 전송된다. (Vector2 형)
- 마우스 위치 정보는 마우스 클릭 이벤트가 아니더라도 전송되므로 키 입력 매핑 중에도 사용할 수 있다.

## 주의사항

- `MappingInfo` 객체를 만들 때 해당 오브젝트의 이름을 입력해줄 필요가 있다. 아직 스스로 찾는 기능은 없다.
- `ControlManager.RemoveMapping` 메소드는 해당 오브젝트 이름으로 등록된 모든 매핑을 제거한다.

## 구현 예정

- 마우스 레이어 자동 변경 기능

## 부록

### 매핑용 문자열 목록

```c#
// 알파벳
"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
// 방향키
"arrowL", "arrowD", "arrowU", "arrowR",
// 각종 기능키
"space", "enter", "backspace", "ctrlL", "ctrlR",
"altL", "altR", "shiftL", "shiftR", "tab", "caps", "esc",
// 숫자
"n1", "n2", "n3", "n4", "n5", "n6", "n7", "n8", "n9", "n0",
// 상단 특수문자(` - =)
"backQuote", "minus", "equal",
// 특수문자(. , / ; ' [ ] \)
"dot", "comma", "slash", "semicolon", "quote", "bracketL", "barcketR", "backslash",
// 펑션키
"f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8", "f9", "f10", "f11", "f12",
// 마우스(왼쪽, 오른쪽, 휠클릭, 추가키 (1, 2, 3))
"mouseL", "mouseR", "mouseM", "mouseEx1", "mouseEx2", "mouseEx3"
```

## 변경 사항

### 11-25
- 문서 첫 작성.

### 11-26
- 추가 기능 관련 관련 서술 추가.(키 매핑 레이어 변경 기능, 마우스 매핑기능, 등록 매핑 수정기능, 매핑 정보 추가 기능)
- 다수의 오브젝트에서 동일한 레이어를 사용하기 위한 방법 서술.

### 11-27
- 마우스 위치 관련 전송방법 수정. (첫번째 인수 고정 -> 맨 끝 인수로)
- 부록으로 매핑용 키 텍스트 목록 추가.

### 11-28
- 매핑 그룹화 기능 개선.
- 주의사항 추가.

### 21-05-04
- 메소드 명칭 표기법 변경 : `camel casing` -> `pascal casing`.