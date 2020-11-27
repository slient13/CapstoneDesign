using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    public GameObject target;
    public List<PlayInfo> playInfoList = new List<PlayInfo>();

    void Start() {
    }

    public void functionCaller(Message msg) {
        // Debug.Log(msg.args[0]);
        target = GameObject.Find(msg.targetName);
        // 타겟 포착 실패시 메세지 호출 후 함수 종료
        if (target == null) {
            Debug.Log("Basesystem.functionCaller.error : Could not find object.");
            return;
        }
        target.SendMessage(msg.functionName, msg);
        // Debug.Log(msg.returnValue[0]);
    }
    
    // 새 플레이 정보 등록 (내부용)
    void newPlayInfo(string infoName, int value) {
        playInfoList.Add(new PlayInfo(infoName, value));
    }
    void newPlayInfo(string infoName, float value) {
        playInfoList.Add(new PlayInfo(infoName, value));
    }
    void newPlayInfo(string infoName, string value) {
        playInfoList.Add(new PlayInfo(infoName, value));
    }
    // 새 플레이 정보 등록 (외부용)
    public void newPlayInfo(Message msg) {
        string infoName =  (string)msg.args[0];     // 이름
        string infoType =  (string)msg.args[1];     // 타입
        object infoValue = msg.args[2];             // 값
        PlayInfo tempInfo;
        if (infoType == "int") tempInfo = new PlayInfo(infoName, (int)infoValue);
        else if (infoType == "float") tempInfo = new PlayInfo(infoName, (float)infoValue);
        else if (infoType == "string") tempInfo = new PlayInfo(infoName, (string)infoValue);
        else {  // type 이 없는 경우
            Debug.Log("BaseSystem/newPlayInfo.error : args don't include value or type infomation");
            return;
        }

        playInfoList.Add(tempInfo);
    }

    public void playInfoSetter(Message msg) {
        string infoName = (string)msg.args[0];  // 변경 대상 이름.
        object value = msg.args[1];             // 변경 값.
        int index = findInfo(infoName);

        if (index == -1) Debug.Log("BaseSystem/playInfoChanger.error : there is no information which name is " + infoName);
        else playInfoList[index].setValue(value);
    }
    public void playInfoChanger(Message msg) {
        string infoName = (string)msg.args[0];  // 변경 대상 이름.
        object value = msg.args[1];             // 변경 값.
        int index = findInfo(infoName);

        if (index == -1) Debug.Log("BaseSystem/playInfoChanger.error : there is no information which name is " + infoName);
        else playInfoList[index].modifyValue(value);
    }

    int findInfo(string infoName) {
        int i;
        for (i = 0; i < playInfoList.Count; ) {
            if (playInfoList[i].getInfoName() == infoName) return i;
            else i++;
        }
        
        // 중간에 루프를 탈출하지 않았다면 못 찾은 것이므로 -1 반환.
        return -1;
    }

    public void getPlayInfo(Message msg) {
        string infoName = (string) msg.args[0]; // 정보를 원하는 대상 이름.
        int index = findInfo(infoName);
        if (index == -1) Debug.Log("BaseSystem/getPlayInfo.error : there is no information which name is " + infoName);
        else {
            msg.returnValue.Add(playInfoList[index].getInfoValue());    // 오브젝트 형으로 정보 반환.
            msg.returnValue.Add(playInfoList[index].getInfoType());     // 언박싱 위한 타입
        }
    }
}

