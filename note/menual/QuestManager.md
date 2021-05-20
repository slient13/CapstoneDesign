## 개요
- Quest 관련된 동작을 담당하는 클래스.


## 메소드
``` c#
bool loadQeust() {}                                     // 동작(Quest 로드). 자동 수행.
int getProcessData(string code, string type) {}         // 입력(코드, 타입). 출력(정보-값). 타입 = {Inventory | Stat}
void setData(string code, string type, string value) {} // 입력(코드, 타입). 동작(정보 변경). 타입 = {Inventory | Stat}
bool checkGoal(Quest target) {}                         // 출력(목표 달성 여부)
bool checkPrice(Quest target) {}                        // 출력(비용 충족 여부)
bool finishQuest(Quest target) {}                       // 동작(비용 청산, 보상 제공, 퀘스트 종료), 출력(성공 여부)
public void GetQuestProcessInfo(Message message) {}     // 출력((목표, 비용)-(코드, 현재 값, 목표 값)-리스트)
public bool StartQuest(Message message) {}              // 입력(퀘스트-코드). 동작(퀘스트 시작), 출력(성공 여부)
public void IsQuestInProcess(Message message) {}        // 입력(퀘스트-코드). 출력(퀘스트 진행 중 여부)
public bool CheckQuestFinish(Message message) {}        // 입력(퀘스트-코드, 종료 시도 여부), 동작(? 종료-시도-여부 = true : 퀘스트-종료-시도), 출력(종류)
```

## 입력 파일.
- 파일 위치 : `Resources/Quest/Info/`
- 퀘스트 이름 = 파일 이름.
- goal : 목표. 달성 필요.
    - type = `Inventory | Stat`
    - code : 대상 코드.
    - value : 목표값.
- price : 비용. 달성 필요. 완료 시 소모.
    - type = `Inventory | Stat`
    - code : 대상 코드.
    - value : 비용값.
- reward : 보상. 완료 시 제공.
    - type = `Inventory | Stat`
    - code : 대상 코드.
    - value : 보상값.