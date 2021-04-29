using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public ItemBuffer itemBuffer;
    public Transform slotRoot;

    private List<Slot> slots;

    public System.Action<ItemProperty> onSlotClick;

    void Start()
    {
        slots = new List<Slot>();

        //slots의 하위 slot들을 모두 가져온다
        int slotCnt = slotRoot.childCount;

        for (int i = 0; i < slotCnt; i++)
        {
            var slot = slotRoot.GetChild(i).GetComponent<Slot>();

            if(i < itemBuffer.items.Count)
            {
                slot.SetItem(itemBuffer.items[i]);
            }

            slots.Add(slot);
        }

        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickSlot(Slot slot)
    {
        //Debug.Log(slot.name);
        if (onSlotClick != null)
        {
            /*
            onSlotClick(slot.item);
            Debug.Log(slot.item.name);
            */

            // 구매
            if (slot.tag == "Health")   BuyItem(slot, "Health", 1000);
            if (slot.tag == "Fish")     BuyItem(slot, "Fish", 700);
            if (slot.tag == "Bug")      BuyItem(slot, "Bug", 100);

            // 판매
            if (slot.tag == "health")   SellItem(slot, "Health", 1000);
            if (slot.tag == "fish")     SellItem(slot, "Fish", 700);
            if (slot.tag == "bug")      SellItem(slot, "Bug", 100);            
        }
    }

    bool BuyItem(Slot slot, string itemCode, int needMoney) {
        onSlotClick(slot.item);
        // Debug.Log(slot.item.name);
        Message buyEvent = new Message("ShopManager/Buy : test, " + itemCode + ", 1").FunctionCall();
        bool isDone = (bool) buyEvent.returnValue[0];

        if(isDone == false)
        {
            Debug.Log("돈이 부족합니다. 구매 시도 아이템 = " + itemCode + ", 가격 = " + needMoney);
            return false;
        }
        else {
            return true;
        }
    }

    bool SellItem(Slot slot, string itemCode, int getMoney) {
        onSlotClick(slot.item);
        // Debug.Log(slot.item.name);

        Message sellEvent = new Message("ShopManager/Sell : test, " + itemCode + ", 1").FunctionCall();
        bool isDone = (bool) sellEvent.returnValue[0];

        if(isDone == false)
        {
            Debug.Log("판매할 아이템이 없습니다. 판매 시도 아이템 = " + itemCode);
            return false;
        }
        else {
            return true;
        }
    }
}
