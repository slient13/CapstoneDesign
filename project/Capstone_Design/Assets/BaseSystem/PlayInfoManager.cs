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
        hp_sp_recovery();
    }

    void hp_sp_recovery()
    {
        if (sp_recovery_switch == false) return;

        if (time > 10.0f)
        {
            time = 0.0f;
            PlayInfo sp = (PlayInfo) new Message($"GetPlayInfo : Player.Stat.Sp").FunctionCall().returnValue[0];
            if (((int)sp.GetValue(0)) != sp.GetRange(0)[1])
                new Message($"ChangeData : Player.Stat.Sp, 5").FunctionCall();
            else 
                new Message($"ChangeData : Player.Stat.Hp, 2").FunctionCall();
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
