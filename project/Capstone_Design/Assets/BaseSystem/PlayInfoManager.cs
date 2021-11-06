using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfoManager : MonoBehaviour
{
    string[] infoFileNameList = {
        "Player/Stat",
        "System/Process",
        "System/Player"
    };

    public List<Creature> creatureList;
    void Awake()
    {
        foreach (string fileName in infoFileNameList) getPlayInfo(fileName);
        loadPlayData();
    }

    // Update is called once per frame
    float time = 0.0f;
    public bool sp_recovery_switch = true;
    void Update()
    {
        if (sp_recovery_switch == false) return;

        if (time > 10.0f)
        {
            time = 0.0f;
            new Message($"ChangeData : Player.Stat.Sp, 5").FunctionCall();
        }
        time += Time.deltaTime;
    }

    void SetSpRecoveryOnOff(Message message)
    {
        int mode = (int)message.args[0];

        if (mode == 1) this.sp_recovery_switch = true;
        else if (mode == 0) this.sp_recovery_switch = false;
        else this.sp_recovery_switch = !this.sp_recovery_switch;
    }

    // 플레이 정보 등록.
    void getPlayInfo(string fileName)
    {
        List<string> infoStringList = ExternalFileSystem.SingleTon().GetPlayInfo(fileName);
        foreach (string infoString in infoStringList)
        {
            // Debug.Log($"PlayInfoManager.getPlayInfo.debug : infoString = {infoString}");
            new Message("CreatePlayInfo : " + infoString).FunctionCall();
        }
    }

    // 플레이 정보 로드.    
    void loadPlayData()
    {
        Debug.Log("PlayInfoManager.loadPlayData : is called");
        List<string> dataStringList = ExternalFileSystem.SingleTon().LoadPlayData();
        foreach (string dataString in dataStringList)
        {
            new Message("SetData : " + dataString).FunctionCall();
        }
    }

    public void SavePlayData(Message message)
    {
        Debug.Log("PlayInfoManager.SavePlayData : is called");
        Message getPlayInfoList = new Message("GetPlayInfo : ").FunctionCall();
        PlayInfo root_playinfo = (PlayInfo)getPlayInfoList.returnValue[0];
        ExternalFileSystem.SingleTon().SavePlayData(root_playinfo);
    }
}

public class Creature
{
    public string type { get; }
    public string code { get; }
    public string name { get; }
    public int hp { get; }
    public int attack { get; }
    public int defense { get; }
    public List<Skill> skillList;
    public List<Drop> dropList;

    public struct Skill
    {
        public string code { get; }
        public string name { get; }
        public double effect { get; }
        public Skill(string code, string name, double effect)
        {
            this.code = code;
            this.name = name;
            this.effect = effect;
        }
    }

    public struct Drop
    {
        public string dropItemCode { get; }
        public double rate { get; }
        public Drop(string dropItemCode, double rate)
        {
            this.dropItemCode = dropItemCode;
            this.rate = rate;
        }
    }

    public Creature(string type = "None", string code = "None", string name = "None"
                    , string hp = "0", string attack = "0", string defense = "0"
                    , List<string> skillStringList = null, List<string> dropStringList = null)
    {
        this.type = type;
        this.code = code;
        this.name = name;
        // 숫자 자료형 할당.
        this.hp = Convert.ToInt32(hp);
        this.attack = Convert.ToInt32(attack);
        this.defense = Convert.ToInt32(defense);
        // 메모리 할당.
        skillList = new List<Skill>();
        dropList = new List<Drop>();
        // 스킬 입력
        foreach (string skillString in skillStringList)
        {
            string[] temp = skillString.Split(',');
            string skillCode = temp[0].Trim();
            string skillName = temp[1].Trim();
            double skillEffect = Convert.ToDouble(temp[2].Trim());
            skillList.Add(new Skill(skillCode, skillName, skillEffect));
        }
        // 드롭 입력
        foreach (string dropString in dropStringList)
        {
            string[] temp = dropString.Split(',');
            string dropItemCode = temp[0].Trim();
            double rate = Convert.ToDouble(temp[1].Trim());
            dropList.Add(new Drop(dropItemCode, rate));
        }
    }
}