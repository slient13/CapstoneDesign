# 인벤토리 사용법(개발 시)

## 구조

### InventoryManager

인벤토리의 내부 시스템. 전반적인 인벤토리 동작을 관리한다.

```c#
public void addNewItem(Message msg) {}                  // 신규 아이템 정보 등록
    // $1 = 신규 아이템 코드
    // $2 = 신규 아이템 이름
    // $3 = 신규 아이템 툴팁
    // $4 = 신규 아이템 효과(Message 양식)
public void modifyItem(Message msg) {}                  // 아이템 정보 변경.
    // $1 = 변경 아이템 코드, $2 = 변경 개수.
    // 해당 코드의 아이템이 등록되어 있지 않은 경우 작동 중단.
    // 동일한 아이템이 기존에 있는 경우 개수 변경
    // 동일한 아이템이 없는 경우 새로 추가
    // 변경 결과 아이템 개수가 0개가 되면(0개 이하는 자동으로 0개로 바뀜) 삭제.
public void itemMove(int beforePos, int afterPos) {}    // 아이템 위치 변경.
public void getItemBoxList(Message message) {}          // 아이템박스 리스트 반환. 인수 없음.
public void getItemNumber(Message message) {}           // 해당 아이템의 개수 반환. $1 = 아이템 코드
```

### InventoryUIManager

인벤토리 UI 관리자. 인벤토리 UI의 업데이트 및 조작에 대한 처리를 담당한다.
