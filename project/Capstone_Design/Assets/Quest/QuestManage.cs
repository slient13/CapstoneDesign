using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManage : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public int id;
    

    Dictionary<int, QuestData> questlist;
    void Start()
    {
        questlist = new Dictionary<int, QuestData>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        questlist.Add(10, new QuestData("낚시를 해서 광어 한 마리 잡아오기.", new int[] { 1000, 2000 }));
    }
    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }
    public void CheckQuest()
    {
        if(id == questlist[questId].npcId[questActionIndex])
        questActionIndex++;
    }

    
}
