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
            if (slot.tag == "Health")   buyItem(slot, "Health", 1000);
            if (slot.tag == "Fish")     buyItem(slot, "Fish", 700);
            if (slot.tag == "Bug")      buyItem(slot, "Bug", 100);

            // 판매
            if (slot.tag == "health")   sellItem(slot, "Health", 1000);
            if (slot.tag == "fish")     sellItem(slot, "Fish", 700);
            if (slot.tag == "bug")      sellItem(slot, "Bug", 100);            
        }
    }

    bool buyItem(Slot slot, string itemCode, int needMoney) {
        onSlotClick(slot.item);
        // Debug.Log(slot.item.name);

        Message msg_money = new Message("getPlayInfo: money").functionCall();
        int money = (int)msg_money.returnValue[0];

        if(money < needMoney)
        {
            Debug.Log("돈이 부족합니다. 구매 시도 아이템 = " + itemCode + ", 가격 = " + needMoney);
            return false;
        }
        else {
            new Message("playInfoChanger: money, " + -(needMoney)).functionCall();
            new Message("InventoryManager/modifyItem : " + itemCode + ", 1").functionCall();
            return true;
        }
    }

    bool sellItem(Slot slot, string itemCode, int getMoney) {
        onSlotClick(slot.item);
        // Debug.Log(slot.item.name);

        Message msg_item = new Message("InventoryManager/getItemNumber: " + itemCode).functionCall();
        int itemNumber = (int)msg_item.returnValue[0];

        if(itemNumber == 0)
        {
            Debug.Log("판매할 아이템이 없습니다. 판매 시도 아이템 = " + itemCode);
            return false;
        }
        else {
            new Message("playInfoChanger: money, " + getMoney).functionCall();
            new Message("InventoryManager/modifyItem : " + itemCode + ", -1").functionCall();
            return true;
        }
    }
}
