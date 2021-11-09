using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager
{
    public InfoManager() { }

    public string GetSceneStartValue()
    {
        return (string)new Message($"GetPlayInfoValue : System.Process.SceneChangeValue").FunctionCall().returnValue[0];
    }

    public int GetHp()
    {
        return (int)new Message($"GetPlayInfoValue : Player.Stat.Hp").FunctionCall().returnValue[0];
    }

    public int GetSp()
    {
        return (int)new Message($"GetPlayInfoValue : Player.Stat.Sp").FunctionCall().returnValue[0];
    }

    public void ChangeHp(int degree)
    {
        new Message($"ChangeData : Player.Stat.Hp, {degree}").FunctionCall();
    }

    public void ChangeSp(int degree)
    {
        new Message($"ChangeData : Player.Stat.Sp, {degree}").FunctionCall();
    }

    public int GetAtk()
    {
        int output = (int)new Message($"GetPlayInfoValue : Player.Stat.Attack").FunctionCall().returnValue[0];
        output = output * (GetSp() + 100) / 200;
        return output;
    }

    public int GetDef()
    {
        int output = (int)new Message($"GetPlayInfoValue : Player.Stat.Defense").FunctionCall().returnValue[0];
        output = output * (GetSp() + 100) / 200;
        return output;
    }

    public int GetEvasion()
    {
        return (int)new Message($"GetPlayInfoValue : Player.Stat.Evasion").FunctionCall().returnValue[0];
    }

    public Enemy GetEnemyInfo(string monster_code)
    {
        return ExternalFileSystem.SingleTon().GetEnemyInfo(monster_code);
    }

    List<string> item_code_list;
    List<int> item_count_list;

    public List<string> GetItemList()
    {
        List<ItemBox> itemBoxes = (List<ItemBox>)new Message("InventoryManager/GetItemBoxList : ").FunctionCall().returnValue[0];
        List<string> output = new List<string>();
        item_code_list = new List<string>();
        item_count_list = new List<int>();

        foreach (ItemBox itemBox in itemBoxes)
        {
            if (itemBox.itemType == "Consumable")
            {
                Item item = (Item)new Message($"InventoryManager/GetItem : {itemBox.itemCode}").FunctionCall().returnValue[0];
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

    // 0 = 끄기, 1 = 켜기, 그 외 = 토글.
    public void SetSpRecoveryOnOff(int mode)
    {
        new Message($"PlayInfoManager/SetSpRecoveryOnOff : {mode}").FunctionCall();
    }

    GameObject player;
    public Vector3 GetPlayerLastPos()
    {
        float x, y, z;
        x = (float)new Message($"GetPlayInfoValue : System.Player.pos_x").FunctionCall().returnValue[0];
        y = (float)new Message($"GetPlayInfoValue : System.Player.pos_y").FunctionCall().returnValue[0];
        z = (float)new Message($"GetPlayInfoValue : System.Player.pos_z").FunctionCall().returnValue[0];
        Vector3 output = new Vector3(x, y, z);

        return output;
    }

    public void SavePlayerPos()
    {
        if (player == null) this.player = GameObject.Find("Player");
        
        this.SetPlayerPosData(player.transform.position);        
    }

    public void SetPlayerPosData(Vector3 targetPos, bool isMove = false)
    {
        new Message($"SetData : System.Player.pos_x")
            .AddArg(targetPos.x)
            .FunctionCall();
        new Message($"SetData : System.Player.pos_y")
            .AddArg(targetPos.y)
            .FunctionCall();
        new Message($"SetData : System.Player.pos_z")
            .AddArg(targetPos.z)
            .FunctionCall();

        if (isMove == true)
        {
            this.SetPlayerPosLast();
        }
    }

    public void SetPlayerPosData(float x, float y, float z, bool isMove = false)
    {
        this.SetPlayerPosData(new Vector3(x, y, z), isMove);
    }

    public void SetPlayerPosLast()
    {
        if (player == null) this.player = GameObject.Find("Player");

        player.transform.position = this.GetPlayerLastPos();
    }

    public void GetLastSceneName()
    {
        return (string) new Message($"GetPlayInfoValue : System.Process.LastSceneName").FunctionCall().returnValue[0];
    }
}
