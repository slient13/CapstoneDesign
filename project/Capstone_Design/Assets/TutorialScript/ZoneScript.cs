using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneScript : MonoBehaviour
{
    private GameObject RacingPanel;
    private Text RacingText;

    void Start()
    {
        RacingPanel = GameObject.Find("RacingPanel");
        RacingText = GameObject.Find("RacingText").GetComponent<Text>();
        RacingPanel.SetActive(false);
    }

    public void NPCChatEnter(string text)
    {
        RacingText.text = text;
        RacingPanel.SetActive(true);
    }

    public void NPCChatExit()
    {
        RacingText.text = "";
        RacingPanel.SetActive(false);
    }
}