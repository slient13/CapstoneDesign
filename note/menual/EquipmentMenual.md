## 개요
- 장비 관련 데이터의 로드와 장착 및 해제 기능을 제공.
- 현재는 단순히 스탯을 변화시키는 장비만이 지원됨.

## 기능

``` c#
public void Equip(Message message) {}
// 장비를 장착함.
// 입력 = 장비.코드.
public void Unequip(Message message) {}
// 장비를 해제함
// 입력 = 슬롯 번호
public void GetEquipState(Message message) {}
// 현재 장비 착용 상태를 불러옴
// 출력 = 장비 리스트.
public void IsEquip(Message message) {}
// 입력한 장비가 착용 상태인지 검사
// 입력 = 장비.코드, 출력 = bool.
```