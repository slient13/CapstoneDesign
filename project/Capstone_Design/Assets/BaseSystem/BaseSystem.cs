using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    public GameObject target;

    void Start() {
    }

    public void functionCaller(Message msg) {
        // Debug.Log(msg.args[0]);
        target = GameObject.Find(msg.targetName);
        // 타겟 포착 실패시 메세지 호출 후 함수 종료
        if (target == null) {
            Debug.Log("Error : Could not find object.");
            return;
        }
        target.SendMessage(msg.functionName, msg);
        // Debug.Log(msg.returnValue[0]);
    }
}

