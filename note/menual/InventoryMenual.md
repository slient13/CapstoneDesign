# 인벤토리 사용법(개발 시)

## 구조

### InventoryManager

인벤토리의 내부 시스템. 전반적인 인벤토리 동작을 관리한다.

```c#
public void AddNewItem(Message msg) {}                  // 신규 아이템 정보 등록
    // $1 = 신규 아이템 코드
    // $2 = 신규 아이템 타입
    // $3 = 신규 아이템 이름
    // $4 = 신규 아이템 툴팁
    // $5 = 신규 아이템 효과(Message-string-list)
public void ModifyItem(Message msg) {}                  // 아이템 정보 변경.
    // $1 = 변경 아이템 코드, $2 = 변경 개수.
    // 해당 코드의 아이템이 등록되어 있지 않은 경우 작동 중단.
    // 동일한 아이템이 기존에 있는 경우 개수 변경
    // 동일한 아이템이 없는 경우 새로 추가
    // 변경 결과 아이템 개수가 0개가 되면(0개 이하는 자동으로 0개로 바뀜) 삭제.
public void ItemMove(int beforePos, int afterPos) {}    // 아이템 위치 변경.
public void GetItemBoxList(Message message) {}          // 아이템박스 리스트 반환. 인수 없음.
public void GetItemNumber(Message message) {}           // 해당 아이템의 개수 반환. $1 = 아이템 코드
public void GetItemBoxCount(Message message) {}         // 현재 인벤토리에 들어있는 아이템 가짓수 반환.
public void GetItem(Message message) {}                 // 아이템 반환.
public void UseItem(Message message) {}                 // 아이템 사용. 소비하고 효과를 발동. 단 아이템 효과가 존재해야 함.
```

### InventoryUIManager

인벤토리 UI 관리자. 인벤토리 UI의 업데이트 및 조작에 대한 처리를 담당한다. // 변경 예정.

## 아이템 기록 방법
저장 위치 : `InventoryManager.itemInfoPathList` 참고.

저장 양식 : 
- code = 아이템 코드. 생략 불가능. 생략하면 입력이 안됨.
- type = 아이템 타입. 생략 시 **general**로 자동 설정됨.
- name = 아이템 이름. 생략 시 `code` 값으로 자동 설정됨.
- desc = 아이템 설명. 생략 시 빈 내용이 됨.
- effect = 아이템 효과. 생략 시 효과가 없는 것으로 저장 됨. 다중 효과 부여 가능.
- end = 입력 종료.

숏컷 : 
- short = 타입, 코드, 이름, 설명[, 효과]


## updateLog
### 21-05-04
- 각 메소드의 명칭이 첫 문자 대문자로 변경.
- `GetItemBoxCount`, `UseItem` 인터페이스 추가.

### 21-05-14
- 빠져있던 `GetItem` 메소드 설명 추가.
- 아이템 정의 방법에 대한 기술 추가.

### 21-05-21
- 저장 위치 분산 지원 기능 추가.
- 정보 입력용 숏컷 추가.