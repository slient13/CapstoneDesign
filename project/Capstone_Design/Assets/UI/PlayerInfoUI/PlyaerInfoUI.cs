using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerInfoUI : MonoBehaviour
{
    public GameObject infoPanel;
    bool activeInfo = false;

    private void Start()
    {
        infoPanel.SetActive(activeInfo);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            activeInfo = !activeInfo;
            infoPanel.SetActive(activeInfo);
        }
    }
}
