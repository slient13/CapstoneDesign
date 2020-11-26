using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour {
    GameObject itemBox;
    List<GameObject> itemBoxList = new List<GameObject>();
    List<Text> itemNumberList = new List<Text>();
    GameObject itemPanel;
    

    int MAX_ITEM_BOX = 16;

    private void Start() {
        itemBox = Resources.Load("Inventory/Prefab/ItemBox") as GameObject;
        itemPanel = GameObject.Find("ItemPanel");
        itemBoxPlacer();
    }

    void itemBoxPlacer() {
        GameObject tempObject;
        for (int i = 0; i < MAX_ITEM_BOX / 4; i++) {
            for (int j = 0 ; j < MAX_ITEM_BOX / 4; j++) {
                tempObject = GameObject.Instantiate(itemBox);
                tempObject.transform.SetParent(itemPanel.transform, true);
                tempObject.transform.localPosition = new Vector3(-195 + i*100, 195 - j*100, 0);
                itemNumberList.Add(tempObject.transform.GetChild(0).GetComponent<Text>());
                itemNumberList[(4*i + j)].text = "";
                itemBoxList.Add(tempObject);
            }
        }
    }

    private void Update() {
        Message msg = new Message("InventoryManager/getItemBoxList : ");
        msg.functionCall();
        List<ItemBox> tempItemBoxList = (List<ItemBox>)msg.returnValue[0];
        for (int i = 0; i < MAX_ITEM_BOX; i++) {
            if (tempItemBoxList[i].itemNumber != 0) 
                itemNumberList[i].text = "" + tempItemBoxList[i].itemNumber;
            else 
                itemNumberList[i].text = "";
        }
    }
}