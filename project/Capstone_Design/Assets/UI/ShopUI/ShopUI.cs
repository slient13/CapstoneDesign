using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject panel;
    public Trade buyTrade;
    public Trade sellTrade;
    public Vector2 posBuffer;           // 직전 클릭 위치 저장. 더블클릭 구현용.
    public int mode = 0;                // 상점 모드. 0 = 구매, 1 = 판매.
    public bool isActive = false;       // 활성화 여부.
    void Awake() {
    }

    void Start() {
        // 가장 바탕의 판넬 지정
        panel = transform.GetChild(0).gameObject;                
        // 각 패널과 리스트를 저장.
        buyTrade = new Trade(panel.transform.GetChild(2).gameObject, panel.transform.GetChild(4).gameObject);
        sellTrade = new Trade(panel.transform.GetChild(7).gameObject, panel.transform.GetChild(6).gameObject);
        // 더블 클릭 체크용 버퍼 초기화.
        posBuffer = new Vector2(0, 0);
        panel.SetActive(false);
        // 매핑
        MappingInfo mapping = new MappingInfo("ShopUI");
        mapping.AddMapping("OnDoubleClick : ", "mouseL");
        mapping.AddMapping("CloseUI : ", "esc");
        mapping.Enroll("ShopUI");
    }

    void setShopInfo(string shopCode) {
        // 상점 정보 받아옴.
        Message GetShop = new Message($"ShopManager/GetShop : {shopCode}").FunctionCall();
        ShopInfo shop = (ShopInfo) GetShop.returnValue[0];
        if (shop == null) return;
        // 구매 가능 아이템 목록 추출, 가공.
        List<ShopItem> buyItemList = new List<ShopItem>();
        foreach(KeyValuePair<string, int> buyData in shop.buyList) {
            buyItemList.Add(new ShopItem(buyData.Key, buyData.Value));
        }
        // 판매 가능 아이템 목록 추출 및 인벤토리 검사, 가공.
        Message GetItemBoxList = new Message($"InventoryManager/GetItemBoxList : ").FunctionCall();
        List<ItemBox> itemBoxList = (List<ItemBox>) GetItemBoxList.returnValue[0];

        List<ShopItem> sellItemList = new List<ShopItem>();
        foreach(KeyValuePair<string, int> sellData in shop.sellList) {
            foreach(ItemBox itemBox in itemBoxList) {
                if (itemBox.itemCode == sellData.Key) sellItemList.Add(new ShopItem(sellData.Key, itemBox.itemNumber, sellData.Value));
            }
        }
        // 각 패널에 적용.
        buyTrade.shopItemList = buyItemList;
        sellTrade.shopItemList = sellItemList;
        // 선택 리스트는 초기화.
        buyTrade.tradeDataList.Clear();
        sellTrade.tradeDataList.Clear();
        // 최초 싱크 실시.
        buyTrade.Sync();
        sellTrade.Sync();
    }

    public void OpenUI(Message message) {
        string shopCode = (string) message.args[0];
        isActive = true;
        panel.SetActive(isActive);
        setShopInfo(shopCode);
    }

    public void CloseUI() {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void ModeChange(int mode) {
        if (mode == 0) {
            this.mode = mode;
            buyTrade.viewPanel.SetActive(true);
            sellTrade.viewPanel.SetActive(false);
            buyTrade.selectedViewPanel.SetActive(true);
            sellTrade.selectedViewPanel.SetActive(false);
        }
        if (mode == 1) {
            this.mode = mode;
            buyTrade.viewPanel.SetActive(false);
            sellTrade.viewPanel.SetActive(true);
            buyTrade.selectedViewPanel.SetActive(false);
            sellTrade.selectedViewPanel.SetActive(true);
        }
    }

    public void OnDoubleClick(Message message) {
        MouseDetector detector = new MouseDetector();
        Vector2 nowPos = detector.GetMousePos();
        if (posBuffer == nowPos) {
            if (mode == 0) buyTrade.SelectItem();
            else if (mode == 1) sellTrade.SelectItem();
        }
        else posBuffer = nowPos;        
    }
}

// 아이템과 그것의 개수 및 가격을 저장할 수 있는 구조체.
public class ShopItem {
    public string itemCode;
    public int count;
    public int price;
    public ShopItem(string itemCode, int count, int price) {
        this.itemCode = itemCode;
        this.count = count;
        this.price = price;
    }

    public ShopItem(string itemCode, int price) {
        this.itemCode = itemCode;
        this.count = 99999;
        this.price = price;
    }
}

public class Trade {
    public GameObject viewPanel;
    public List<GameObject> itemViewList;
    public GameObject selectedViewPanel;
    public List<GameObject> selectedViewList;
    public List<ShopItem> shopItemList;
    public List<TradeData> tradeDataList;

    public Trade() {
        itemViewList = new List<GameObject>();
        selectedViewList = new List<GameObject>();
        tradeDataList = new List<TradeData>();
    }
    public Trade(GameObject viewPanel, GameObject selectedViewPanel) : this() {
        this.viewPanel = viewPanel;
        this.selectedViewPanel = selectedViewPanel;
        for (int i = 0; i < viewPanel.transform.GetChild(0).GetChild(0).GetChild(0).childCount; i++) {
            itemViewList.Add(viewPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject);
        }
        for (int i = 0; i < selectedViewPanel.transform.GetChild(0).GetChild(0).GetChild(0).childCount; i++) {
            selectedViewList.Add(selectedViewPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject);
        }
    }

    public void ModifyItem(string itemCode, int itemCount = 1) {
        bool checker = false;
        for (int i = 0; i < shopItemList.Count; i++) {
            if (shopItemList[i].itemCode == itemCode) {
                shopItemList[i].count += -itemCount;
                break;
            }
        }

        checker = false;
        for (int i = 0; i < tradeDataList.Count; i++) {
            if (tradeDataList[i].itemCode == itemCode) {
                checker = true;
                tradeDataList[i].itemCount += itemCount;
                if (tradeDataList[i].itemCount <= 0) tradeDataList.Remove(tradeDataList[i]);
                    // 결과가 0 이하면 목록에서 제거.
                break;
            }
        }
        if (checker == false && itemCount > 0) tradeDataList.Add(new TradeData(itemCode, itemCount));
            // 일치하는 경우를 못 찾은 경우.
        Sync();
    }

    public int SelectedItemCount() {
        return tradeDataList.Count;
    }

    // 출력 정보 싱크를 맞춤.
    public void Sync() {
        // 상단 아이템 목록 패널 싱크.
        for (int i = 0; i < itemViewList.Count; i++) {            
            if (i < shopItemList.Count) {
                itemViewList[i].SetActive(true);
                Message msg = new Message($"InventoryManager/GetItem : {shopItemList[i].itemCode}").FunctionCall();
                Item item = (Item) msg.returnValue[0];
                itemViewList[i].transform.GetChild(0).GetComponent<Image>().sprite = item.GetItemImage();
                    // image
                itemViewList[i].transform.GetChild(1).GetComponent<Text>().text = item.GetItemName();
                    // name
                if (shopItemList[i].count > 9999) itemViewList[i].transform.GetChild(2).GetComponent<Text>().text = "INF"; // 9999 이상이면 그냥 무한으로 취급.
                else itemViewList[i].transform.GetChild(2).GetComponent<Text>().text = $"{shopItemList[i].count}";
                    // count
                itemViewList[i].transform.GetChild(3).GetComponent<Text>().text = $"{shopItemList[i].price}";
                    // price
            }
            else {
                itemViewList[i].SetActive(false);
            }
        }

        // 하단 선택 아이템 목록 패널 싱크.
        for (int i = 0; i < selectedViewList.Count; i++) {
            if (i < tradeDataList.Count) {
                selectedViewList[i].SetActive(true);
                Message msg = new Message($"InventoryManager/GetItem : {tradeDataList[i].itemCode}").FunctionCall();
                Item item = (Item) msg.returnValue[0];
                selectedViewList[i].transform.GetChild(0).GetComponent<Image>().sprite = item.GetItemImage();
                    // image
                selectedViewList[i].transform.GetChild(1).GetComponent<Text>().text = item.GetItemName();
                    // name
                selectedViewList[i].transform.GetChild(2).GetComponent<Text>().text = $"{tradeDataList[i].itemCount}";
                    // count
                selectedViewList[i].transform.GetChild(3).GetComponent<Text>().text = "" + shopItemList.Find(x => x.itemCode == tradeDataList[i].itemCode).price;
                    // price
            }
            else {
                selectedViewList[i].SetActive(false);
            }
        }
    }

    // 좌표를 입력받아 그것에 해당하는 아이템이 무엇인지 확인하고 아래 목록으로 이동.
        // 만약 대상이 하단 패널이면 오히려 위로 올림.
    public void SelectItem() {        
        MouseDetector detector = new MouseDetector();
        // 현재 커서가 상단 패널에 있는지 하단 패널에 있는지 확인.
        int mode = -1;
        detector.TargetChange(viewPanel.transform);
        if (detector.Trigger()) mode = 0;
        detector.TargetChange(selectedViewPanel.transform);
        if (detector.Trigger()) mode = 1;

        if (mode == -1) return;
            // mode 가 -1인 경우 엉뚱한 곳이 클릭되었다는 뜻이므로 탈출.
        if (mode == 0) {
            // 상단 패널 안인 경우.
            Debug.Log($"ShopUI/Trade/SelectItem.notice : is done double click.");
            for (int i = 0; i < shopItemList.Count; i++) {
                detector.TargetChange(itemViewList[i].transform);
                if (detector.Trigger()) {
                    if (shopItemList[i].count != 0) ModifyItem(shopItemList[i].itemCode);
                    return;
                }
            }
        }
        if (mode == 1) {
            // 하단 패널 안인 경우.
            for (int i = 0; i < tradeDataList.Count; i++) {
                detector.TargetChange(selectedViewList[i].transform);
                if (detector.Trigger()) {
                    ModifyItem(tradeDataList[i].itemCode, -1);
                    return;
                }
            }
        }
    }
}

public class TradeData {
    public string itemCode;
    public int itemCount;
    public TradeData(string itemCode, int itemCount) {
        this.itemCode = itemCode;
        this.itemCount = itemCount;
    }
}