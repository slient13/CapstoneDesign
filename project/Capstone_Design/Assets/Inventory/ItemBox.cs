using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemBox {
    public string itemCode;
    public int itemNumber;
    public Sprite itemImg;
    
    public ItemBox() {
        this.itemCode = ""; 
        this.itemNumber = 0;
        this.itemImg = null;
    }
    public ItemBox(string itemCode, int itemNumber, Sprite itemImg) {
        this.itemCode = itemCode;
        this.itemNumber = itemNumber;
        this.itemImg = itemImg;
    }

    public void changeNumber(int degree) {
        itemNumber += degree;
        if (itemNumber < 0) itemNumber = 0;
    }

    public void changeItem(string itemCode, int itemNumber, Sprite itemImg) {
        this.itemCode = itemCode;
        this.itemNumber = itemNumber;
        this.itemImg = itemImg;
    }

    public void changeItem(ItemBox other) {
        this.itemCode = other.itemCode;
        this.itemNumber = other.itemNumber;
        this.itemImg = other.itemImg;
    }

    public void reset() {
        this.itemCode = "";
        this.itemNumber = 0;
        this.itemImg = null;
    }
}