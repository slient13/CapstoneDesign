using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentPanel;
    bool activeEquipment = false;

    private void Start()
    {
        equipmentPanel.SetActive(activeEquipment);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            activeEquipment = !activeEquipment;
            equipmentPanel.SetActive(activeEquipment);
        }
    }
}