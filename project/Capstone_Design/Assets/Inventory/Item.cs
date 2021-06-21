using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item {
    string code;        // 아이템 코드. 개발 및 이미지 매칭에 사용.
    string type;        // 아이템 타입. 표기 분류 및 취급 분류에 사용.
    string name;        // 아이템 이름.
    string tooltip;     // 아이템 툴팁. 설명 문구
    List<string> effect;      // 아이템 사용 시 효과. 커멘드 형식
    Sprite img;         // 아이템 이미지

    public Item(string code, string type, string name, string tooltip, List<string> effect, Sprite img) {
        this.code = code;
        this.type = type;
        this.name = name;
        this.tooltip = tooltip;
        this.effect = effect;
        this.img = img;
    }

    public string GetItemCode() {
        return this.code;
    }

    public string GetItemType() {
        return this.type;
    }
    public string GetItemName() {
        return this.name;
    }
    public string GetItemToolTip() {
        return this.tooltip;
    }
    public List<string> GetItemEffect() {
        return this.effect;
    }
    public Sprite GetItemImage() {
        return this.img;
    }
}