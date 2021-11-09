using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
    public BattleManager battleManager;
    public CommentPanel commentPanel;
    public BattleAudioPack audioPack;
    InfoManager infoManager;

    void Start()
    {
        MappingInfo mapping = new MappingInfo("ItemSelect");
        mapping.AddMapping("ChangeSelect : -1", "arrowU");
        mapping.AddMapping("ChangeSelect : -1", "w");
        mapping.AddMapping("ChangeSelect : -1", "a");
        mapping.AddMapping("ChangeSelect : 1", "arrowD");
        mapping.AddMapping("ChangeSelect : 1", "s");
        mapping.AddMapping("ChangeSelect : 1", "d");
        mapping.AddMapping("Cancel : ", "esc");
        mapping.AddMapping("Select : ", "enter");
        mapping.AddMapping("Select : ", "space");
        mapping.Enroll("ItemSelect");

        infoManager = new InfoManager();
        this.CloseSelect();
    }

    public GameObject panel;
    public Text panelText;
    //////////////////////////////
    public int select_index;
    public string selectedItemName = "";
    public int selectedItemEffect = 0;
    List<string> itemNameList;
    List<int> itemCountList;

    public void StartSelect()
    {
        this.select_index = -1;
        itemNameList = infoManager.GetItemList();
        itemNameList.Insert(0, "뒤로가기");
        itemCountList = infoManager.GetItemCount();

        panel.SetActive(true);
        new Message($"ControlManager/LayerChanger : ItemSelect").FunctionCall();

        sync();
    }

    public void ChangeSelect(Message message)
    {
        int direction = (int)message.args[0];
        this.select_index += direction;

        if (this.select_index < -1)
            this.select_index = -1;
        else if (this.select_index >= this.itemNameList.Count - 1)
            this.select_index = this.itemNameList.Count - 2;

        sync();
    }

    void sync()
    {
        List<string> output = new List<string>();

        int start = 0;
        int offset_index = this.select_index + 1;
        {   // set start
            if (offset_index < 4)
                start = 0;
            else if (offset_index >= 4 && offset_index < this.itemNameList.Count - 3)
                start = offset_index - 3;
            else
                start = this.itemNameList.Count - 7;
        }

        for (int i = start, printCount = 0;
            i < this.itemNameList.Count && printCount < 7;
            ++i, ++printCount)
        {
            if (i == offset_index)
            {
                if (i == 0) output.Add($"> {this.itemNameList[i]}");
                else output.Add($"> {this.itemNameList[i]} : {this.itemCountList[i-1]}");
            }
            else
            {
                if (i == 0) output.Add($"{this.itemNameList[i]}");
                else output.Add($"{this.itemNameList[i]} : {this.itemCountList[i-1]}");
            }
        }

        string outputText = "";
        foreach (string line in output)
        {
            outputText += $"{line}\n";
        }
        this.panelText.text = outputText;
    }

    public void Select()
    {
        if (this.select_index == -1)
        {
            //선택한 아이템 없을시
            battleManager.SetCommandPanel(true);
            this.Cancel();
        }
        else
        {
            this.selectedItemName = this.itemNameList[this.select_index + 1];
            this.selectedItemEffect = infoManager.GetItemEffect(this.select_index);

            //선택한 아이템 있을시
            commentPanel.ItemUse(selectedItemName);
            battleManager.AddPlayerHp(selectedItemEffect);
            infoManager.UseItem(select_index);
        }
        this.CloseSelect();
    }

    public void Cancel()
    {
        this.selectedItemName = "";
        this.selectedItemEffect = 0;
        this.CloseSelect();
    }

    void CloseSelect()
    {
        panel.SetActive(false);
        new Message($"ControlManager/LayerChanger : general").FunctionCall();
    }
}