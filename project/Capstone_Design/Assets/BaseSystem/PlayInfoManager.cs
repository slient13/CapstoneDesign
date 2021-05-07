using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfoManager : MonoBehaviour
{
    string[] infoFileNameList = {
        "Player/Stat"
    };
    void Start()
    {
        foreach(string fileName in infoFileNameList) getPlayInfo(fileName);
        loadPlayData();
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

    // 플레이어 정보 로드.
    void loadPlayData() {
        Debug.Log("PlayInfoManager.loadPlayData : is called");
        List<string> dataStringList = ExternalFileSystem.SingleTon().LoadPlayData();
        foreach(string dataString in dataStringList) {
            new Message("SetPlayInfo : " + dataString).FunctionCall();
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

        if (targetName == "hp") changeHp(degree);
        // else if (targetName == "maxHp") changeMaxHp(degree);
        else if (targetName == "money") changeMoney(degree);
        else {
            new Message("ChangePlayInfo : " + targetName + ", " + degree).FunctionCall();
        }
    }

    public void GetData(Message message) {
        string targetName = (string) message.args[0];
        Message data = new Message("GetPlayInfoValue : " + targetName).FunctionCall();
        message.returnValue.Add(data.returnValue[0]);
        message.returnValue.Add(data.returnValue[1]);
    }

    void changeHp(int degree) {
        Message before = new Message("PlayInfoManager/GetData : hp").FunctionCall();
        Message max = new Message("PlayInfoManager/GetData : maxHp").FunctionCall();
        int beforeHp = (int) before.returnValue[0];
        int maxHp = (int) max.returnValue[0];

        int afterHp = beforeHp + degree;
        if (afterHp < 0) afterHp = 0;
        else if (afterHp > maxHp) afterHp = maxHp;
        new Message("SetPlayInfo : hp, " + afterHp).FunctionCall();
    }

    void changeMaxHp(int degree) {
        Message before = new Message("PlayInfoManager/GetData : maxHp").FunctionCall();
        int maxHp = (int) before.returnValue[0];
        maxHp += degree;
        if (maxHp < 0) maxHp = 0;
        new Message("SetPlayInfo : maxHp, " + maxHp).FunctionCall();
    }

    void changeMoney(int degree) {
        Message before = new Message("PlayInfoManager/GetData : money").FunctionCall();
        int beforeMoney = (int) before.returnValue[0];

        int afterMoney = beforeMoney + degree;
        if (afterMoney < 0) afterMoney = 0;
        else if (afterMoney > 1000000) afterMoney = 1000000;
        new Message("SetPlayInfo : money, "+ afterMoney).FunctionCall();
    }
}
