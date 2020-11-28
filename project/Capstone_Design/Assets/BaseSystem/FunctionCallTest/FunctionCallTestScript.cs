using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionCallTestScript : MonoBehaviour
{
    public GameObject baseSystem;

    public void testFunction(Message msg) {
        int temp = 2 * (int)msg.args[0];
        msg.returnValue.Add(temp);
    }

    public void selfCallFunction(Message msg) {
        ArrayList testArr = (ArrayList)msg.args[0];
        ArrayList testArr2;
        for (int i = 0; i < 2; i++) {
            testArr2 = (ArrayList)testArr[i];
            for(int j = 0; j < 2; j++) {
                Debug.Log("args[0][" + i + "][" + j + "] = " + testArr2[j]);
            }
        }
        Debug.Log("args[1] = " + msg.args[1]);
        Debug.Log("args[2] = " + msg.args[2]);
    }

    void Start() {
        baseSystem = GameObject.Find("BaseSystem");

        Message msg = new Message("TestObject/selfCallFunction : [[11, 12], [21, 22]], 123, OK");
        baseSystem.SendMessage("functionCaller", msg);
    }
}
