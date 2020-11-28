using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    List<Item> itemList = new List<Item>();
    List<ItemBox> itemBoxList = new List<ItemBox>();
    int MAX_ITEM_BOX = 16;

    void Start() {
        for (int i = 0; i < MAX_ITEM_BOX; i++) {
            itemBoxList.Add(new ItemBox());
        }
    }

    // 외부에서 추가하는 용도
    public void addNewItem(Message msg) {
        string itemCode = (string) msg.args[0];     // 아이템 코드(중복 불가)
        string itemName = (string) msg.args[1];     // 아이템 이름(중복 가능)
        string itemTooltip = (string) msg.args[2];  // 아이템 툴팁
        string itemEffect = (string) msg.args[3];   // 아이템 효과(명령어 양식)

        // 일치하는 정보를 가진 아이템이 존재하는 경우 중단.
        if (isInItemList(itemCode)) {
            Debug.Log("InventoryManager/addNewItem.error : the item which codeName is " + itemCode + " is already exist");
            msg.returnValue.Add(false)
            return;
        }

        msg.returnValue.Add(true)
        itemList.Add(new Item(itemCode, itemName, itemTooltip, itemEffect));
    }

    public void modifyItem(Message msg) {
        string itemCode = (string)msg.args[0];  // 변경 아이템 코드
        int itemNum = (int)msg.args[1];         // 변경 아이템 개수

        // 추가하려는 아이템이 itemList 에 없는 경우 중단.
        if (!isInItemList(itemCode)) {
            Debug.Log("InventoryManager/modifyItem.error : there is no item information which codeName is " + itemCode);
            return;
        }
        // 해당 코드의 아이템이 이미 인벤토리에 들어있는지 확인
        int index = matchItemBox(itemCode);
        // 들어있다면 해당 아이템 박스의 아이템 개수를 변경
        if (index != -1) itemBoxList[index].changeNumber(itemNum);
        // 들어있지 않다면 비어있는 아이템 박스를 찾아 가장 빠른 위치에 새로 추가.
        else {
            index = findEmptySlot();
            itemBoxList[index].changeItem(itemCode, itemNum);
        }
        // 결과로 아이템 개수가 0개가 된다면 해당 아이템 박스를 비워버림.
        if (itemBoxList[index].itemNumber == 0) itemBoxList[index].reset();
    }

    // 변경을 원하는 아이템의 정보가 itemList 에 기록되어 있는지 확인
    bool isInItemList(string itemCode) {
        int i;
        for (i = 0; i < itemList.Count;) {
            if (itemList[i].getItemCode() == itemCode) break;
            else i++;
        }
        if (i < itemList.Count) return true;
        else return false;
    }

    // 해당 코드의 아이템이 이미 인벤토리에 들어있는지 확인
    int matchItemBox(string itemCode) {
        int i;
        for (i = 0; i < MAX_ITEM_BOX;) {
            if (itemBoxList[i].itemCode == itemCode) break;
            else i++;
        }
        if (i == MAX_ITEM_BOX) return -1;
        else return i;
    }

    int findEmptySlot() {
        int i;
        for (i = 0; i < MAX_ITEM_BOX;) {
            if (itemBoxList[i].itemCode.Length == 0) break;
            else i++;
        }
        if (i == MAX_ITEM_BOX) return -1;
        else return i;
    }

    public void itemMove(int beforePos, int afterPos) {
        // 이전 위치의 아이템 박스가 비어있는 경우 중단.
        if (itemBoxList[beforePos].itemNumber == 0) return;
        // 다음 위치의 아이템 박스가 비어있는 경우 단순히 이동
        else if(itemBoxList[afterPos].itemNumber == 0) {
            itemBoxList[afterPos].changeItem(itemBoxList[beforePos]);
            itemBoxList[beforePos].reset();
        }
        // 둘 다 비어있지 않은 경우 서로의 위치를 교환.
        else {
            ItemBox tempItemBox = new ItemBox();
            tempItemBox = itemBoxList[afterPos];
            itemBoxList[afterPos].changeItem(itemBoxList[beforePos]);
            itemBoxList[beforePos].changeItem(tempItemBox);
        }
    }

    public void getItemBoxList(Message message) {
        message.returnValue.Add(itemBoxList);
    }

    public void getItemNumber(Message message) {
        string itemCode = (string)message.args[0];  // 개수를 원하는 아이템의 이름
        int number;

        int index = matchItemBox(itemCode);
        if (index != -1) number = itemBoxList[index].itemNumber;
        else number = -1;

        message.returnValue.Add(number);
    }
}