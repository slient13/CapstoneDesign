using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    void Start()
    {
        MappingInfo mapping = new MappingInfo("UIManager");
        mapping.AddMapping("OpenUI : PlayerInfoUI", "p");
        mapping.AddMapping("OpenUI : InventoryUICanvas", "i");
        mapping.AddMapping("OpenUI : MenuUI", "m");
        mapping.AddMapping("OpenUI : EquipmentUI", "r");
        mapping.AddMapping("OpenUI : QuestUI", "q");
        mapping.Enroll();
    }

    public void OpenUI(Message message)
    {
        string targetUI = (string)message.args[0];
        new Message($"{targetUI}/OpenUI : ").FunctionCall();
        new Message($"ControlManager/LayerChanger : {targetUI}").FunctionCall();
    }
    public void OpenTalkView(Message msg)
    {
        string name = (string)msg.args[0];  // 대상 NPC 이름
        new Message("TalkUI/StartTalkByKey : " + name).FunctionCall();
        new Message("ControlManager/LayerChanger : talkView").FunctionCall();
    }
}