using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject processingPanel;
    public List<Text> processingQuestListTextList;
    public List<string> processingQuestNameList;
    bool isActive = false;

    private void Start()
    {
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(isActive);
        processingPanel = transform.GetChild(1).gameObject;
        for (int i = 0; i < processingPanel.transform.GetChild(1).childCount; i++) {
            processingQuestListTextList.Add(processingPanel.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<Text>());
        }
        processingQuestNameList = new List<string>();
        MappingInfo mapping = new MappingInfo("QuestUI");
        mapping.AddMapping("CloseUI : ", "esc");
        mapping.Enroll("QuestUI");
    }

    private void Updata()
    {
    }

    public void OpenUI(Message message) {
        isActive = true;
        panel.SetActive(isActive);
    }

    public void CloseUI() {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void ProcessingSync() {
        processingQuestNameList.Clear();
        Message getProcessingQuestList = new Message($"QuestManager/GetProcessingQuestList : ").FunctionCall();
        for (int i = 0; i < getProcessingQuestList.returnValue.Count; i++) {
            Quest quest = (Quest) getProcessingQuestList.returnValue[i];
            processingQuestNameList.Add(quest.name);
        }
        for (int i = 0; i < processingQuestListTextList.Count; i++) {
            if (i < processingQuestNameList.Count) processingQuestListTextList[i].text = processingQuestNameList[i];
            else processingQuestListTextList[i].text = "";
        }
    }
}
