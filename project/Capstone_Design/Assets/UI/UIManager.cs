using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public List<GameObject> UIList = new List<GameObject>();    
    // public Stack<string> activeUIStack = new Stack<string>();
    public GameObject canvas;
    int currentLayer;

    void Start() {
        // 캔버스와 그 자식 요소 로드
        canvas = GameObject.Find("UICanvas");
        foreach(Transform child in canvas.transform) {
            UIList.Add(child.gameObject);
        }

        MappingInfo mapping = new MappingInfo("UIManager");
        mapping.AddMapping("OpenUI : PlayerInfoUI"  , "p");
        mapping.AddMapping("OpenUI : InventoryUICanvas", "i");
        mapping.AddMapping("OpenUI : MenuUI", "m");
        // mapping.AddMapping("OpenUI : EquipmentUI"   , "r");  // 시간 부족.
        // mapping.AddMapping("OpenUI : QuestUI"       , "q");  // 시간 부족.
        // mapping.AddMapping("CloseUI : "             , "!ctrlL, esc");
        // mapping.AddMapping("CloseAllUI : "          , "_ctrlL, esc");
        // mapping.AddMapping("OpenTalkView : npc", "t");  // 디버그용.
        mapping.Enroll();
    }

    public void OpenUI(Message message) {
        string targetUI = (string) message.args[0];
        new Message($"{targetUI}/OpenUI : ").FunctionCall();
        new Message($"ControlManager/LayerChanger : {targetUI}").FunctionCall();
        // activeUIStack.Push(targetUI);
    }

    // public void CloseUI() {
    //     try { 
    //         string temp = activeUIStack.Pop();
    //         new Message($"{temp}/CloseUI : ").FunctionCall(); 
    //     }
    //     catch { Debug.Log("UIManager/CloseUI : There is no active UI."); }
    // }

    // public void CloseAllUI() {
    //     string temp;
    //     while(true) {
    //         try {
    //             temp = activeUIStack.Pop();
    //             new Message($"{temp}/CloseUI : ").FunctionCall();
    //         } catch {
    //             break;
    //         }
    //     }
    // }

    // 인벤토리를 활성화 시키고 키 매핑 레이어 전환.
    public void OpenInventory() {
        foreach(GameObject obj in UIList) {
            if (obj.name == "Inventory") {
                obj.SetActive(true);
                new Message("ControlManager/LayerChanger : inventory").FunctionCall();
            }
        }
    }

    public void OpenTalkView(Message msg) {
        string name = (string)msg.args[0];  // 대상 NPC 이름
        new Message("TalkUI/StartTalkByKey : " + name).FunctionCall();
        new Message("ControlManager/LayerChanger : talkView").FunctionCall();
    }
}