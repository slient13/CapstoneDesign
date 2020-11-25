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
```

### `InputChecker`

- 입력 상태를 검사, 상태를 추적한다. 보통 `ControlManager`에 소속되어 작동한다.
- 관련 동작은 `ControlManager`에서 전담한다.

### `MappingInfo`

- 각 오브젝트들이 가지는 자기 자신의 키 매핑 정보이다.

```c#
public List<Info> infoList;                    // 매핑 정보 리스트이다.
public string objectName;                      // 해당 오브젝트의 이름 문자열이다.
public void addMapping(string command, string keyPattern) {}   // 매핑 정보 추가
public void removeMapping(int index) {}        // 매핑 정보 제거
public List<string> getMappingInfo() {}        // 매핑 정보 목록 확인
public void reset() {}                         // 매핑 정보 초기화
public MappingInfo copy(MappingInfo other) {}  // 입력 매핑 정보 복사, 반영(자동 초기화)
public void enroll() {}                        // 매핑 정보 등록
```

위에서 `command`는 통신 중계자 함수에서 사용하는 양식에서 타겟 오브젝트를 뺀 것이며, `keyPattern`은 ControlManager 에서 쓰는 양식이다.

아래와 같이 사용한다.

```c#
mappinginfo.addMapping("move : up", "_w");
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

### `MappingInfo` 객체 이용법

- `addMapping` 메소드를 이용하여 함수의 이름이랑 인수가 적인 커멘드와 키 조합을 저장한다.
- `enroll` 메소드를 이용하여 매핑을 등록한다.
- 기존 매핑 정보를 고쳐야될 일이 있다면 `removeMapping` 메소드를 이용한다.
- 완전히 동일 한 매핑 구조를 가진다면 타 오브젝트의 `MappingInfo`객체를 불러와 `copy` 메소드를 이용해 복사할수도 있다.

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
"a, b"      // 'a'와 'b'가 동시에 입력되는 경우를 첫 1회만 감지한다.
"_w, !a, !s, !d"    // 'w'는 눌리고, 'a, s, d'는 안눌린 상태를 지속적으로 감지한다.
"z, _x"     // 첫 예시처럼 1회만 감지하지만 x가 먼저 눌리고 있다가 뒤이어 z가 눌린 경우에도 인식한다.
"_r, ^t"    // 'r'는 눌리고 있고, 그와 동시에 't'가 눌렀다가 떼지는 순간을 1회 감지한다.
```

## 주의사항

- `MappingInfo` 객체를 만들 때 해당 오브젝트의 이름을 입력해줄 필요가 있다. 이를 잊은 경우 `enroll` 메소드로 등록할 때 인수로 이름을 기입해줄 수도 있다. 모두 잊어먹으면 정상 작동이 보장되지 않는다.
- 이미 등록해버린 정보는 수정할 수 없다. 등록 후 `removeMapping` 메소드 등을 사용해도 변치 않는다. 이를 반영하는 기능은 구현중에 있다.
- `copy` 메소드는 자동적으로 `reset` 메소드를 호출한다. 즉, 해당 `MappingInfo` 객체에 저장되어 있던 정보는 보존되지 않는다.

## 구현 예정

- 키 매핑 레이어 변경 기능
- 마우스 관련 매핑 기능
- 마우스 레이어 자동 변경 기능
- 등록 매핑 정보 수정 기능.
- 매핑 정보 추가 기능 보강.

## 변경 사항

- 11-26 : 문서 첫 작성.
