using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controlable : MonoBehaviour
{
    //이렇게 abstract를 사용하면 Controlable를 상속받는 
    //클래스들은 아래 함수들을 강제로 구현해야함. 
    //이동 함수
    public abstract void Move(Vector2 input);

    //회전 함수
    public abstract void Rotate(Vector2 input);

    //차량 승하차 함수
    public abstract void Interact();

    //점프 함수
    public abstract void Jump();
} 
