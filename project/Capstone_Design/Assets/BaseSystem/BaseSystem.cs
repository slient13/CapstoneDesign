using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    public GameObject target;
    public List<PlayInfo> playInfoList = new List<PlayInfo>();

    void Start() {
    }

    // 임시로 아무런 동작을 요하지 않는 message 객체가 필요할 때 사용.
    public void None(Message message) {
        Debug.Log("BaseSystem.none is called.");
    }

    public void FunctionCaller(Message msg) {
        // Debug.Log(msg.args[0]);
        target = GameObject.Find(msg.targetName);
        // 타겟 포착 실패시 메세지 호출 후 함수 종료
        if (target == null) {
            Debug.Log("BaseSystem.functionCaller.error : Could not find object. targetName = " + msg.targetName);
            return;
        }
        target.SendMessage(msg.functionName, msg);
        // Debug.Log(msg.returnValue[0]);
    }
    
    // 새 플레이 정보 등록 (외부용)
    public void CreatePlayInfo(Message msg) {
        string infoName =  (string)msg.args[0];     // 이름
        string infoType =  (string)msg.args[1];     // 타입
        object infoValue = msg.args[2];             // 값
        int min = 0;
        int max = 0;
        if (msg.args.Count == 5) {
            min = (int) msg.args[3];
            max = (int) msg.args[4];
        }
        if (infoType == null || infoType == "") {
            Debug.Log("BaseSystem/newPlayInfo.error : args don't include value or type infomation");
            return;
        }
        PlayInfo tempInfo = null;
        if (msg.args.Count == 5) {
            if (infoType == "int")          tempInfo = new PlayInfo(infoName, (int)infoValue, min, max);
            else if (infoType == "float")   tempInfo = new PlayInfo(infoName, (float)infoValue, min, max);
            else if (infoType == "string")  tempInfo = new PlayInfo(infoName, (string)infoValue, min, max);
        }
        else {
            if (infoType == "int")          tempInfo = new PlayInfo(infoName, (int)infoValue);
            else if (infoType == "float")   tempInfo = new PlayInfo(infoName, (float)infoValue);
            else if (infoType == "string")  tempInfo = new PlayInfo(infoName, (string)infoValue);
        }

        if (tempInfo != null) playInfoList.Add(tempInfo);
    }

    public void SetPlayInfo(Message msg) {
        string infoName = (string)msg.args[0];  // 변경 대상 이름.
        object value = msg.args[1];             // 변경 값.
        int index = findInfo(infoName);

        if (index == -1) Debug.Log("BaseSystem/playInfoChanger.error : there is no information which name is " + infoName);
        else playInfoList[index].SetValue(value);
    }
    public void ChangePlayInfo(Message msg) {
        string infoName = (string)msg.args[0];  // 변경 대상 이름.
        object value = msg.args[1];             // 변경 값.
        int index = findInfo(infoName);

        if (index == -1) Debug.Log("BaseSystem/playInfoChanger.error : there is no information which name is " + infoName);
        else playInfoList[index].ModifyValue(value);
    }

    int findInfo(string infoName) {
        int i;
        for (i = 0; i < playInfoList.Count; ) {
            if (playInfoList[i].GetInfoName() == infoName) return i;
            else i++;
        }
        
        // 중간에 루프를 탈출하지 않았다면 못 찾은 것이므로 -1 반환.
        return -1;
    }

    public void GetPlayInfoValue(Message msg) {
        string infoName = (string) msg.args[0]; // 정보를 원하는 대상 이름.
        int index = findInfo(infoName);
        if (index == -1) Debug.Log("BaseSystem/getPlayInfo.error : there is no information which name is " + infoName);
        else {
            msg.returnValue.Add(playInfoList[index].GetInfoValue());    // 오브젝트 형으로 정보 반환.
            msg.returnValue.Add(playInfoList[index].GetInfoType());     // 언박싱 위한 타입
        }
    }

    public void GetPlayInfoList(Message msg) {
        msg.returnValue.Add(playInfoList);
    }
}

