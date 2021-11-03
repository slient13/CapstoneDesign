## 기능

``` c#
public void Save() {}
// 기능 : 게임 진행상황을 저장함.
public void CloseGame() {}
// 기능 : 진행 상황을 저장하고 게임을 종료함.
public void ChangeScene(Message message) {}
// 입력 : 대상 씬 이름
// 기능 : 정보를 유지하며 씬을 전환함.
```

## 등록

- `GameProcessManager.cs` 파일 내 `Start()` 메서드에 대상 씬의 이름을 추가하는 코드를 작성하면 변경 대상이 될 수 있음.
- 게임 빌드 설정에서 해당 씬을 포함시켜줘야 제대로 작동함.