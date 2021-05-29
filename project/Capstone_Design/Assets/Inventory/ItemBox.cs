using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemBox {
    public string itemType;
    public string itemCode;
    public int itemNumber;
    public Sprite itemImg;
    
    public ItemBox() {
        this.itemType = "";
        this.itemCode = ""; 
        this.itemNumber = 0;
        this.itemImg = null;
    }
    public ItemBox(string itemType, string itemCode, int itemNumber, Sprite itemImg) {
        this.itemType = itemType;
        this.itemCode = itemCode;
        this.itemNumber = itemNumber;
        this.itemImg = itemImg;
    }

    public void changeNumber(int degree) {
        itemNumber += degree;
        if (itemNumber < 0) itemNumber = 0;
    }

    public void changeItem(string itemType, string itemCode, int itemNumber, Sprite itemImg) {
        this.itemType = itemType;
        this.itemCode = itemCode;
        this.itemNumber = itemNumber;
        if (this.itemNumber < 0) this.itemNumber = 0;
        this.itemImg = itemImg;
    }

    public void changeItem(ItemBox other) {
        this.itemType = other.itemType;
        this.itemCode = other.itemCode;
        this.itemNumber = other.itemNumber;
        if (this.itemNumber < 0) this.itemNumber = 0;
        this.itemImg = other.itemImg;
    }

    public void reset() {
        this.itemType = "";
        this.itemCode = "";
        this.itemNumber = 0;
        this.itemImg = null;
    }
}