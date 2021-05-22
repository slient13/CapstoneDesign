using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public GameObject questpanel;
    bool activeQuest = false;

    private void Start()
    {
        questpanel.SetActive(activeQuest);
    }
   


    private void Update()
    {
     if(Input.GetKeyDown(KeyCode.Q))
        {
            activeQuest = !activeQuest;
            questpanel.SetActive(activeQuest);
        }
    }
}
