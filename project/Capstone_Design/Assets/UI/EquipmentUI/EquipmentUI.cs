using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentpanel;
    bool activeEquipment = false;

    private void Start()
    {
        equipmentpanel.SetActive(activeEquipment);
    }

    private void Updata()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeEquipment = !activeEquipment;
            equipmentpanel.SetActive(activeEquipment);
        }
    }
}