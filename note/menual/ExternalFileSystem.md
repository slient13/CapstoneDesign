## 개요
- 파일 입출력을 전담하는 싱글톤 클래스.

## 사용법
1. 클래스에서 `Singleton()` 메소드를 바로 호출. 객체를 받아옴.
1. 아래 명시된 메소드를 이용하여 원하는 동작 수행.

## 인터페이스

``` C#
List<string> GetItemInfo() {}                        // 출력(아이템 정의)
bool SaveInventory(List<ItemBox> itemBoxList) {}     // 입력(인벤토리 정보)
List<string> LoadInventory() {}                      // 출력(인벤토리 정보가 담긴 한 줄 단위 문자열 리스트)
ShopInfo GetShopInfo(string shopName) {}             // 입력(상점 코드), 출력(상점 정의)
List<string> GetTalkInfo(string talkName) {}         // 입력(대상 코드), 출력(대화 스크립트)
List<string> GetPlayInfo() {}                        // 출력(플레이 정보 정의)
List<string> LoadPlayData() {}                       // 출력(플레이 정보)
void SavePlayData(List<PlayInfo> playInfoList) {}    // 입력(플레이 정보)
public Dictionary<string, Quest> LoadQeust(List<string> questNameList) {} // 입력(퀘스트-이름-리스트), 출력(퀘스트-딕셔너리)
```

## update log

### 21-05-20
- `LoadQuest` 추가