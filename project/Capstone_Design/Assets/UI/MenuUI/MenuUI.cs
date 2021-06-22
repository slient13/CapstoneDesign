using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public MenuBase menuBase;
    public ConfirmSave confirmSave;
    public ConfirmClose confirmClose;
    public OptionMenu optionMenu;
    GameObject playerUI;

    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        playerUI = transform.GetChild(6).gameObject;
        menuBase     =     new MenuBase(this, transform.GetChild(2).gameObject);
        confirmSave  =  new ConfirmSave(this, transform.GetChild(4).gameObject);
        confirmClose = new ConfirmClose(this, transform.GetChild(3).gameObject);
        optionMenu   =   new OptionMenu(this, transform.GetChild(5).gameObject);
        menuBase.lootPanel.SetActive(false);
        confirmSave.lootPanel.SetActive(false);
        confirmClose.lootPanel.SetActive(false);
        optionMenu.lootPanel.SetActive(false);
        // 레이어용.
        MappingInfo mapping = new MappingInfo("MenuUI");
        mapping.Enroll("MenuUI");
    }
    public void OpenUI() {
        isActive = true;
        menuBase.lootPanel.SetActive(isActive);
        confirmSave.lootPanel.SetActive(false);
        confirmClose.lootPanel.SetActive(false);
        optionMenu.lootPanel.SetActive(false);
    }

    public void CloseUI() {
        isActive = false;
        menuBase.lootPanel.SetActive(isActive);
        confirmSave.lootPanel.SetActive(false);
        confirmClose.lootPanel.SetActive(false);
        optionMenu.lootPanel.SetActive(false);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void ConfirmSaveAction() {
        confirmSave.lootPanel.SetActive(true);
    }
    public void ConfirmCloseAction() {
        confirmClose.lootPanel.SetActive(true);
    }
    public void Sync() {
        playerUI.GetComponent<MenuPlayerUI>().Sync();
    }
}

public class MenuBase {
    public MenuUI menuUI;
    public GameObject lootPanel;
    public Button btn_continue;
    public Button btn_save;
    public Button btn_option;
    public Button btn_close;
    public MenuBase(MenuUI menuUI, GameObject lootPanel) {
        this.menuUI = menuUI;   // 해당 객체의 메소드를 쓸 일이 있음.
        this.lootPanel = lootPanel;
        this.btn_continue = lootPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        this.btn_save     = lootPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        this.btn_option   = lootPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        this.btn_close    = lootPanel.transform.GetChild(1).GetChild(3).GetComponent<Button>();
        btn_continue.onClick.AddListener(act_continue);
        btn_save.onClick.AddListener(act_save);
        btn_option.onClick.AddListener(act_option);
        btn_close.onClick.AddListener(act_close);
    }
    void act_continue() { menuUI.CloseUI(); }
    void act_save() {}
    void act_option() {}
    void act_close() {}
}

public class Confirm {
    public MenuUI menuUI;
    public GameObject lootPanel;
    public Button btn_yes;
    public Button btn_no;

    public Confirm(MenuUI menuUI, GameObject lootPanel) {
        this.menuUI = menuUI;   // 해당 객체의 메소드를 쓸 일이 있음.
        this.lootPanel = lootPanel;
        this.btn_yes = lootPanel.transform.GetChild(1).GetComponent<Button>();
        this.btn_no = lootPanel.transform.GetChild(2).GetComponent<Button>();
        btn_yes.onClick.AddListener(yes);
        btn_no.onClick.AddListener(no);
    }

    protected virtual void yes() {
        lootPanel.SetActive(false);
        menuUI.CloseUI();
    }
    protected void no() {
        lootPanel.SetActive(false);
    }
}

public class ConfirmSave : Confirm {
    public ConfirmSave(MenuUI menuUI, GameObject lootPanel) : base(menuUI, lootPanel) {}
    protected override void yes() {
        new Message("InventoryManager/SaveInventory :").FunctionCall();
        new Message("PlayInfoManager/SavePlayData : ").FunctionCall();
        base.yes();        
    }
}

public class ConfirmClose : Confirm {
    public ConfirmClose(MenuUI menuUI, GameObject lootPanel) : base(menuUI, lootPanel) {}
    protected override void yes() {
        new Message("GameProcessManager/CloseGame : ").FunctionCall(); 
        base.yes();
    }
}

public class OptionMenu {
    public MenuUI menuUI;
    public GameObject lootPanel;
    public OptionMenu(MenuUI menuUI, GameObject lootPanel) {
        this.menuUI = menuUI;
        this.lootPanel = lootPanel;
    }
}