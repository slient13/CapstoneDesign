using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour {
    GameObject itemBox;
    List<GameObject> itemBoxList = new List<GameObject>();
    List<Image> itemImageList = new List<Image>();
    List<Text> itemNumberList = new List<Text>();
    List<Text> alterTextList = new List<Text>();
    GameObject itemPanel;
    

    int MAX_ITEM_BOX = 16;

    private void Start() {
        itemBox = Resources.Load("Inventory/Prefab/ItemBox") as GameObject;
        itemPanel = GameObject.Find("ItemPanel");
        itemBoxPlacer();

        MappingInfo mapping = new MappingInfo("Inventory");
        mapping.addMapping("closeInventory : ", "esc");
        mapping.enroll("inventory");

        gameObject.SetActive(false);
    }

    void itemBoxPlacer() {
        GameObject tempObject;
        for (int axleY = 0; axleY < MAX_ITEM_BOX / 4; axleY++) {
            for (int axleX = 0 ; axleX < MAX_ITEM_BOX / 4; axleX++) {
                // 새 itemBox 판넬을 생성해서 배치함.
                tempObject = GameObject.Instantiate(itemBox);
                tempObject.transform.SetParent(itemPanel.transform, true);
                tempObject.transform.localPosition = new Vector3(-195 + axleX*100, 195 - axleY*100, 0);
                // itemBox 판넬 자체의 Image 컴포넌트에 접근, 리스트에 저장함.
                itemImageList.Add(tempObject.transform.GetComponent<Image>());
                // itemBox 내 아이템 개수를 표시하는 자식 UI에 접근, 리스트에 저장함
                itemNumberList.Add(tempObject.transform.GetChild(0).GetComponent<Text>());
                itemNumberList[(4*axleY + axleX)].text = "";
                // itemBox 내 alterText 를 표시하는 자식 UI에 접근, 리스트에 저장함
                alterTextList.Add(tempObject.transform.GetChild(1).GetComponent<Text>());
                alterTextList[(4*axleY + axleX)].text = "";
                // 해당 판넬 자체를 itemBoxList 에 저장함.
                itemBoxList.Add(tempObject);
            }
        }
    }

    private void Update() {
        Message msg = new Message("InventoryManager/getItemBoxList : ");
        msg.functionCall();
        List<ItemBox> tempItemBoxList = (List<ItemBox>)msg.returnValue[0];  // 아이템박스 리스트 전체
        for (int i = 0; i < MAX_ITEM_BOX; i++) {
            if (tempItemBoxList[i].itemNumber != 0) {
                // 개수 표시
                itemNumberList[i].text = "" + tempItemBoxList[i].itemNumber;
                // 이미지 표시. 없다면 대체 텍스트 표시
                if (tempItemBoxList[i].itemImg != null) {
                    itemImageList[i].sprite = tempItemBoxList[i].itemImg;
                    alterTextList[i].text = "";
                }
                else {
                    alterTextList[i].text = tempItemBoxList[i].itemCode;
                }
            }
            else {
                itemNumberList[i].text = "";
                alterTextList[i].text = "";
                itemImageList[i].sprite = null;
            }
        }
    }

    public void closeInventory() {
        gameObject.SetActive(false);
        new Message("ControlManager/layerChanger : general").functionCall();
    }
}