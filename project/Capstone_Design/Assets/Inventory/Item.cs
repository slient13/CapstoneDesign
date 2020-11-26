using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    string itemCode;
    string itemName;
    string itemTooltip;

    public Item(string itemCode, string itemName, string itemTooltip) {
        this.itemCode = itemCode;
        this.itemName = itemName;
        this.itemTooltip = itemTooltip;
    }

    public string getItemCode() {
        return this.itemCode;
    }
    public string getItemName() {
        return this.itemName;
    }
    public string getItemToolTip() {
        return this.itemTooltip;
    }
}