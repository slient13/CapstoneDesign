using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public MappingInfo infoList;

    private void Start()
    {
        infoList = new MappingInfo("Player");
        infoList.AddMapping("move : up", "_w");
        infoList.AddMapping("move : down", "_s");
        infoList.Enroll();
    }

    public void move(Message message)
    {
        if ((string)message.args[0] == "up") transform.Translate(Vector3.forward * Time.deltaTime);
        else if ((string)message.args[0] == "down") transform.Translate(Vector3.back * Time.deltaTime);
    }
}