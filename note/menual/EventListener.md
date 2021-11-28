## 개요
- `Message` 객체를 통한 모든 통신을 검사, 미리 입력한 대상 함수가 실행된 경우 연결된 메소드를 자동 실행시키는 클래스.
- singletone 클래스. 별도 객체 생성 x.

## 구조

``` c#
public static EventListener GetEventListener() {}
    // EventListener 객체 반환.
public void EventCall(Message message) {}
    // 이벤트 호출 함수. 자동 수행.
public void Binding(string objectName, string functionName, string eventCommand) {}
    // 바인딩 함수. 감지할 메시지 객체의 타겟 오브젝트 이름과 함수 이름을 지정.
    // 지정된 함수는 `EventCall` 함수 수행 시 자동으로 따라 수행됨.
public void Binding(BindingInfo bindingInfo) {}
    // 바인딩 함수. 오버로딩.
public void CancleBinding(string objectName, string functionName, string eventCommand) {}
    // 입력과 일치하는 바인딩 제거.
```

## 사용법
등록
- `EventListener.GetEventListener().Binding(인수)`를 통해 등록.
    - `objectName` : 감지하고 싶은 이벤트의 대상 오브젝트 이름.
    - `functionName` : 감지하고 싶은 이벤트의 함수 이름.
    - `eventCommand` : 이벤트 발생 시 수행할 코드. `Message`객체 사용법과 동일.

사용
- `EventCall()` 함수에 `Message` 객체를 전달해주면 바인딩 된 메시지인지 확인하고 연결된 함수들을 자동으로 실행함.
- `EventCall()`을 통해 실행된 대상 함수는 본래 지정된 인수에 추가로 집어넣어준 `Message` 객체의 인수들까지 추가되어 호출됨.
    - 추가되는 인수들은 `ArrayList`형으로 묶여 마지막 인수 하나로 추가되어 전달됨.