using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public List<Item> itemList = new List<Item>();                 // 등록된 아이템의 리스트
    public List<Sprite> itemImageList = new List<Sprite>(); // 등록된 아이템의 이미지 리스트.
    public List<ItemBox> itemBoxList = new List<ItemBox>(); // 아이템 박스 리스트

    // 아이템 최대 수량. 현재 사용하지 않음.
    // int MAX_ITEM_BOX = 999;
    int box_count = 0;

    // 아이템 정보가 기술된 위치들.
    string[] itemInfoPathList = {
        "Inventory/ItemInfo", 
        "Inventory/Test"
    };

    void Start() {
        LoadItemInfo();
        new Message("InventoryManager/LoadInventory : ").FunctionCall();
    }

    // 외부 파일에 저장된 아이템 리스트를 읽어와 저장하는 코드.
    private void LoadItemInfo() {
        List<string> itemInfoStringList = ExternalFileSystem.SingleTon().GetItemInfo(itemInfoPathList);
        string code = "";       // 아이템 코드(중복 불가)
        string type = "";       // 아이템 이름(중복 가능)
        string name = "";       // 아이템 타입.
        string tooltip = "";    // 아이템 툴팁
        List<string> effect = new List<string>();   // 아이템 효과(명령어 양식)
        foreach(string itemInfoString in itemInfoStringList) {
            // end를 만나면 묶어서 새로은 정보 등록.
            if (itemInfoString == "end") {
                // code 가 없으면 오류로 판단, 스킵.
                if (code == "") {
                    Debug.Log("InventoryManager/LoadItemInfo.error : No item code input.");
                    code = "";
                    type = "";
                    name = "";
                    tooltip = "";
                    continue;
                }
                // type 이 없으면, general 로 초기화.
                if (type == "") type = "general";
                // name 이 없으면 code 값으로 초기화.
                if (name == "") name = code;
                // if (effect.Count == 0) effect.Add("None");
                Message msg = new Message($"InventoryManager/AddNewItem : {code}, {type}, {name}, {tooltip}");
                msg.args.Add(effect);
                msg.FunctionCall();
                Debug.Log($"InventoryManager/LoadItemInfo : [{code}] is added.");
                code = "";
                type = "";
                name = "";
                tooltip = "";
                effect = new List<string>();
            }
            // 그 외에는 정보에 대입.
            else {
                string[] temp = itemInfoString.Split('=');
                string mode = temp[0].Trim();
                string value = temp[1].Trim();
                if (mode == "short") {
                    Debug.Log("InventoryManager/LoadItemInfo : shortCut is detected.");
                    string[] shortInfoList = value.Split(',');
                    type = shortInfoList[0].Trim();
                    code = shortInfoList[1].Trim();
                    name = shortInfoList[2].Trim();
                    tooltip = shortInfoList[3].Trim();
                    string effectString = "";
                    for (int i = 4; i < shortInfoList.Length; i++) effectString += shortInfoList[i];
                    effectString = effectString.Trim();
                    if (effectString != "") effect.Add(effectString);
                    // 반영.                        
                    Message msg = new Message($"InventoryManager/AddNewItem : {code}, {type}, {name}, {tooltip}");
                    msg.args.Add(effect);
                    msg.FunctionCall();
                    Debug.Log($"InventoryManager/LoadItemInfo : [{code}] is added.");
                    code = "";
                    type = "";
                    name = "";
                    tooltip = "";
                    effect = new List<string>();
                }
                else if (mode == "code")    code = value;
                else if (mode == "type")    type = value;
                else if (mode == "name")    name = value;
                else if (mode == "desc")    tooltip = value;
                else if (mode == "effect")  effect.Add(value);
            }
        }
    }
    // 아이템 정보 등록
    public void AddNewItem(Message msg) {
        string itemCode = (string) msg.args[0];     // 아이템 코드(중복 불가)
        string itemType = (string) msg.args[1];     // 아이템 타입.
        string itemName = (string) msg.args[2];     // 아이템 이름(중복 가능)
        string itemTooltip = (string) msg.args[3];  // 아이템 툴팁
        List<string> itemEffect = (List<string>) msg.args[4];   // 아이템 효과(명령어 양식)

        // 일치하는 정보를 가진 아이템이 존재하는 경우 중단.
        if (isInItemList(itemCode)) {
            Debug.Log("InventoryManager/AddNewItem.error : the item which codeName is " + itemCode + " is already exist");
            msg.returnValue.Add(false);
            return;
        }
        string imgPath = "Inventory/ItemImage/" + itemCode;
        Sprite img = Resources.Load<Sprite>(imgPath) as Sprite;
        itemImageList.Add(img);
        itemList.Add(new Item(itemCode, itemType, itemName, itemTooltip, itemEffect, img));
        msg.returnValue.Add(true);
    }
    // 인벤토리 정보를 불러오는 메소드.
    public void LoadInventory(Message message) {
        Debug.Log("InventoryManager/LoadInventory : is called.");
        List<string> itemDataList = ExternalFileSystem.SingleTon().LoadInventory();
        foreach(string itemData in itemDataList) {
            new Message("InventoryManager/ModifyItem : " + itemData).FunctionCall();   
        }
    }
    // 아이템 소지 정보를 저장하는 메소드.
    public void SaveInventory(Message message) {
        Debug.Log("InventoryManager/SaveInventory : is called.");
        ExternalFileSystem.SingleTon().SaveInventory(itemBoxList);
    }
    // 아이템의 이름이나 툴팁, 효과등을 받아오는 함수.
    public void GetItem(Message msg) {
        if (msg.args.Count == 0) {
            Debug.Log("InventoryManager/GetItemInfo.error : There is no Input");
            return;
        }

        string itemCode = (string)msg.args[0];  // 정보를 원하는 아이템의 코드.

        // 아이템 등록 확인
        if (!isInItemList(itemCode)) {
            Debug.Log("InventoryManager/GetItemInfo.error : there is no item which code is " + itemCode);
            return;
        }

        foreach(Item item in itemList) {
            if (item.GetItemCode() == itemCode) msg.returnValue.Add(item);
        }
    }

    // 아이템의 보유 개수를 반환하는 함수.
    public void GetItemCount(Message message) {
        string itemCode = (string) message.args[0];
        int index = matchItemBox(itemCode);
        int count = 0;
        if (index != -1) count = itemBoxList[index].itemNumber;
        message.returnValue.Add(count);
    }

    public void ModifyItem(Message msg) {
        string itemCode = (string)msg.args[0];  // 변경 아이템 코드
        int itemNum = (int)msg.args[1];         // 변경 아이템 개수
        string itemType = findType(itemCode);
        Sprite itemImg = findImage(itemCode);

        // 추가하려는 아이템이 itemList 에 없는 경우 중단.
        if (!isInItemList(itemCode)) {
            Debug.Log("InventoryManager/ModifyItem.error : there is no item information which codeName is " + itemCode);
            return;
        }
        // 해당 코드의 아이템이 이미 인벤토리에 들어있는지 확인
        int index = matchItemBox(itemCode);
        // 들어있다면 해당 아이템 박스의 아이템 개수를 변경
        if (index != -1) itemBoxList[index].changeNumber(itemNum);
        // 들어있지 않다면 새 아이템 박스를 생성해서 추가.
        else {
            newItemBox();
            index = box_count - 1;
            itemBoxList[index].changeItem(itemType, itemCode, itemNum, itemImg);
        }
        // 결과로 아이템 개수가 0개가 된다면 해당 아이템 박스를 비워버림.
        if (itemBoxList[index].itemNumber == 0) deleteItemBox(index);
    }

    string findType(string itemCode) {
        string output = null;
        int i;
        for (i = 0; i < itemList.Count;) {
            if (itemList[i].GetItemCode() == itemCode) {
                output = itemList[i].GetItemType();
                break;
            }
            else i++;
        }
        if (i < itemList.Count) return output;
        else return null;
    }

    Sprite findImage(string itemCode) {
        Sprite img = null;
        int i;
        for (i = 0; i < itemList.Count;) {
            if (itemList[i].GetItemCode() == itemCode) {
                img = itemImageList[i];
                break;
            }
            else i++;
        }
        if (i < itemList.Count) return img;
        else return null;
    }

    // 변경을 원하는 아이템의 정보가 itemList 에 기록되어 있는지 확인
    bool isInItemList(string itemCode) {
        int i;
        for (i = 0; i < itemList.Count;) {
            if (itemList[i].GetItemCode() == itemCode) break;
            else i++;
        }
        if (i < itemList.Count) return true;
        else return false;
    }

    // 해당 코드의 아이템이 이미 인벤토리에 들어있는지 확인
    int matchItemBox(string itemCode) {
        int i;
        for (i = 0; i < box_count;) {
            if (itemBoxList[i].itemCode == itemCode) break;
            else i++;
        }
        if (i == box_count) return -1;
        else return i;
    }

    // 새로운 박스를 생성함.
    void newItemBox() {
        itemBoxList.Add(new ItemBox());
        box_count += 1;
    }

    // 아이템 박스를 제거함.
    void deleteItemBox(int index) {
        itemBoxList.RemoveAt(index);
        box_count -= 1;
    }

    // 가장 빠른 빈 슬롯 반환. 없으면 -1 반환.
    // 설계 변경으로 사용하지 않음.
    /*
    int findEmptySlot() {
        int i;
        for (i = 0; i < MAX_ITEM_BOX;) {
            if (itemBoxList[i].itemCode.Length == 0) break;
            else i++;
        }
        if (i == MAX_ITEM_BOX) return -1;
        else return i;
    }
    */

    public void ItemMove(int beforePos, int afterPos) {
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

    public void GetItemBoxList(Message message) {
        message.returnValue.Add(itemBoxList);
    }

    // Get items' number in inventory.
    public void GetItemNumber(Message message) {
        string itemCode = (string)message.args[0];  // 개수를 원하는 아이템의 이름
        int number;

        int index = matchItemBox(itemCode);
        if (index != -1) number = itemBoxList[index].itemNumber;
        else number = 0;

        message.returnValue.Add(number);
    }

    public void GetItemBoxCount(Message message) {
        message.returnValue.Add(box_count);
    }

    // 아이템 사용
    public void UseItem(Message message) {
        string itemCode = (string) message.args[0];
        int index = matchItemBox(itemCode);
        if (index != -1) {
            // 해당 아이템 코드를 기반으로 아이템 효과 정보를 받아옴.
            List<string> commandList = null;
            foreach (Item item in itemList) {
                if (itemCode == item.GetItemCode()) commandList = item.GetItemEffect();
            }
            // 해당 아이템의 효과가 없다면 그대로 중단.(사용 안됨.)
            if (commandList.Count == 0) {
                Debug.Log($"InventoryManager.UseItem.Error : Item-{itemCode} has no effect. It can't be used.");
                return;
            }
            // 사용 아이템 개수 감소.
            new Message($"InventoryManager/ModifyItem : {itemCode}, -1").FunctionCall();
            // 아이템 효과 적용.
            for (int i = 0; i < commandList.Count; i++) {
                // 약식인 경우 대응 형태로 변경.
                if (commandList[i].IndexOf('/') == -1) {
                    string[] data = commandList[i].Split(':');                
                    commandList[i] = "PlayInfoManager/ChangeData : " + data[0] + ", " + data[1];
                } 
                // 효과 실행.
                new Message(commandList[i]).FunctionCall();
            }
        }
    }
}