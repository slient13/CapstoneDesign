using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public class _Processing
    {
        public GameObject panel;
        public Text name;
        public Toggle selected_toggle;
        public bool isActive = false;
        public _Processing(GameObject panel, Text name, Toggle selected_toggle)
        {
            this.panel = panel;
            this.name = name;
            this.selected_toggle = selected_toggle;
        }
    }
    public class _FinishAble
    {
        public GameObject panel;
        public Text name;
        public bool isActive = false;
        public _FinishAble(GameObject panel, Text name)
        {
            this.panel = panel;
            this.name = name;
        }
    }
    public class _ContentView
    {
        public Text title;
        public Text contents;
        public Text reward;
        public _ContentView(Text title, Text contents, Text reward)
        {
            this.title = title;
            this.contents = contents;
            this.reward = reward;
        }
        public void Clear()
        {
            title.text = "선택된 퀘스트 없음";
            contents.text = "왼쪽 목록에서 선택해주세요.";
            reward.text = "";
        }
    }
    public class _SideUI
    {
        public GameObject panel;
        public List<Text> textList;
        public _SideUI()
        {
            this.textList = new List<Text>();
        }
        public void Change(List<string> questNameList)
        {
            for (int i = 0; i < textList.Count; ++i)
            {
                if (i < questNameList.Count)
                    textList[i].text = questNameList[i];
                else
                    textList[i].text = "";
            }
        }
    }
    //////////////////////////////////////////////////////
    const int __qeust_list_count = 6;
    const int __side_ui_list_count = 3;
    //////////////////////////////////////////////////////
    public List<_Processing> processing_list = new List<_Processing>();
    public List<_FinishAble> finishAble_list = new List<_FinishAble>();
    public _ContentView processing_contentView;
    public _ContentView finishAble_contentView;
    public _SideUI sideUI = new _SideUI();
    public GameObject panel;
    //////////////////////////////////////////////////////
    List<Quest> processing_questList;
    List<Quest> finishAble_questList;
    bool isActive = false;
    int selected_count;
    int processing_count;
    private void Start()
    {
        {   // set panel
            panel = transform.GetChild(0).gameObject;
            panel.SetActive(isActive);
        }
        {   // set processing_list
            Transform processingListTransform = panel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0);
            for (int i = 0; i < __qeust_list_count; ++i)
            {
                GameObject temp_processing_panel = processingListTransform.GetChild(i).gameObject;
                Text temp_processing_text = processingListTransform.GetChild(i).GetChild(0).GetComponent<Text>();
                Toggle temp_processing_toggle = processingListTransform.GetChild(i).GetChild(1).GetComponent<Toggle>();
                processing_list.Add(new _Processing(
                    temp_processing_panel,
                    temp_processing_text,
                    temp_processing_toggle));
            }
        }
        {   // set finishAble_list
            Transform finishAbleListTransform = panel.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0);
            for (int i = 0; i < __qeust_list_count; ++i)
            {
                GameObject temp_finishAble_panel = finishAbleListTransform.GetChild(i).gameObject;
                Text temp_finishAble_text = finishAbleListTransform.GetChild(i).GetChild(0).GetComponent<Text>();
                finishAble_list.Add(new _FinishAble(
                    temp_finishAble_panel,
                    temp_finishAble_text));
            }
        }
        {   // set processing_contentView
            Transform processing_contentView_transform = panel.transform.GetChild(4);
            Text temp_processing_contentView_title_text =
                processing_contentView_transform.GetChild(0).GetChild(0).GetComponent<Text>();
            Text temp_processing_contentView_title_contents =
                processing_contentView_transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
            Text temp_processing_contentView_title_reward =
                processing_contentView_transform.GetChild(1).GetChild(0).GetComponent<Text>();
            processing_contentView = new _ContentView(
                temp_processing_contentView_title_text,
                temp_processing_contentView_title_contents,
                temp_processing_contentView_title_reward);
        }
        {   // set finishAble_contentView
            Transform finishAble_contentView_transform = panel.transform.GetChild(5);
            Text temp_finishAble_contentView_title_text =
                finishAble_contentView_transform.GetChild(0).GetChild(0).GetComponent<Text>();
            Text temp_finishAble_contentView_title_contents =
                finishAble_contentView_transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
            Text temp_finishAble_contentView_title_reward =
                finishAble_contentView_transform.GetChild(1).GetChild(0).GetComponent<Text>();
            finishAble_contentView = new _ContentView(
                temp_finishAble_contentView_title_text,
                temp_finishAble_contentView_title_contents,
                temp_finishAble_contentView_title_reward);
        }
        {   // set sideUI
            sideUI.panel = transform.GetChild(1).gameObject;
            for (int i = 0; i < sideUI.panel.transform.GetChild(1).childCount; i++)
            {
                sideUI.textList.Add(sideUI.panel.transform.GetChild(1).GetChild(i).GetChild(0).GetComponent<Text>());
            }
        }
        {   // set mapping
            MappingInfo mapping = new MappingInfo("QuestUI");
            mapping.AddMapping("CloseUI : ", "esc");
            mapping.Enroll("QuestUI");
        }
        {   // initialize
            selected_count = 0;
            processing_count = 0;
        }
        {   // default setting
            Sync();
            ChangeContentView(-1);
            CloseUI();
        }
    }

    public void OpenUI()
    {
        isActive = true;
        panel.SetActive(isActive);
        ChangeContentView(-1);
        Sync();
    }

    public void CloseUI()
    {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void Sync()
    {
        {   // get quest procssing data
            processing_questList = (List<Quest>)new Message($"QuestManager/GetProcessingQuestList : ").FunctionCall().returnValue[0];

            finishAble_questList = new List<Quest>();
            foreach (Quest quest in processing_questList)
            {
                int check = (int)new Message($"QuestManager/CheckQuestFinish : {quest.code}").FunctionCall().returnValue[0];
                if (check == 1) finishAble_questList.Add(quest);
            }
        }
        {   // sync quest UI's datas.
            {   // processing
                for (int i = 0; i < __qeust_list_count; ++i)
                {
                    if (i < processing_questList.Count)
                    {
                        processing_list[i].name.text = processing_questList[i].name;
                        processing_list[i].isActive = true;
                    }
                    else
                    {
                        processing_list[i].name.text = "";
                        processing_list[i].isActive = false;
                    }
                }
            }
            {   // finishAble
                for (int i = 0; i < __qeust_list_count; ++i)
                {
                    if (i < finishAble_questList.Count)
                    {
                        finishAble_list[i].name.text = finishAble_questList[i].name;
                        finishAble_list[i].isActive = true;
                    }
                    else
                    {
                        finishAble_list[i].name.text = "";
                        finishAble_list[i].isActive = false;
                    }
                }
            }
        }
        {   // set active of processing and finishable panel
            foreach (_Processing processing in processing_list)
            {
                processing.panel.SetActive(processing.isActive);
            }

            foreach (_FinishAble finishAble in finishAble_list)
            {
                finishAble.panel.SetActive(finishAble.isActive);
            }
        }
        SyncSideUI();
    }
    public void SyncSideUI()
    {   // sync side UI
        List<string> temp_quest_name_list = new List<string>();
        foreach (_Processing processing in processing_list)
            if (processing.selected_toggle.isOn == true)
                temp_quest_name_list.Add(processing.name.text);
        sideUI.Change(temp_quest_name_list);
    }
    public void ChangeContentView(int mode)
    {
        if (mode == -1)
        {
            processing_contentView.Clear();
            finishAble_contentView.Clear();
        }
        else if (mode < 10)
        {
            if (processing_questList.Count <= mode)
                processing_contentView.Clear();
            else
            {
                Quest targetQuest = processing_questList[mode];
                processing_contentView.title.text = targetQuest.name;
                processing_contentView.contents.text = targetQuest.desc;
                processing_contentView.reward.text = targetQuest.rewardDesc;
            }
        }
        else if (mode < 20)
        {
            mode -= 10;
            if (finishAble_questList.Count <= mode)
                finishAble_contentView.Clear();
            else
            {
                Quest targetQuest = finishAble_questList[mode];
                finishAble_contentView.title.text = targetQuest.name;
                finishAble_contentView.contents.text = targetQuest.desc;
                finishAble_contentView.reward.text = targetQuest.rewardDesc;
            }
        }
    }
}
