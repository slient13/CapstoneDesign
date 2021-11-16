using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public class _EquipStatePanel
    {
        public _EquipStatePanel()
        {
            sprite_list = new List<Image>();
            altText_list = new List<Text>();
        }
        public List<Image> sprite_list;
        public List<Text> altText_list;
    }

    public class _StatPanel
    {
        public Text hp;
        public Text sp;
        public Text atk;
        public Text def;
        public Text temp_a;
        public Text temp_b;
    }

    public class _TootipPanel
    {
        public GameObject panel;
        public Text name;
        public Text contents;
    }

    public GameObject panel;
    public _EquipStatePanel equipStatePanel;
    public _StatPanel statPanel;
    public _TootipPanel tooltipPanel;

    int select_index = 0;
    bool isActive = false;
    public List<Equipment> equipment_list;
    private void Start()
    {
        equipStatePanel = new _EquipStatePanel();
        statPanel = new _StatPanel();
        tooltipPanel = new _TootipPanel();

        panel = transform.GetChild(0).gameObject;

        panel.SetActive(isActive);
        for (int i = 0; i < 4; ++i)
        {
            equipStatePanel.sprite_list.Add(
                panel.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<Image>());
            equipStatePanel.altText_list.Add(
                panel.transform.GetChild(1).GetChild(i).GetChild(1).GetComponent<Text>());
        }

        statPanel.hp = panel.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>();
        statPanel.atk = panel.transform.GetChild(2).GetChild(4).GetChild(0).GetComponent<Text>();
        statPanel.temp_a = panel.transform.GetChild(2).GetChild(5).GetChild(0).GetComponent<Text>();
        statPanel.sp = panel.transform.GetChild(2).GetChild(9).GetChild(0).GetComponent<Text>();
        statPanel.def = panel.transform.GetChild(2).GetChild(10).GetChild(0).GetComponent<Text>();
        statPanel.temp_b = panel.transform.GetChild(2).GetChild(11).GetChild(0).GetComponent<Text>();

        tooltipPanel.panel = transform.GetChild(1).gameObject;
        tooltipPanel.name = tooltipPanel.panel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        tooltipPanel.contents = tooltipPanel.panel.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        tooltipPanel.panel.SetActive(false);

        MappingInfo mapping = new MappingInfo("EquipmentUI");
        mapping.AddMapping("CloseUI : ", "esc");
        mapping.Enroll("EquipmentUI");

        MappingInfo mapping_toolitp = new MappingInfo("EquipmentUI");
        mapping_toolitp.AddMapping("CloseTooltipForMessage : 0", "esc");
        mapping_toolitp.AddMapping("CloseTooltipForMessage : 1", "mouseL");
        mapping_toolitp.Enroll("EquipmentUITooltip");
    }

    void sync()
    {
        this.equipment_list = (List<Equipment>)new Message("EquipmentSystem/GetEquipState : ").FunctionCall().returnValue[0];
        for (int i = 0; i < 4; ++i)
        {
            if (i >= this.equipment_list.Count)
            {
                this.equipStatePanel.sprite_list[i].sprite = null;
                this.equipStatePanel.altText_list[i].text = "";
            }
            else if (this.equipment_list[i].img == null)
            {
                this.equipStatePanel.sprite_list[i].sprite = null;
                this.equipStatePanel.altText_list[i].text = this.equipment_list[i].name;
            }
            else
            {
                this.equipStatePanel.sprite_list[i].sprite = this.equipment_list[i].img;
                this.equipStatePanel.altText_list[i].text = "";
            }
        }

        Message getHp = new Message("GetPlayInfo : Player.Stat.Hp").FunctionCall();
        PlayInfo playInfo_Hp = (PlayInfo)getHp.returnValue[0];
        PlayInfo playInfo_Hp_data = playInfo_Hp.GetDataList()[0];
        int hp_max = playInfo_Hp_data.GetRange()[1];
        this.statPanel.hp.text = $"{hp_max}";

        Message getSp = new Message("GetPlayInfo : Player.Stat.Sp").FunctionCall();
        PlayInfo playInfo_Sp = (PlayInfo)getSp.returnValue[0];
        PlayInfo playInfo_Sp_data = playInfo_Sp.GetDataList()[0];
        int sp_max = playInfo_Sp_data.GetRange()[1];
        this.statPanel.sp.text = $"{sp_max}";

        Message getAttack = new Message("GetPlayInfoValue : Player.Stat.Attack").FunctionCall();
        this.statPanel.atk.text = "" + (int)getAttack.returnValue[0];

        Message getDefence = new Message("GetPlayInfoValue : Player.Stat.Defense").FunctionCall();
        this.statPanel.def.text = "" + (int)getDefence.returnValue[0];

        this.statPanel.temp_a.text = "";
        this.statPanel.temp_b.text = "";
    }

    public void OpenUI(Message message)
    {
        this.sync();
        isActive = true;
        panel.SetActive(isActive);
    }

    public void CloseUI()
    {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void OpenTooltip(int index)
    {
        this.select_index = index;
        if (index < this.equipment_list.Count)
        {
            this.tooltipPanel.panel.SetActive(true);
            Vector2 pos = new MouseDetector().GetMousePos();
            this.tooltipPanel.panel.transform.position = new Vector3(pos.x, pos.y, 0);

            this.tooltipPanel.name.text = this.equipment_list[index].name;
            this.tooltipPanel.contents.text = this.equipment_list[index].GetDesc();

            new Message("ControlManager/LayerChanger : EquipmentUITooltip").FunctionCall();
        }
        else 
        {
            Debug.Log($"EquipmentUI.OpenTooltip.error : out of range. index = {index}, equipment count = {this.equipment_list.Count}");
        }
    }

    public void CloseTooltipForMessage(Message message)
    {
        int mode = (int) message.args[0];
        if (mode == 0) this.CloseTooltip(false);
        else if (mode == 1)
        {
            bool check = new MouseDetector(this.tooltipPanel.panel.transform).Trigger(MouseDetector.PinMode.C);
            if (check == false) this.CloseTooltip(false);
        }
    }

    public void CloseTooltip(bool isUnequip)
    {
        if (isUnequip) new Message($"EquipmentSystem/Unequip : {this.select_index}").FunctionCall();
        this.tooltipPanel.panel.SetActive(false);
        this.sync();
        new Message("ControlManager/LayerChanger : EquipmentUI").FunctionCall();
    }
}