using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfoManager : MonoBehaviour
{
    string[] infoFileNameList = {
        "Player/Stat"
    };

    string[] creatureInfoPathList = {
        "Rpg/Monster/Info"
    };
    public List<Creature> creatureList;
    // // 테스트용.
    // public string testType = "";
    // public string testCode;
    // public string testName;
    // public string testHp;
    // public string testAttack;
    // public string testDefense;
    void Awake()
    {
        foreach(string fileName in infoFileNameList) getPlayInfo(fileName);
        loadPlayData();
        creatureList = new List<Creature>();
        GetCreatureInfo();
        // // 테스트 코드 : 실행 잘 됨.
        // Message testGet = new Message("PlayInfoManager/GetData : Creature, W_Pig").FunctionCall();
        // Creature temp = (Creature) testGet.returnValue[0];
        // testType = temp.type;
        // testCode = temp.code;
        // testName = temp.name;
        // testHp = $"{temp.hp}";
        // testAttack = $"{temp.attack}";
        // testDefense = $"{temp.defense}";
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 플레이 정보 등록.
    void getPlayInfo(string fileName) {
        List<string> infoStringList = ExternalFileSystem.SingleTon().GetPlayInfo(fileName);
        foreach(string infoString in infoStringList) {
            new Message("CreatePlayInfo : " + infoString).FunctionCall();
        }
    }

    // 플레이 정보 로드.    
    void loadPlayData() {
        Debug.Log("PlayInfoManager.loadPlayData : is called");
        List<string> dataStringList = ExternalFileSystem.SingleTon().LoadPlayData();
        foreach(string dataString in dataStringList) {
            new Message("SetPlayInfo : " + dataString).FunctionCall();
        }
    }

    // 몬스터 정보 로드. 수동형.
    public void GetCreatureInfo() {
        foreach(string filepath in creatureInfoPathList) {
            List<Creature> temp = ExternalFileSystem.SingleTon().GetCreatureInfo(filepath);
            creatureList.AddRange(temp);
        }
    }

    public void SavePlayData(Message message) {
        Debug.Log("PlayInfoManager.SavePlayData : is called");
        Message getPlayInfoList = new Message("GetPlayInfoList : ").FunctionCall();
        List<PlayInfo> playinfoList = (List<PlayInfo>) getPlayInfoList.returnValue[0];
        ExternalFileSystem.SingleTon().SavePlayData(playinfoList);
    }

    public void ChangeData(Message message) {
        string targetName = (string) message.args[0];
        int degree = (int) message.args[1];

        // if (targetName == "Hp") changeHp(degree);
        // else if (targetName == "MaxHp") changeMaxHp(degree);
        // else if (targetName == "Money") changeMoney(degree);
        // else {
            new Message($"ChangePlayInfo : {targetName}, {degree}").FunctionCall();
        // }
    }

    public void GetData(Message message) {
        if (message.args.Count == 1) {
            string targetName = (string) message.args[0];
            Message data = new Message($"GetPlayInfoValue : {targetName}").FunctionCall();
            message.returnValue.Add(data.returnValue[0]);
            message.returnValue.Add(data.returnValue[1]);
            message.returnValue.Add(((int[]) data.returnValue[2])[0]);
            message.returnValue.Add(((int[]) data.returnValue[2])[1]);
        }
        else if (message.args.Count == 2) {
            string type = (string) message.args[0];
            string code = (string) message.args[1];
            if (type == "Creature") {
                Creature creature = creatureList.Find(x => x.code == code);
                message.returnValue.Add(creature);
            }
        }
    }

    // void changeHp(int degree) {
    //     Message before = new Message("PlayInfoManager/GetData : Hp").FunctionCall();
    //     Message max = new Message("PlayInfoManager/GetData : MaxHp").FunctionCall();
    //     int beforeHp = (int) before.returnValue[0];
    //     int maxHp = (int) max.returnValue[0];

    //     int afterHp = beforeHp + degree;
    //     if (afterHp < 0) afterHp = 0;
    //     else if (afterHp > maxHp) afterHp = maxHp;
    //     new Message("SetPlayInfo : Hp, " + afterHp).FunctionCall();
    // }

    // void changeMaxHp(int degree) {
    //     Message before = new Message("PlayInfoManager/GetData : MaxHp").FunctionCall();
    //     int maxHp = (int) before.returnValue[0];
    //     maxHp += degree;
    //     if (maxHp < 0) maxHp = 0;
    //     new Message("SetPlayInfo : MaxHp, " + maxHp).FunctionCall();
    // }

    // void changeMoney(int degree) {
    //     Message before = new Message("PlayInfoManager/GetData : Money").FunctionCall();
    //     int beforeMoney = (int) before.returnValue[0];

    //     int afterMoney = beforeMoney + degree;
    //     if (afterMoney < 0) afterMoney = 0;
    //     else if (afterMoney > 1000000) afterMoney = 1000000;
    //     new Message("SetPlayInfo : Money, "+ afterMoney).FunctionCall();
    // }
}

public class Creature {
    public string type {get;}
    public string code {get;}
    public string name {get;}
    public int hp {get;}
    public int attack {get;}
    public int defense {get;}
    public List<Skill> skillList;
    public List<Drop> dropList;

    public struct Skill {
        public string code {get;}
        public string name {get;}
        public double effect {get;}
        public Skill(string code, string name, double effect) {
            this.code = code;
            this.name = name;
            this.effect = effect;
        }
    }

    public struct Drop {
        public string dropItemCode {get;}
        public double rate {get;}
        public Drop(string dropItemCode, double rate) {
            this.dropItemCode = dropItemCode;
            this.rate = rate;
        }
    }

    public Creature(string type = "None", string code = "None", string name = "None"
                    , string hp = "0", string attack = "0", string defense = "0"
                    , List<string> skillStringList = null, List<string> dropStringList = null) {
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
        foreach(string skillString in skillStringList) {
            string[] temp = skillString.Split(',');
            string skillCode = temp[0].Trim();
            string skillName = temp[1].Trim();
            double skillEffect = Convert.ToDouble(temp[2].Trim());
            skillList.Add(new Skill(skillCode, skillName, skillEffect));
        }
        // 드롭 입력
        foreach(string dropString in dropStringList) {
            string[] temp = dropString.Split(',');
            string dropItemCode = temp[0].Trim();
            double rate = Convert.ToDouble(temp[1].Trim());
            dropList.Add(new Drop(dropItemCode, rate));            
        }
    }
}