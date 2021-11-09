using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    Dictionary<string, Quest> questList;
    // Start is called before the first frame update
    public List<string> questNameList;
    void Awake()
    {
        questNameList = new List<string>();
        questNameList.Add("GetFish");
        loadQuest();
        loadQuestProcess();
    }
    bool loadQuest()
    {
        questList = ExternalFileSystem.SingleTon().LoadQuest(questNameList);
        return true;
    }

    void SaveQuestProcess()
    {
        List<string> processing_quest_code_list = new List<string>();
        foreach(KeyValuePair<string, Quest> quest_pair in questList)
        {
            Quest quest = quest_pair.Value;
            if (quest.isProcess == true) processing_quest_code_list.Add(quest.code);
        }
        ExternalFileSystem.SingleTon().SaveQuestProcess(processing_quest_code_list);
    }
    void loadQuestProcess()
    {
        List<string> processing_quest_name_list = ExternalFileSystem.SingleTon().LoadQuestProcess();
        foreach (string processing_quest_name in processing_quest_name_list)
            StartQuest(new Message($":{processing_quest_name}"));
    }
    public void GetQuestProcessInfo(Message message)
    {
        string target = (string)message.args[0];

        // 존재 여부 확인.
        if (!questList.ContainsKey(target))
        {
            Debug.Log("QuestManager.GetQuestProcessInfo.Error : There is no key - " + target);
            message.returnValue.Add(0);
            return;
        }
        Quest targetQuest = questList[target];

        // 진행 여부 확인.
        if (targetQuest.isProcess == false)
        {
            Debug.Log($"QuestManager.GetQuestProcessInfo.Error : [{target}] is not in process.");
            message.returnValue.Add(0);
            return;
        }
        message.returnValue.Add(1);

        // 임시 변수 할당.
        List<string> codeList = new List<string>();
        List<int> processList = new List<int>();
        List<int> goalList = new List<int>();

        // goal 확인.
        foreach (QuestInfo goal in targetQuest.goalList)
        {
            codeList.Add(goal.code);
            processList.Add(getProcessData(goal.code, goal.type));
            goalList.Add(Convert.ToInt32(goal.value));
        }
        // price 확인.
        foreach (QuestInfo price in targetQuest.priceList)
        {
            codeList.Add(price.code);
            processList.Add(getProcessData(price.code, price.type));
            goalList.Add(Convert.ToInt32(price.value));
        }
    }

    // 대상 퀘스트 시작
    public bool StartQuest(Message message)
    {
        string targetName = (string)message.args[0];
        if (!questList.ContainsKey(targetName))
        {
            Debug.Log("QuestManager.StartQuest.Error : There is no key - " + targetName);
            message.returnValue.Add(0);
            return false;
        }
        // 임시 변수 할당.
        Quest targetQuest = questList[targetName];
        if (!targetQuest.isProcess)
        {
            targetQuest.isProcess = true;
            message.returnValue.Add(1);
            Debug.Log($"QuestManager.StartQuest.Notice : ['{targetQuest.code}'] start.");
        }
        else
        {
            Debug.Log($"QuestManager.StartQuest.Error : The Quest '{targetQuest.code}' is already in process.");
            message.returnValue.Add(0);
        }
        questUISync();
        return true;
    }

    // 퀘스트가 진행중인지 확인. 외부용.
    public void IsQuestInProcess(Message message)
    {
        string targetName = (string)message.args[0];
        if (!questList.ContainsKey(targetName))
        {
            Debug.Log("QuestManager.IsQuestInProcess.Error : There is no key - " + targetName);
            message.returnValue.Add(null);
            return;
        }
        // 임시 변수 할당.
        Quest targetQuest = questList[targetName];
        if (targetQuest.isProcess) message.returnValue.Add(1);
        else message.returnValue.Add(0);
    }

    // 진행중인 퀘스트 목록 반환
    public void GetProcessingQuestList(Message message)
    {
        List<Quest> output = new List<Quest>();
        foreach (KeyValuePair<string, Quest> questPair in questList)
        {
            if (questPair.Value.isProcess == true) output.Add(questPair.Value);
        }
        message.returnValue.Add(output);
    }

    // 퀘스트 완료 체크.
    public bool CheckQuestFinish(Message message)
    {
        string target = (string)message.args[0];
        bool doFinish = false;      // 퀘스트 진행 체크와 동시에 퀘스트를 종료할지 여부.
        if (message.args.Count == 2)
        {
            if ((int)message.args[1] == 1) doFinish = true;
            Debug.Log($"QuestManager/CheckQuestFinish.doFinish = {doFinish}");
        }
        // 키 비교.
        if (!questList.ContainsKey(target))
        {
            Debug.Log("QuestManager/checkPrice.Error : There is no key. input = " + target);
            return false;
        }
        // 임시 변수 할당.
        Quest targetQuest = questList[target];
        // 진행 여부 확인
        if (!targetQuest.isProcess)
        {
            Debug.Log($"QuestManager/checkPrice.Error : [{target}] is not in process.");
            message.returnValue.Add(2);
            return false;
        }

        if (!checkGoal(targetQuest)) { message.returnValue.Add(0); return false; }
        if (!checkPrice(targetQuest)) { message.returnValue.Add(0); return false; }

        // 퀘스트 완료 실행 여부가 참일 경우 퀘스트 종료. 그렇지 않으면 단순히 확인만
        if (doFinish == true) finishQuest(targetQuest);
        message.returnValue.Add(1);
        return true;
    }
    
    public void CancelQuest(Message message)
    {
        string targetCode = (string) message.args[0];
        Quest targetQuest = this.questList[targetCode];
        targetQuest.isProcess = false;
    }
    // 진행 관련 정보 반환.
    int getProcessData(string code, string type)
    {
        Message getData;
        Message getItemCount;
        if (type == "Player.Stat")
        {
            getData = new Message($"GetPlayInfoValue : {type}.{code}").FunctionCall();
            return (int)getData.returnValue[0];
        }
        if (type == "Inventory")
        {
            getItemCount = new Message($"InventoryManager/GetItemNumber : {code}").FunctionCall();
            return (int)getItemCount.returnValue[0];
        }
        Debug.Log($"QuestManager/getProcessData.error : There is no match case. code = {code}, type = {type}");
        return -1;
    }
    // 비용 및 보상 적용 용도.
    void setData(string code, string type, string value)
    {
        Debug.Log($"QuestManager/setData.test : type = {type}, code = {code}, value = {value}");
        if (type == "Player.Stat")
        {
            new Message($"ChangeData : {type}.{code}, {value}").FunctionCall();
            return;
        }
        else if (type == "Inventory")
        {
            new Message($"InventoryManager/ModifyItem : {code}, {value}").FunctionCall();
            return;
        }
        Debug.Log($"QuestManager/setData.error : There is no type . input = {type}.");
    }
    // 목표 비고
    bool checkGoal(Quest target)
    {
        foreach (QuestInfo goal in target.goalList)
        {
            int dataValue = getProcessData(goal.code, goal.type);
            // 비교.
            if (dataValue == -1) return false;
            else if (dataValue < Convert.ToInt32(goal.value)) return false;
        }
        // 어떤 목표도 누락되지 않음.
        return true;
    }
    // 비용 비교.
    bool checkPrice(Quest target)
    {
        foreach (QuestInfo price in target.priceList)
        {
            // Debug.Log($"QuestManager.checkPrice.debug : price.code = {price.code}, price.type = {price.type}, price.value = {price.value}.");
            int dataValue = getProcessData(price.code, price.type);
            // 비교.
            if (dataValue == -1) return false;
            else if (dataValue < Convert.ToInt32(price.value)) return false;
        }
        return true;
    }
    // 퀘스트 종료, 보상 지금.
    bool finishQuest(Quest target)
    {
        Debug.Log($"QuestManager/finishQuest.target.priceList.count = {target.priceList.Count}");
        foreach (QuestInfo price in target.priceList) setData(price.code, price.type, "-" + price.value);
        foreach (QuestInfo reward in target.rewardList) setData(reward.code, reward.type, reward.value);
        target.isProcess = false;
        questUISync();
        return true;
    }

    void questUISync()
    {
        new Message("QuestUI/Sync : ").FunctionCall();
    }
}

public class Quest
{
    public string code { get; }
    public string name;
    public string desc;
    public string rewardDesc;
    public List<QuestInfo> goalList;
    public List<QuestInfo> priceList;
    public List<QuestInfo> rewardList;
    public bool isProcess;

    public Quest(string code, string name = "", string desc = "", string rewardDesc = "" )
    {
        this.code = code;
        this.name = name;
        this.desc = desc;
        this.rewardDesc = rewardDesc;
        this.goalList = new List<QuestInfo>();
        this.priceList = new List<QuestInfo>();
        this.rewardList = new List<QuestInfo>();
        this.isProcess = false;
    }
}

public class QuestInfo
{
    public string type;
    public string code;
    public string value;
    public QuestInfo(string type, string code, string value)
    {
        this.type = type;
        this.code = code;
        this.value = value;
    }
}