# 플레이 정보 관리 메뉴얼

## 구성

### BaseSystem

- 플레이 정보를 가지고 있는 개체.
- 플레이 정보의 신규 정의 및 변경, 조회등의 기능을 제공함.

```c#
public void newPlayInfo(Message msg) {}     // 새로운 플레이 정보를 정의함. $1 = 정보이름, $2 = 정보타입, $3 = 정보값(object 형 방식)
public void playInfoSetter(Message msg) {}  // 플레이 정보 설정. $1 = 정보이름, $2 = 설정값
public void playInfoChanger(Message msg) {} // 플레이 정보 변경. (int, float의 경우 덧셈, string의 경우 이어붙힘))
                                            //  $1 = 정보이름, $2 = 변동값
public void getPlayInfo(Message msg) {}     // 플레이 정보 확보.
                                            //  $1 = 정보이름
                                            // returnValue[0] = 반환값(object 박싱), returnValue[1] = 반환 값의 타입.(string -> object)
```
