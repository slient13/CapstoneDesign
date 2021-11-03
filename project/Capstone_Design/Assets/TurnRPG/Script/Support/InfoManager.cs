using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
    {
    public InfoManager() { }

    public int GetHp() 
    {
        return (int) new Message($"GetPlayInfoValue : Player.Stat.Hp").FunctionCall().returnValue[0];
    }

    public void ChangeHp(int degree)
    {
        new Message($"ChangeData : Player.Stat.Hp, {degree}").FunctionCall();
    }

    public int GetAtk()
    {
        return (int) new Message($"GetPlayInfoValue : Player.Stat.Attack").FunctionCall().returnValue[0];
    }

    public int GetDef()
    {
        return (int) new Message($"GetPlayInfoValue : Player.Stat.Defense").FunctionCall().returnValue[0];
    }
    
    public Enemy GetEnemyInfo(string monster_code)
    {
        return ExternalFileSystem.SingleTon().GetEnemyInfo(monster_code);
    }

    List<string> item_code_list;
    List<int> item_count_list;

    public List<string> GetItemList()
    {
        List<ItemBox> itemBoxes = (List<ItemBox>) new Message("InventoryManager/GetItemBoxList : ").FunctionCall().returnValue[0];
        List<string> output = new List<string>();
        item_code_list = new List<string>();
        item_count_list = new List<int>();

        foreach (ItemBox itemBox in itemBoxes)
        {
            if (itemBox.itemType == "Consumable")
            {
                Item item = (Item) new Message($"InventoryManager/GetItem : {itemBox.itemCode}").FunctionCall().returnValue[0];
                output.Add(item.GetItemName());
                item_code_list.Add(item.GetItemCode());
                item_count_list.Add(itemBox.itemNumber);
            }
        }

        return output;
    }

    public List<int> GetItemCount()
    {
        GetItemList();
        return item_count_list;
    }

    public void UseItem(int index)
    {
        new Message($"InventoryManager/UseItem : {item_code_list[index]}").FunctionCall();
        GetItemList();
    }
}
