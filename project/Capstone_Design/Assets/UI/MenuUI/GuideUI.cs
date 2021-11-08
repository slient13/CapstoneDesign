using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    public GameObject panel;

    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        panel = transform.GetChild(0).gameObject;
        CloseUI();
        // 레이어용.
        MappingInfo mapping = new MappingInfo("GuideUI");
        mapping.AddMapping("CloseUI : ", "esc");
        mapping.Enroll("GuideUI");
    }
    public void OpenUI()
    {
        isActive = true;
        panel.SetActive(isActive);
    }

    public void CloseUI()
    {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }
}