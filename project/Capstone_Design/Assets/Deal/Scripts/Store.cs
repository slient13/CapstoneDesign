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

            if (slot.tag == "Health")
            {
                onSlotClick(slot.item);
                Debug.Log(slot.item.name);

                Message msg13 = new Message("getPlayInfo: Health");
                msg13.functionCall();
                int Health = (int)msg13.returnValue[0];

                /*
                Message msg3 = new Message("playInfoChanger: Health, 10");
                msg3.functionCall();
                */

                Message msg14 = new Message("playInfoChanger: money, -1000");
                msg14.functionCall();

                Message msg35 = new Message("getPlayInfo: money");
                msg35.functionCall();
                int money = (int)msg35.returnValue[0];

                Message msg15 = new Message("InventoryManager/modifyItem : Health, 1");
                msg15.functionCall();

               if(money <= 0)
                {
                    Debug.Log("체력을 구매 할 돈이 부족합니다.");
                }
            }


            if (slot.tag == "Fish")
            {
                onSlotClick(slot.item);
                Debug.Log(slot.item.name);

                Message msg16 = new Message("getPlayInfo: Fish");
                msg16.functionCall();
                int Fish = (int)msg16.returnValue[0];

                Message msg17 = new Message("playInfoChanger: Fish, 1");
                msg17.functionCall();

                Message msg18 = new Message("playInfoChanger: money, -700");
                msg18.functionCall();

                Message msg36 = new Message("getPlayInfo: money");
                msg36.functionCall();
                int money = (int)msg36.returnValue[0];

                Message msg19 = new Message("InventoryManager/modifyItem : Fish, 1");
                msg19.functionCall();

                if (money <= 0)
                {
                    Debug.Log("물 고 기를 구매 할 돈이 부족합니다.");
                }
            }


            if (slot.tag == "Bug")
            {
                onSlotClick(slot.item);
                Debug.Log(slot.item.name);

                Message msg20 = new Message("getPlayInfo: Bug");
                msg20.functionCall();
                int Bug = (int)msg20.returnValue[0];

                Message msg21 = new Message("playInfoChanger: Bug, 1");
                msg21.functionCall();

                Message msg22 = new Message("playInfoChanger: money, -100");
                msg22.functionCall();

                Message msg37 = new Message("getPlayInfo: money");
                msg37.functionCall();
                int money = (int)msg37.returnValue[0];

                Message msg23 = new Message("InventoryManager/modifyItem : Bug, 1");
                msg23.functionCall();

                if (money <= 0)
                {
                    Debug.Log("미 끼를 구매 할 돈이 부족합니다.");
                }
            }

            if (slot.tag == "health")
            {
                onSlotClick(slot.item);
                Debug.Log(slot.item.name);

                Message msg24 = new Message("getPlayInfo: Health");
                msg24.functionCall();
                int Health = (int)msg24.returnValue[0];

                /*
                Message msg3 = new Message("playInfoChanger: Health, 10");
                msg3.functionCall();
                */

                Message msg25 = new Message("playInfoChanger: money, 1000");
                msg25.functionCall();

                Message msg38 = new Message("getPlayInfo: money");
                msg38.functionCall();
                int money = (int)msg38.returnValue[0];

                Message msg26 = new Message("InventoryManager/modifyItem : Health, -1");
                msg26.functionCall();

                if (Health <= 0)
                {
                    Debug.Log("더 이상 판매할 체력이 없습니다.");
                }
            }


            if (slot.tag == "fish")
            {
                onSlotClick(slot.item);
                Debug.Log(slot.item.name);

                Message msg27 = new Message("getPlayInfo: Fish");
                msg27.functionCall();
                int Fish = (int)msg27.returnValue[0];

                Message msg28 = new Message("playInfoChanger: Fish, -1");
                msg28.functionCall();

                Message msg29 = new Message("playInfoChanger: money, 700");
                msg29.functionCall();

                Message msg39 = new Message("getPlayInfo: money");
                msg39.functionCall();
                int money = (int)msg39.returnValue[0];

                Message msg30 = new Message("InventoryManager/modifyItem : Fish, -1");
                msg30.functionCall();

                if (Fish <= 0)
                {
                    Debug.Log("더 이상 판매할 물고기가 없습니다.");
                }
            }


            if (slot.tag == "bug")
            {
                onSlotClick(slot.item);
                Debug.Log(slot.item.name);

                Message msg31 = new Message("getPlayInfo: Bug");
                msg31.functionCall();
                int Bug = (int)msg31.returnValue[0];

                Message msg32 = new Message("playInfoChanger: Bug, -1");
                msg32.functionCall();

                Message msg33 = new Message("playInfoChanger: money, 100");
                msg33.functionCall();

                Message msg40 = new Message("getPlayInfo: money");
                msg40.functionCall();
                int money = (int)msg40.returnValue[0];

                Message msg34 = new Message("InventoryManager/modifyItem : Bug, -1");
                msg34.functionCall();

                if (Bug <= 0)
                {
                    Debug.Log("더 이상 판매할 미끼가 없습니다.");
                }
            }
        }
    }

    /*
    public void OnClickSellSlot(Slot slot)
    {
        //Debug.Log(slot.name);
        if (onSlotClick != null)
        {
            onSlotClick(slot.item);
            Debug.Log(slot.item.name);

            if (slot.tag == "health")
            {

                Message msg24 = new Message("getPlayInfo: Health");
                msg24.functionCall();
                int Health = (int)msg24.returnValue[1];

                /*
                Message msg3 = new Message("playInfoChanger: Health, 10");
                msg3.functionCall();
                

                Message msg25 = new Message("playInfoChanger: money, 1000");
                msg25.functionCall();

                Message msg26 = new Message("InventoryManager/modifyItem : Health, -1");
                msg26.functionCall();

                if (Health > 100)
                {
                    Debug.Log("더 이상 올릴 체력이 없습니다.");

                }
            }


            if (slot.tag == "fish")
            {

                Message msg27 = new Message("getPlayInfo: Fish");
                msg27.functionCall();
                int Fish = (int)msg27.returnValue[1];

                Message msg28 = new Message("playInfoChanger: Fish, -1");
                msg28.functionCall();

                Message msg29= new Message("playInfoChanger: coin, 700");
                msg29.functionCall();

                Message msg30 = new Message("InventoryManager/modifyItem : Fish, -1");
                msg30.functionCall();

                if (Fish == 0)
                {
                    Debug.Log("더 이상 판매할 물고기가 없습니다.");
                }
            }


            if (slot.tag == "bug")
            {

                Message msg31 = new Message("getPlayInfo: Bug");
                msg31.functionCall();
                int Bug = (int)msg31.returnValue[1];

                Message msg32 = new Message("playInfoChanger: Bug, -1");
                msg32.functionCall();

                Message msg33 = new Message("playInfoChanger: coin, 100");
                msg33.functionCall();

                Message msg34 = new Message("InventoryManager/modifyItem : Bug, -1");
                msg34.functionCall();

                if (Bug == 0)
                {
                    Debug.Log("더 이상 판매할 미끼가 없습니다.");
                }
            }
        }
    */
    
}
