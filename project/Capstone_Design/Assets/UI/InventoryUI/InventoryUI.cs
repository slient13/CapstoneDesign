using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;
    bool isActive = false;
    public int mode = 0;
    // 현재 보유 아이템 리스트를 저장하는 임시 변수.
    List<ItemBox> itemList;
    // 각 아이템 박스를 담아둔 변수.
    public List<ItemBox> equipmentList;
    public List<ItemBox> consumableList;
    public List<ItemBox> etcList;

    // 각 칸에 사용할 정보들.
    List<EquipmentInventory> equipmentInventories;
    List<ComsumableInventory> comsumableInventories;
    List<EtcInventory> etcInventories;

    // 돈 텍스트
    Text moneyText;
    // 툴팁 패널
    GameObject tooltipPanel;

    public const int INVENTORY_CAPACITY = 30;
    private void Awake()
    {
        // 자식 오브젝트 연결.
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(isActive);
        equipmentInventories = new List<EquipmentInventory>();
        comsumableInventories = new List<ComsumableInventory>();
        etcInventories = new List<EtcInventory>();
        Transform temp = panel.transform.GetChild(1);
        for (int i = 0; i < INVENTORY_CAPACITY; i++) {
            equipmentInventories.Add(
                new EquipmentInventory(
                    temp.GetChild(1).GetChild(0).GetChild(0).GetChild(i)));
            comsumableInventories.Add(
                new ComsumableInventory(
                    temp.GetChild(2).GetChild(0).GetChild(0).GetChild(i)));
            etcInventories.Add(
                new EtcInventory(
                    temp.GetChild(3).GetChild(0).GetChild(0).GetChild(i)));
        }
        moneyText = panel.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        tooltipPanel = panel.transform.GetChild(3).gameObject;
        tooltipPanel.SetActive(false);
        // 이벤트 연결.
        EventListener.GetEventListener().Binding("InventoryManager", "ModifyItem", "InventoryUICanvas/Sync : ");
        EventListener.GetEventListener().Binding("PlayInfoManager", "ChangeData", "InventoryUICanvas/Sync : ");
        // 키 바인딩.
        MappingInfo mapping = new MappingInfo("InventoryUICanvas");
        mapping.AddMapping("CloseUI : ", "esc");
        mapping.AddMapping("CheckHoveringOnBox : ", "!mouseL, mouseR");
        mapping.Enroll("InventoryUICanvas");
        // 기타 초기화.
        equipmentList = new List<ItemBox>();
        consumableList = new List<ItemBox>();
        etcList = new List<ItemBox>();
    }
    public void ChangeMode(int i) {
        this.mode = i;
    }
    public void Sync() {
        // 아이템 리스트 불러옴.
        Message getItemList = new Message("InventoryManager/GetItemBoxList : ").FunctionCall();
        itemList = (List<ItemBox>) getItemList.returnValue[0];
        // 기존 리스트 초기화.
        equipmentList.Clear();
        consumableList.Clear();
        etcList.Clear();
        foreach(ItemBox item in itemList) {
            if (item.itemType == "Equipment") equipmentList.Add(item);
            else if (item.itemType == "Consumable") consumableList.Add(item);
            else etcList.Add(item);
        }
        
        for (int i = 0; i < equipmentInventories.Count; i++) {
            if (i < equipmentList.Count) {
                ItemBox temp = equipmentList[i];
                equipmentInventories[i].SetItem(
                    temp.itemCode,
                    temp.itemImg,
                    temp.itemNumber);
            }
            else equipmentInventories[i].Clear();
        }
        for (int i = 0; i < comsumableInventories.Count; i++) {
            if (i < consumableList.Count) {
                ItemBox temp = consumableList[i];
                comsumableInventories[i].SetItem(
                    temp.itemCode,
                    temp.itemImg,
                    temp.itemNumber);
            }
            else comsumableInventories[i].Clear();
        }
        for (int i = 0; i < etcInventories.Count; i++) {
            if (i < etcList.Count) {
                ItemBox temp = etcList[i];
                etcInventories[i].SetItem(
                    temp.itemCode,
                    temp.itemImg,
                    temp.itemNumber);
            }
            else etcInventories[i].Clear();
        }
        Message getMoney = new Message("PlayInfoManager/GetData : Money").FunctionCall();
        int money = (int) getMoney.returnValue[0];
        moneyText.text = string.Format("{0:N0}", money);
    }

    public void OpenUI(Message message) {
        isActive = true;
        panel.SetActive(isActive);
        Sync();
    }

    public void CloseUI() {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void CheckHoveringOnBox() {
        MouseDetector detector = new MouseDetector();
        // 모드에 따라 임시 변수 할당.
        List<InventoryPanel> temp = null;
        // 클릭 확인.
        for (int i = 0; i < INVENTORY_CAPACITY; i++) {
            GameObject box;
            string itemCode;
            // mode 에 따라 box 할당.
            if (mode == 0 && equipmentInventories[i].isSetItem == true) {
                box = equipmentInventories[i].box;
                itemCode = equipmentInventories[i].itemCode;
            }
            else if (mode == 1 && comsumableInventories[i].isSetItem == true) {
                box = comsumableInventories[i].box;
                itemCode = comsumableInventories[i].itemCode;
            }
            else if (mode == 2 && etcInventories[i].isSetItem == true) {
                box = etcInventories[i].box;
                itemCode = etcInventories[i].itemCode;
            }
            else continue;
            // 타겟 설정.
            detector.TargetChange(box.transform);
            // 현재 마우스가 타겟 위에 있는지 확인. 있다면 현재 커서 위치에 툴팁 생성
            if (detector.Trigger()) {
                showTooltip(detector.GetMousePos(), itemCode);
                return;
            }
        }
        // 일치하는 케이스가 없다면 툴팁 닫기.
        CloseTooltip();
    }

    void showTooltip(Vector2 pos, string itemCode) {
        // 빈 공간 클릭 체크.
        if (itemCode == "") {
            CloseTooltip();
            return;            
        }

        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = new Vector3(pos.x, pos.y, 0);
        //
        tooltipPanel.transform.GetComponent<InventoryTooltip>().itemCode = itemCode;
        tooltipPanel.transform.GetComponent<InventoryTooltip>().Sync();
    }

    public void CloseTooltip() {
        tooltipPanel.SetActive(false);
    }
}

public class InventoryPanel {
    public GameObject box;
    public GameObject image;
    public GameObject numberText;
    public string itemCode;
    public bool isSetItem = false;

    public InventoryPanel() {}
    public InventoryPanel(GameObject box, GameObject image, GameObject numberText) {
        this.box = box;
        this.image = image;
        this.numberText = numberText;
    }
    public InventoryPanel(Transform box) {
        this.box = box.gameObject;
        this.image = box.GetChild(0).gameObject;
        this.numberText = box.GetChild(0).GetChild(0).GetChild(0).gameObject;
    }

    // protected void setClickListener() {
    //     this.box.GetComponent<Button>().onClick.AddListener(OpenTooltip);        
    // }
    public void SetItem(string itemCode, Sprite itemImage, int itemNumber) {
        isSetItem = true;
        this.itemCode = itemCode;
        box.SetActive(true);
        if (itemImage != null) image.GetComponent<Image>().sprite = itemImage;
        numberText.GetComponent<Text>().text = $"{itemNumber}";
    }
    public void Clear() {
        isSetItem = false;
        this.itemCode = "";
        box.SetActive(false);
    }

    // public void OpenTooltip() {
    //     Message openTooltip = new Message($"InventoryUICanvas/ShowTooltip : ");
    //     openTooltip.args(box.transform);
    // }
}

class EquipmentInventory : InventoryPanel {
    public EquipmentInventory (GameObject box, GameObject image) : base() {
        this.box = box;
        this.image = image;
        this.numberText = null;
    }

    public EquipmentInventory (Transform box) : base() {
        this.box = box.gameObject;
        this.image = box.GetChild(0).gameObject;
        this.numberText = null;
    }
    public void SetItem(string itemCode, Sprite itemImage, int itemNumber) {
        isSetItem = true;
        this.itemCode = itemCode;
        box.SetActive(true);
        if (itemImage != null) image.GetComponent<Image>().sprite = itemImage;
    }
}

class ComsumableInventory : InventoryPanel {
    public ComsumableInventory(GameObject box, GameObject image, GameObject numberText) : base(box, image, numberText){}
    public ComsumableInventory(Transform box) : base(box) {}
}

class EtcInventory : InventoryPanel {
    public EtcInventory(GameObject box, GameObject image, GameObject numberText) : base(box, image, numberText) {}
    public EtcInventory(Transform box) : base(box) {}
}