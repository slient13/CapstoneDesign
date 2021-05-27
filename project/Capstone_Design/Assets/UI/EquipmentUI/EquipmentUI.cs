using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject panel;
    bool isActive = false;

    private void Start()
    {
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(isActive);
    }

    private void Updata()
    {
    }

    public void OpenUI(Message message) {
        isActive = true;
        panel.SetActive(isActive);        
    }

    public void CloseUI(Message message) {
        isActive = false;
        panel.SetActive(isActive);
    }
}