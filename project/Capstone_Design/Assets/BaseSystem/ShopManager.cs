using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    List<ShopInfo> shopInfos;
    // Start is called before the first frame update
    void Start()
    {
        shopInfos = new List<ShopInfo>();
        getShopInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getShopInfo() {
        shopInfos.Add(ExternalFileSystem.SingleTon().GetShopInfo());
    }

    // Interface //
    // Buy : input(item.code, number), output(isDone)
    public void Buy(Message message) {
        // Setup.
        string shopCode = (string) message.args[0];        
        string itemCode = (string) message.args[1];
        int itemNumber = (int) message.args[2];
        ShopInfo shop = null;
        foreach(ShopInfo shopinfo in shopInfos) {
            if (shopinfo.shopCode == shopCode) {
                shop = shopinfo;
                break;
            }
        }
        if (shop == null) {
            Debug.Log("ShopManager/Buy : shopCode is invalid. Input shopCode is" + shopCode + ".");
            message.returnValue.Add(false);
            return;
        }
        // Money Check.
        Message getMoney = new Message("PlayInfoManager/GetMoney : ").FunctionCall();
        int itemPrice = shop.buyList[itemCode];
        int money = (int) getMoney.returnValue[0];
        int needMoney = itemPrice * itemNumber;
        // If don't have enough money, stop process.
        if (money < needMoney) {
            message.returnValue.Add(false);
            return;
        }
        // Action.        
        new Message("PlayInfoManager/ChangeMoney : " + -needMoney).FunctionCall();
        new Message("InventoryManager/ModifyItem : " + itemCode + ", " + itemNumber).FunctionCall();
        message.returnValue.Add(true);
    }
    // Sell : input(item.code, number), output(isDone)
    public void Sell(Message message) {
        // Setup.
        string shopCode = (string) message.args[0];        
        string itemCode = (string) message.args[1];
        int itemNumber = (int) message.args[2];
        ShopInfo shop = null;
        foreach(ShopInfo shopinfo in shopInfos) {
            if (shopinfo.shopCode == shopCode) {
                shop = shopinfo;
                break;
            }
        }
        if (shop == null) {
            Debug.Log("ShopManager/Buy : shopCode is invalid. Input shopCode is" + shopCode + ".");
            message.returnValue.Add(false);
            return;
        }
        // Inventory Check.
        Message GetItemNumber = new Message("InventoryManager/GetItemNumber : " + itemCode).FunctionCall();
        int itemPrice = shop.sellList[itemCode];
        int itemNumberInInventory = (int) GetItemNumber.returnValue[0];
        // If don't have enough money, stop process.
        if (itemNumberInInventory == 0) {
            message.returnValue.Add(false);
            return;
        }
        // Action.
        int salePrice = itemPrice * itemNumber;
        new Message("PlayInfoManager/ChangeMoney : " + salePrice).FunctionCall();
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