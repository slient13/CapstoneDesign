using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Trigger : MonoBehaviour
{
    public string ChatText = "";
    private GameObject Main;
    void Start()
    {
        Main = GameObject.Find("Main");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Main.GetComponent<MainScript>().NPCChatEnter(ChatText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Main.GetComponent<MainScript>().NPCChatExit();
        }
    }
}