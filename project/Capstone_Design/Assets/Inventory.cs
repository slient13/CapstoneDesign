using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform rootSlot;
    public Store store;

    private List<Slot> slots;
    // Start is called before the first frame update
    void Start()
    {
        slots = new List<Slot>();

        int slotCnt = rootSlot.childCount;

        for(int i = 0; i < slotCnt; i++)
        {
            var slot = rootSlot.GetChild(i).GetComponent<Slot>();

            slots.Add(slot);
        }

        store.onSlotClick += BuyItem;
    }

    void BuyItem(ItemProperty item)
    {
        //Debug.Log(item.name);
        var emptySlot = slots.Find(t =>
        {
            return t.item == null || t.item.name == string.Empty;

            
                Message msg10 = new Message("getPlayInfo: Health");
                msg10.functionCall();
                int health = (int)msg10.returnValue[0];

                /*
                Message msg3 = new Message("playInfoChanger: Health, 10");
                msg3.functionCall();
                */

                Message msg4 = new Message("playInfoChanger: Coin, -1000");
                msg4.functionCall();

                if (health > 100)
                {
                    Message msg5 = new Message("InventoryManager/modifyItem : Health, 1");
                    msg5.functionCall();
                }
            
            
            
                Message msg6 = new Message("InventoryManager/modifyItem : fish, 1");
                msg6.functionCall();
                Message msg7 = new Message("playInfoChanger: coin, -700");
                msg7.functionCall();
            
                Message msg8 = new Message("InventoryManager/modifyItem : bug, 1");
                msg8.functionCall();
                Message msg9 = new Message("playInfoChanger: coin, -100");
                msg9.functionCall();
            
        });

        if(emptySlot != null)
        {
            emptySlot.SetItem(item);
        }
    }
}
