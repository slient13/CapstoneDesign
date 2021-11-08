using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zone2Script : MonoBehaviour
{
    private GameObject HuntingPanel;
    private Text HuntingText;

    void Start()
    {
        HuntingPanel = GameObject.Find("HuntingPanel");
        HuntingText = GameObject.Find("HuntingText").GetComponent<Text>();
        HuntingPanel.SetActive(false);
    }

    public void NPCChatEnter(string text)
    {
        HuntingText.text = text;
        HuntingPanel.SetActive(true);
    }

    public void NPCChatExit()
    {
        HuntingText.text = "";
        HuntingPanel.SetActive(false);
    }
}