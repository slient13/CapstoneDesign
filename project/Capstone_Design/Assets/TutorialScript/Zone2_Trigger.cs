using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone2_Trigger : MonoBehaviour
{
    public string ChatText = "";
    private GameObject Zone2;
    void Start()
    {
        Zone2 = GameObject.Find("Zone2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Zone2.GetComponent<Zone2Script>().NPCChatEnter(ChatText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Zone2.GetComponent<Zone2Script>().NPCChatExit();
        }
    }
}