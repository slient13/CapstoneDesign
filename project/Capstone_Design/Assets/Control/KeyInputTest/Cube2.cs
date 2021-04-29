using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube2 : MonoBehaviour {
    public MappingInfo infoList;

    private void Start() {
        infoList = new MappingInfo("Cube2");
        infoList.AddMapping("Move : up", "_w");
        infoList.AddMapping("Move : down", "_s");
        infoList.AddMapping("Toggle: ", "n1");
        infoList.Enroll();
    }

    public void Move(Message message) {
        if ((string)message.args[0] == "up") transform.Translate(Vector3.forward * Time.deltaTime);
        else if ((string)message.args[0] == "down") transform.Translate(Vector3.back * Time.deltaTime);
    }

    public void Toggle(Message msg) {
        new Message("ControlManager/layerChanger : Cube1").FunctionCall();
    }
}