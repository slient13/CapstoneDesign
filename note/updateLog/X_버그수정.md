## 21-05-30
DebugAction
- 원인 불명의 사유로 날라갔던 디버그 콘솔 재 제작, 부착.

PlayInfoManager, EventListener
- 파일 로드보다 이벤트 체크가 빨라 생기던 문제 해결

PlayInfoManager
- DB 파일 양식이 일부 잘못된 것 수정

InventoryManager
- DB 파일 양식에서 소모품의 `type`을 `Consumable`으로 변경.
- `type`을 대문자로 변경.
- 현재 지원 타입. `Equipment, Consumable`. 그 외에도 입력은 되나 인벤토리 분류는 안됨.

## 21-09-05
DebugAction
- `BaseSystem/None`에 붙여놨던 이벤트 테스트용 코드 제거.

Message
- 이제 소괄호로 감싸면 콤마를 포함한 문자열 전체를 인자로 넘길 수 있음.

Player
- 이제 플레이어는 멈추지 않고도 뛸 수 있다.


## 21-09-24 : 누락 사항 반영
`PlayerInfoUI`, 플레이어 스탯창.
- 누락된 변경사항 반영. 정상 작동 확인.

## 21-11-28 : 버그 다수 수정
`QuestUI` 관련
- 퀘스트 조건이 충족되더라도 `QuestUI`를 열기 전까진 `SideUI`가 변화되지 않는 버그 수정.

이동 포탈 관련
- 포탈에서 상호작용 시도 시 상점 UI가 나오던 버그 수정.

레이싱 조작 관련
- 좌측 방향 전환용 스틱이 기능하지 않는 문제 해결. 이제 키보드와 스틱 모두 이용 할 수 있음.

`InventoryUI` 관련
- 아직 이미지가 배정되지 않은 임시 아이템을 추가하거나 제거할 때 다른 아이템이 존재하는 경우 이미지를 덮어쓰던 버그 수정.