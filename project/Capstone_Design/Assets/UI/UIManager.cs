using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public List<GameObject> UIList = new List<GameObject>();    
    public GameObject canvas;
    int currentLayer;

    void Start() {
        // 캔버스와 그 자식 요소 로드
        canvas = GameObject.Find("Canvas");
        foreach(Transform child in canvas.transform) {
            UIList.Add(child.gameObject);
        }

        MappingInfo mapping = new MappingInfo("UIManager");
        mapping.addMapping("openInventory : ", "i");
        // mapping.addMapping("openTalkView : npc", "t");  // 디버그용.
        mapping.enroll();
    }

    // 인벤토리를 활성화 시키고 키 매핑 레이어 전환.
    public void openInventory() {
        foreach(GameObject obj in UIList) {
            if (obj.name == "Inventory") {
                obj.SetActive(true);
                new Message("ControlManager/layerChanger : inventory").functionCall();
            }
        }
    }

    public void openTalkView(Message msg) {
        string name = (string)msg.args[0];  // 대상 NPC 이름
        new Message("TalkUI/StartTalkByKey : " + name).functionCall();
        new Message("ControlManager/layerChanger : talkView").functionCall();
    }
}