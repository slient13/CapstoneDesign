using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    GameObject playerUI;
    // Start is called before the first frame update
    void Start()
    {
        playerUI = transform.GetChild(6).gameObject;
    }

    public void Sync() {
        playerUI.GetComponent<MenuPlayerUI>().Sync();
    }
}
