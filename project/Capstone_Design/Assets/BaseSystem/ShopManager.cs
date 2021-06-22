using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    Dictionary<string, ShopInfo> shopInfoesDic;
    string[] shopwList = {
        "None",     // 상점 실행중이지 않을 때 처리하는 용도.
        "Sample",
        "ItemShop",
        "EquipmentShop"
    };
    ShopInfo shop;
    // Start is called before the first frame update
    void Start()
    {
        shopInfoesDic = new Dictionary<string, ShopInfo>();
        LoadShopInfo();
        shop = shopInfoesDic["None"];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadShopInfo() {
        for (int i = 0; i < shopwList.Length; i++) {
            shopInfoesDic.Add(shopwList[i], (ExternalFileSystem.SingleTon().GetShopInfo(shopwList[i])));
        }
    }

    public void ChangeShop(Message message) {
        string shopCode = (string) message.args[0];
        Debug.Log("ShopManager/ChangeShop : " + shop.shopCode + " -> " + shopCode);
        if (shopInfoesDic.ContainsKey(shopCode)) shop = shopInfoesDic[shopCode];
        else {
            Debug.Log("ShopManager/ChangeShop.Error : Invaild shopcode - " + shopCode);
            shop = null;
        } 
    }

    public void GetShop(Message message) {
        ChangeShop(message);
        message.returnValue.Add(shop);
    }

    // Interface //
    // Buy : input(item.code, number), output(isDone)
    public void Buy(Message message) {
        // Setup.
        string itemCode = (string) message.args[0];
        int itemNumber = (int) message.args[1];
        // 상점 코드가 제대로 입력되었는지 확인.
        if (shop == null) {
            Debug.Log("ShopManager/Buy : Incorrect Shop Data");
            message.returnValue.Add(false);
            return;
        }
        // 해당 아이템을 상점에서 취급하는지 확인.
        if (shop.buyList.ContainsKey(itemCode) == false) {
            Debug.Log("ShopManager/Buy : Can't buy " + itemCode + " in " + shop.shopCode);
            message.returnValue.Add(false);
            return;
        }
        // money Check.
        Message getMoney = new Message("PlayInfoManager/GetData : Money").FunctionCall();
        int itemPrice = shop.buyList[itemCode];
        int money = (int) getMoney.returnValue[0];
        int needMoney = itemPrice * itemNumber;
        // If don't have enough money, stop process.
        if (money < needMoney) {
            Debug.Log("ShopManager/Buy : There is no " + itemCode);
            message.returnValue.Add(false);
            return;
        }
        // Action.        
        new Message("PlayInfoManager/ChangeData : Money, " + -needMoney).FunctionCall();
        new Message("InventoryManager/ModifyItem : " + itemCode + ", " + itemNumber).FunctionCall();
        message.returnValue.Add(true);
    }
    // Sell : input(item.code, number), output(isDone)
    public void Sell(Message message) {
        // Setup.
        string itemCode = (string) message.args[0];
        int itemNumber = (int) message.args[1];
        // 상점 코드가 제대로 입력되었는지 확인.
        if (shop == null) {
            Debug.Log("ShopManager/Sell : Incorrect Shop Data");
            message.returnValue.Add(false);
            return;
        }
        // 해당 아이템을 상점에서 취급하는지 확인.
        if (shop.sellList.ContainsKey(itemCode) == false) {
            Debug.Log("ShopManager/Sell : Can't sell " + itemCode + " in " + shop.shopCode);
            message.returnValue.Add(false);
            return;
        }
        // Inventory Check.
        Message GetItemNumber = new Message("InventoryManager/GetItemNumber : " + itemCode).FunctionCall();
        int itemPrice = shop.sellList[itemCode];
        int itemNumberInInventory = (int) GetItemNumber.returnValue[0];
        // If don't have enough money, stop process.
        if (itemNumberInInventory == 0) {
            Debug.Log("ShopManager/Sell : Not enough money for " + itemCode);
            message.returnValue.Add(false);
            return;
        }
        // Action.
        int salePrice = itemPrice * itemNumber;
        new Message("PlayInfoManager/ChangeData : Money, " + salePrice).FunctionCall();
        new Message("InventoryManager/ModifyItem : " + itemCode + ", " + -itemNumber).FunctionCall();
        message.returnValue.Add(true);
    }
}

public class ShopInfo {
    public string shopCode;
    public Dictionary<string, int> buyList;
    public Dictionary<string, int> sellList;

    public ShopInfo(string code) {
        shopCode = code;
        buyList = new Dictionary<string, int>();
        sellList = new Dictionary<string, int>();
    }
}