using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    string itemCode;        // 아이템 코드. 개발 및 이미지 매칭에 사용.
    string itemName;        // 아이템 이름.
    string itemTooltip;     // 아이템 툴팁. 설명 문구
    string itemEffect;      // 아이템 사용 시 효과. 커멘드 형식

    public Item(string itemCode, string itemName, string itemTooltip, string itemEffect) {
        this.itemCode = itemCode;
        this.itemName = itemName;
        this.itemTooltip = itemTooltip;
        this.itemEffect = itemEffect;
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
    public string getItemitemEffect() {
        return this.itemEffect;
    }
}