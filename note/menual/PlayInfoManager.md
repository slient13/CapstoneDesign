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
public void GetData(Message message) {}         // 입력(정보 코드), 출력(정보 값)
```

## BaseSystem 에 포함된 코드.
비고
- 아래 메소드들을 직접 사용할 수도 있지만 위 명시된 인터페이스를 사용하기를 권장함.

```c#
public void NewPlayInfo(Message msg) {}     // 입력(코드, 자료형, 초기값)
public void SetPlayInfo(Message msg) {}     // 입력(코드, 설정.값)
public void ChangePlayInfo(Message msg) {}  // 입력(코드, 변경.값)
public void GetPlayInfo(Message msg) {}     // 입력(코드), 출력(값)
```
