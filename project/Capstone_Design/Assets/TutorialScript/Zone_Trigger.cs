using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone_Trigger : MonoBehaviour
{
    public string ChatText = "";
    private GameObject Zone1;
    void Start()
    {
        Zone1 = GameObject.Find("Zone1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Zone1.GetComponent<ZoneScript>().NPCChatEnter(ChatText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Zone1.GetComponent<ZoneScript>().NPCChatExit();
        }
    }
}