# FunctionCaller 함수 사용법

## Message 클래스 구조
- targetName
대상 오브젝트 이름. 같은 씬 내에서 찾아낸다.
- functionName
실행할 함수. 대상 오브젝트의 스크립트 내 정의되어 있어야 한다.
- args
인수들. 다차원 ArrayList 형태로 제공되며, 필요에 따라 각종 형태로 개조할 수 있다.
- returnValue
반환값. 마찬가지로 ArrayList 이며, 반환값이 필요한 경우 관례적으로 이 값을 고치고 이용하도록 한다.

## 사용법
- Message 객체 생성 // 예시 = `msg`
- `BaseSystem.SendMessage("functionCaller", msg);` 입력

## Message 객체 생성 방법
- 명령어 문자열
`targetName/functionName : arg, arg, ...` 의 양식을 가진 문자열을 객체 생성시 넣어서 생성한다.
그러면 해당 문자열을 해독해 자동으로 각 값에 배치한다.
- 값 직접 배정
`Message` 클래스의 대부분의 속성은 `public`으로 선언되어 직접 고칠 수 있다.
이를 이용해 값을 직접 할당하는 식으로도 쓸 수 있다.
이 경우 명령어 방식으로는 전달할 수 없는 클래스 형식의 인수도 전달할 수 있다.

예시
```
Message msg = new Message();
msg.targetName = "object";
msg.functionName = "function";
msg.args.Add(10);
msg.args.Add(20);
msg.args.Add(30);
// 위 코드와 아래 코드는 같은 의미를 가진 Message 객체를 만든다.
Message msg2 = new Message("object/function : 10, 20, 30");
```

## 인수 변환
문자열 입력시 인수는 아래와 같이 변환된다.
```
123 // 정수
123.45 // 실수
abcd // 문자열
[12, 34] // 배열(정수, 정수)
[12, [34, 56], 78] // 배열(정수, 배열(정수, 정수), 정수)
```

## 반환값 사용법
- 평범하게 `SendMessage` 보낸 이후 앞서 생성한 `Message`객체의 `returnValue`값을 직접 참조해서 사용하면 된다.

## 주의사항
- `args` 와 `returnValue` 는 `ArrayList` 형이므로, 요소값들은 기본적으로 `object` 형으로 박싱 되어 있기 때문에, 사용하기 전에 명시적으로 형변환을 한 번 거쳐야 한다.
```
int a = (int)msg.args[0];
// 혹은
int a = msg.args[0] as int;
```
- 호출 대상 함수는 반드시 `Message` 형 변수를 유일한 인수로 가져야만 한다.
- `SendMessage`를 보내는 오브젝트는 반드시 `BaseSystem` 스크립트를 가지고 있는 오브젝트와 연결되어 있어야 한다. 때문에 해당 오브젝트의 이름은 관례적으로 "BaseSystem"으로 통일하도록 한다.
- `returnValue`의 동기화 문제는 테스트 결과 최소한 100만 * 100만 번의 `++`연산에서는 문제점이 발생하지 않는다는 것을 확인했다. 허나 모든 경우에 문제가 없다고 확인된 것은 아니므로 주의할 필요가 있다.
