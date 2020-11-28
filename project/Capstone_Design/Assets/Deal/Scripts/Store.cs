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
            onSlotClick(slot.item);
            Debug.Log(slot.item.name);

        }
    }

}
