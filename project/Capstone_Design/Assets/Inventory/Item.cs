using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item {
    string itemCode;        // 아이템 코드. 개발 및 이미지 매칭에 사용.
    string itemName;        // 아이템 이름.
    string itemTooltip;     // 아이템 툴팁. 설명 문구
    string itemEffect;      // 아이템 사용 시 효과. 커멘드 형식
    Sprite itemImg;         // 아이템 이미지

    public Item(string itemCode, string itemName, string itemTooltip, string itemEffect, Sprite itemImg) {
        this.itemCode = itemCode;
        this.itemName = itemName;
        this.itemTooltip = itemTooltip;
        this.itemEffect = itemEffect;
        this.itemImg = itemImg;
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
    public string getItemEffect() {
        return this.itemEffect;
    }
    public Sprite getItemImage() {
        return this.itemImg;
    }
}