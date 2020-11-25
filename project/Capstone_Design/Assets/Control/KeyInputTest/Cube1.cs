using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : MonoBehaviour {
    [SerializeField] public MappingInfo infoList;

    private void Start() {
        infoList = new MappingInfo();
        infoList.addMapping("move : up", "_w");
        infoList.addMapping("move : down", "_s");
        infoList.enroll("Cube1");
    }

    public void move(Message message) {
        if ((string)message.args[0] == "up") transform.Translate(Vector3.forward * Time.deltaTime);
        else if ((string)message.args[0] == "down") transform.Translate(Vector3.back * Time.deltaTime);
    }
}