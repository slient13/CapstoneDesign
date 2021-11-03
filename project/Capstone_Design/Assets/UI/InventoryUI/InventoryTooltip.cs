using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTooltip : MonoBehaviour
{
    public Text itemNameText;
    public Text itemTooltipText;
    public Text itemItemEffectText;
    public Button buttonUse;
    public Button buttonAbandon;
    public Button buttonCancle;
    public string itemCode;
    // Start is called before the first frame update
    void Awake()
    {
        itemNameText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        itemTooltipText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        itemItemEffectText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        buttonUse = transform.GetChild(3).GetChild(0).GetComponent<Button>();
        buttonUse.onClick.AddListener(use);
        buttonUse.onClick.AddListener(cancel);
        buttonAbandon = transform.GetChild(3).GetChild(1).GetComponent<Button>();
        buttonAbandon.onClick.AddListener(abandon);
        buttonAbandon.onClick.AddListener(cancel);
        buttonCancle = transform.GetChild(3).GetChild(2).GetComponent<Button>();
        buttonCancle.onClick.AddListener(cancel);
    }

    public void Sync() {
        string itemName;
        string itemTooltip;
        string itemEffect;
        Message msg = new Message($"InventoryManager/GetItem : {itemCode}").FunctionCall();
        if (msg.returnValue.Count != 0) {
            Item item = (Item)msg.returnValue[0];
            itemName = item.GetItemName();
            itemTooltip = item.GetItemToolTip();
            itemEffect = "";
            foreach(string effectString in item.GetItemEffectDesc()) itemEffect += effectString;
        }
        else {
            itemName = "";
            itemTooltip = "";
            itemEffect = "";
        }
        itemNameText.text = itemName;
        itemTooltipText.text = itemTooltip;
        itemItemEffectText.text = itemEffect;
    }

    void use() { new Message($"InventoryManager/UseItem : {itemCode}").FunctionCall(); }
    void abandon() {}
    void cancel() { transform.gameObject.SetActive(false); }
}
