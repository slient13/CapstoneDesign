using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : MonoBehaviour
{
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

    public void StartSelect()
    {
        this.select_index = 0;
        itemNameList = infoManager.GetItemList();

        panel.SetActive(true);
        new Message($"ControlManager/LayerChanger : ItemSelect").FunctionCall();

        if (this.itemNameList.Count == 0)
        {
            this.select_index = -1;
            this.panelText.text = $"(보유한 아이템이 없습니다.)";
        }
        else 
            sync();
    }

    public void ChangeSelect(Message message)
    {
        if (this.select_index == -1) return;

        int direction = (int) message.args[0];
        this.select_index += direction;

        if (this.select_index < 0 && this.itemNameList.Count != 0)
            this.select_index = 0;
        else if (this.select_index >= this.itemNameList.Count)
            this.select_index = this.itemNameList.Count - 1;

        sync();
    }

    void sync()
    {
        List<string> output = new List<string>();

        int start = 0;
        {   // set start
            if (this.select_index < 4)
                start = 0;
            else if (this.select_index >= 4 && this.select_index < this.itemNameList.Count - 3)
                start = this.select_index - 3;
            else
                start = this.itemNameList.Count - 7;
        }

        for (int i = start, printCount = 0;
            i < this.itemNameList.Count && printCount < 7;
            ++i, ++printCount)
        {
            if (i == this.select_index) output.Add($"> {this.itemNameList[i]}");
            else output.Add($"{this.itemNameList[i]}");
        }

        string outputText = "";
        foreach(string line in output)
        {
            outputText += $"{line}\n";
        }
        this.panelText.text = outputText;
    }

    public void Select()
    {
        if (this.select_index == -1)
        {
            this.selectedItemName = "";
            this.selectedItemEffect = 0;
        }
        else 
        {
            this.selectedItemName = this.itemNameList[this.select_index];
            this.selectedItemEffect = infoManager.GetItemEffect(this.select_index);
        }
        this.CloseSelect();
    }

    public void Cancel()
    {
        this.CloseSelect();
    }

    void CloseSelect()
    {
        panel.SetActive(false);
        new Message($"ControlManager/LayerChanger : general").FunctionCall();
    }
}