using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f; //선언된 변수 rotationSpeed의 초기값을 60으로 할당
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //실질적인 초당 프라임에 대한 회전을 시키기 위해 변수 rotationSpeed에다가 초당프레임의 역수인 TIme.deltaTime을 곱해준다.
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
