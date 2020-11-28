using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [HideInInspector]
    public ItemProperty item;

    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Button sellBtn;

    private void Awake()
    {
        SetSellBtnInteractable(false);
    }

    void SetSellBtnInteractable(bool b)
    {
        if(sellBtn != null)
        {
            sellBtn.interactable = b;
        }
    }


    public void SetItem(ItemProperty item)
    {
        this.item = item;

        if(item == null)
        {
            image.enabled = false;
            SetSellBtnInteractable(false);
            gameObject.name = "Empty";
        }
        else 
        {
            image.enabled = true;

            gameObject.name = item.name;
            image.sprite = item.sprite;
            SetSellBtnInteractable(true);
        }
    }

    public void OnClickSellBtn()
    {
        SetItem(null);
    }
}
