using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNPC : MonoBehaviour
{


    private void OnTriggerEnter(Collider collider)

    {
        //Debug.Log("충돌 시작!");
        Message msg = new Message("Guide");


    }
    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌이 끝났을 때

    private void OnTriggerExit(Collider collider)

    {

        //Debug.Log("충돌 끝!");


    }

}





