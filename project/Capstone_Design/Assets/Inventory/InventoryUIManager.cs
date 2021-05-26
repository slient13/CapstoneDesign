using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour {
    
    // 아이템 박스 프리팹
    GameObject itemBox;
    // 생성한 아이템 박스 리스트
    List<GameObject> itemBoxList = new List<GameObject>();
    // 매 업데이트마다 아이템 박스 리스트의 값을 받아와 그 아이템 코드를 임시로 저장하는 문자열 배열.
    List<string> itemBoxCodenameList = new List<string>();

    // 효율을 위해 저장해둔 각종 컴포넌트
    // 아이템 박스 판넬 각각의 이미지 컴포넌트 리스트
    List<Image> itemImageList = new List<Image>();
    // 아이템 박스 아이템 숫자 표시 오브젝트의 텍스트 컴포넌트 리스트
    List<Text> itemNumberList = new List<Text>();
    // 아이템 박스 아이템 숫자 표시 오브젝트의 백 판넬(활성화, 비활성화 용)
    List<GameObject> itemNumberBackboardList = new List<GameObject>();
    // 아이템 박스 대체 텍스트 오브젝트의 텍스트 컴포넌트 리스트
    List<Text> alterTextList = new List<Text>();
    
    // 기타 기본적으로 배치된 요소들.
    // 각 아이템 박스들이 배치될 상위 판넬 오브젝트
    public GameObject Content;
    // 툴팁 박스 오브젝트
    public GameObject tooltipPanel;
    // 우상단에 있는 종료 버튼
    public GameObject closeButton;
    
    int MAX_ITEM_BOX = 16; // 임시용이므로 16개로 한정.

    // 구조 
    /*
    Inventory : 
        InventoryWindow : 
            Content : 
            TooltipPanel : 
                ItemName : Text
                ItemTooltip : Text
                ItemEffect : Text
                #   ItemBox *16
            CloseButton : Text
    */
    private void Start() {
        // 아이템 박스 프리팹 로드
        itemBox = Resources.Load("Inventory/Prefab/ItemBox") as GameObject;
        // 아이템 판넬 오브젝트 저장
        Content = transform.GetChild(0).GetChild(1).gameObject;
        // tooltip 판넬 프리팹 로드 및 부모 설정 및         
        tooltipPanel = transform.GetChild(0).GetChild(2).gameObject;
        tooltipPanel.SetActive(false);
        // 종료 버튼 할당.
        closeButton = transform.GetChild(0).GetChild(0).gameObject;
        closeButton.GetComponent<Button>().onClick.AddListener(CloseInventory);        
        // 아이템 코드내임 리스트 세팅
        for (int i = 0; i < MAX_ITEM_BOX; i++) itemBoxCodenameList.Add("");
        // itemBox 배치자 실행
        itemBoxPlacer();

        MappingInfo mapping = new MappingInfo("Inventory");
        mapping.AddMapping("CloseInventory : ", "esc");
        mapping.AddMapping("CheckHoveringOnBox : ", "mouseL, !mouseR");
        mapping.Enroll("inventory");

        gameObject.SetActive(false);
    }

    void itemBoxPlacer() {
        GameObject tempObject;
        // 배치할 때 X 축으로 몇개를 더 배치해야 되는지에 대한 수. 최대 4.
        int remain_axleX = 0;
        for (int axleY = 0; axleY < MAX_ITEM_BOX / 4; axleY++) {
            remain_axleX = MAX_ITEM_BOX - axleY*4;
            if (remain_axleX > 4) remain_axleX = 4;
            for (int axleX = 0 ; axleX < remain_axleX; axleX++) {
                // 새 itemBox 판넬을 생성해서 배치함.
                tempObject = GameObject.Instantiate(itemBox);
                tempObject.transform.SetParent(Content.transform, true);
                tempObject.transform.localPosition = new Vector3(-195 + axleX*100, 195 - axleY*100, 0);
                // itemBox 판넬 자체의 Image 컴포넌트에 접근, 리스트에 저장함.
                itemImageList.Add(tempObject.transform.GetComponent<Image>());
                // itemBox 내 아이템 개수를 표시하는 자식 UI에 접근, 리스트에 저장함
                itemNumberBackboardList.Add(tempObject.transform.GetChild(0).gameObject);
                itemNumberList.Add(tempObject.transform.GetChild(0).GetChild(0).GetComponent<Text>());
                itemNumberList[(4*axleY + axleX)].text = "";
                // itemBox 내 alterText 를 표시하는 자식 UI에 접근, 리스트에 저장함
                alterTextList.Add(tempObject.transform.GetChild(1).GetComponent<Text>());
                alterTextList[(4*axleY + axleX)].text = "";
                // 해당 판넬 자체를 itemBoxList 에 저장함.
                itemBoxList.Add(tempObject);
            }
        }
    }

    private void Update() {
        // 아이템 리스트 불러옴.
        Message msg = new Message("InventoryManager/GetItemBoxList : ");
        msg.FunctionCall();
        List<ItemBox> tempItemBoxList = (List<ItemBox>)msg.returnValue[0];  // 아이템박스 리스트 전체
        // 현재 가진 아이템의 가짓 수 = 아이템 박스 개수.
        int box_count = (int) new Message("InventoryManager/GetItemBoxCount : ").FunctionCall().returnValue[0];
        for (int i = 0; i < MAX_ITEM_BOX; i++) {
            if (i < box_count && tempItemBoxList[i].itemNumber != 0) {
                // 아이템 코드를 저장
                itemBoxCodenameList[i] = tempItemBoxList[i].itemCode;
                // 개수 표시
                itemNumberBackboardList[i].SetActive(true);
                itemNumberList[i].text = "" + tempItemBoxList[i].itemNumber;
                // 이미지 표시. 없다면 대체 텍스트 표시
                if (tempItemBoxList[i].itemImg != null) {
                    itemImageList[i].sprite = tempItemBoxList[i].itemImg;
                    alterTextList[i].text = "";
                }
                else {
                    alterTextList[i].text = tempItemBoxList[i].itemCode;
                }
            }
            else {  // 해당 자리에 아이템이 없는 경우 싹다 안보이게 처리
                itemBoxCodenameList[i] = "";
                itemNumberList[i].text = "";
                itemNumberBackboardList[i].SetActive(false);
                alterTextList[i].text = "";
                itemImageList[i].sprite = null;
            }
        }
    }

    public void CloseInventory() {
        gameObject.SetActive(false);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void CheckHoveringOnBox() {
        MouseDetector detector = new MouseDetector();
        int findChecker = 0;
        foreach(GameObject box in itemBoxList) {
            // 타겟 설정.
            detector.TargetChange(box.transform);
            // 현재 마우스가 타겟 위에 있는지 확인. 있다면 현재 커서 위치에 툴팁 생성
            if (detector.Trigger()) {
                showTooltip(detector.GetMousePos(), itemBoxCodenameList[findChecker]);
                break;
            }
            else findChecker += 1;            
        }
        if (findChecker == itemBoxList.Count) closeTooltip();
    }

    /*
    TooltipPanel : 
        ItemName : Text
        ItemTooltip : Text
        ItemEffect : Text
        #   ItemBox *16
    */
    void showTooltip(Vector2 pos, string itemCode) {
        // 빈 공간 클릭 체크.
        if (itemCode == "") {
            closeTooltip();
            return;            
        }

        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = new Vector3(pos.x, pos.y, 0);
        string itemName;
        string itemTooltip;
        string itemEffect;
        Message msg = new Message("InventoryManager/GetItem : " + itemCode).FunctionCall();
        if (msg.returnValue.Count != 0) {
            Item item = (Item)msg.returnValue[0];
            itemName = item.GetItemName();
            itemTooltip = item.GetItemToolTip();
            itemEffect = "";
            foreach(string effectString in item.GetItemEffect()) itemEffect += effectString;
        }
        else {
            itemName = "";
            itemTooltip = "";
            itemEffect = "";
            closeTooltip();
        }

        // 텍스트 컴포넌트 추출.
        Text itemNameText       = tooltipPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Text itemTooltipText    = tooltipPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        Text itemItemEffectText = tooltipPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        itemNameText.text       = itemName;
        itemTooltipText.text    = itemTooltip;
        itemItemEffectText.text = itemEffect;

        new Message("InventoryManager/UseItem : " + itemCode).FunctionCall();
    }

    void closeTooltip() {
        tooltipPanel.SetActive(false);
    }
}