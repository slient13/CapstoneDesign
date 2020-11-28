using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    void Start()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
       
    }

    // Update is called once per frame
    public void GenerateData()
    {
        Debug.Log("ㅇㅇㅇ");
        talkData.Add(1000, new string[] { "나는 Quest NPC 입니다.", "원하시는 Quest가 있으십니까?" });

        talkData.Add(10 + 1000, new string[]{"안녕 Player.:0",
                                             "내가 저녁에 물고기를 먹고싶은데:1",
                                             "물고기를 잡아 줄 수 있겠어?:0"});
    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
