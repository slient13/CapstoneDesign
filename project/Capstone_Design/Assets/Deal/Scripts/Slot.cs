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


    /*public void OnClickSellBtn()
   {

       SetItem(null);


        if (gameObject.name = "체 력")
        {

            Message msg10 = new Message("getPlayInfo: Health");
            msg10.functionCall();
            int Health = (int)msg10.returnValue[0];

           /*
           Message msg3 = new Message("playInfoChanger: Health, 10");
           msg3.functionCall();


           Message msg4 = new Message("playInfoChanger: Coin, 1000");
            msg4.functionCall();

            Message msg5 = new Message("InventoryManager/modifyItem : Health, -1");
            msg5.functionCall();

            if (Health > 100)
            {
                Debug.Log("더 이상 올릴 체력이 없습니다.");

            }
        }


        if (gameObject.item = "물 고 기")
        {

            Message msg11 = new Message("getPlayInfo: Fish");
            msg11.functionCall();
            int Fish = (int)msg11.returnValue[0];

            Message msg12 = new Message("playInfoChanger: Fish, -1");
            msg12.functionCall();

            Message msg7 = new Message("playInfoChanger: coin, 700");
            msg7.functionCall();

            Message msg6 = new Message("InventoryManager/modifyItem : Fish, -1");
            msg6.functionCall();

            if (Fish > 10)
            {
                Debug.Log("더 이상 물고기를 가질 수 없습니다.");
            }
        }


        if (gameObject.item = "미 끼")
        {

            Message msg13 = new Message("getPlayInfo: Bug");
            msg13.functionCall();
            int Bug = (int)msg13.returnValue[0];

            Message msg14 = new Message("playInfoChanger: Bug, -1");
            msg14.functionCall();

            Message msg9 = new Message("playInfoChanger: coin, 100");
            msg9.functionCall();

            Message msg8 = new Message("InventoryManager/modifyItem : Bug, -1");
            msg8.functionCall();

            if (Bug > 10)
            {
                Debug.Log("더 이상 미끼를 가질 수 없습니다.");
            }
        }

   } */
    


}

