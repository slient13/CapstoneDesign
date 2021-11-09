using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityZoneScript : MonoBehaviour
{
    private GameObject CityPanel;
    private Text CityText;

    void Start()
    {
        CityPanel = GameObject.Find("CityPanel");
        CityText = GameObject.Find("CityText").GetComponent<Text>();
        CityPanel.SetActive(false);
    }

    public void NPCChatEnter(string text)
    {
        CityText.text = text;
        CityPanel.SetActive(true);
    }

    public void NPCChatExit()
    {
        CityText.text = "";
        CityPanel.SetActive(false);
    }
}