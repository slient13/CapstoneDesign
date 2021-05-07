## 개요
- 상점 시스템을 구현하기 위한 기반 코드

## 제공 기능
- 아이템 정보 불러오기
- 구매
- 판매

## 이용 방법
- Message 객체 이용.

## 인터페이스

```c#
public void Buy(Message message) {}     // 구매. 입력(아이템 코드, 갯수), 출력(실패 여부)
public void Sell(Message message) {}    // 판매. 입력(아이템 코드, 갯수), 출력(실패 여부)
```