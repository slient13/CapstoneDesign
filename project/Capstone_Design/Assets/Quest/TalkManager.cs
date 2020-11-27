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
    void GenerateData()
    {
        talkData.Add(1000, new string[] { "나는 Quest NPC 입니다.","원하시는 Quest가 있으십니까?" });
    }
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
