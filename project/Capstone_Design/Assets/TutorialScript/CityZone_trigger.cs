using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityZone_trigger : MonoBehaviour
{
    public string ChatText = "";
    private GameObject GoCity;
    void Start()
    {
        GoCity = GameObject.Find("GoCity");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GoCity.GetComponent<CityZoneScript>().NPCChatEnter(ChatText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GoCity.GetComponent<CityZoneScript>().NPCChatExit();
        }
    }
}