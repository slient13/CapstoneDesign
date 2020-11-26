using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemBox {
    public string itemCode;
    public int itemNumber;
    
    public ItemBox() {
        itemCode = ""; 
        itemNumber = 0;
    }
    public ItemBox(string itemCode, int itemNumber) {
        this.itemCode = itemCode;
        this.itemNumber = itemNumber;
    }

    public void changeNumber(int degree) {
        itemNumber += degree;
        if (itemNumber < 0) itemNumber = 0;
    }

    public void changeItem(string itemCode, int itemNumber) {
        this.itemCode = itemCode;
        this.itemNumber = itemNumber;
    }

    public void changeItem(ItemBox other) {
        this.itemCode = other.itemCode;
        this.itemNumber = other.itemNumber;
    }

    public void reset() {
        this.itemCode = "";
        this.itemNumber = 0;
    }
}