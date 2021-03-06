﻿using System.Collections;
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
        else {
            target.SendMessage(msg.functionName, msg);
            EventListener.GetEventListener().EventCall(msg);
        }
        // Debug.Log(msg.returnValue[0]);
    }
    
    // 새 플레이 정보 등록 (외부용)
    public void CreatePlayInfo(Message msg) {
        string type =  (string)msg.args[0];     // 타입
        string name =  (string)msg.args[1];     // 이름
        object value = (int) msg.args[2];     // 값
        int min = 0;
        int max = 0;
        if (msg.args.Count == 5) {
            min = (int) msg.args[3];
            max = (int) msg.args[4];
        }
        if (type == null || type == "") {
            Debug.Log("BaseSystem/newPlayInfo.error : args don't include value or type infomation");
            return;
        }
        PlayInfo tempInfo = null;
        if (msg.args.Count == 5) tempInfo = new PlayInfo(type, name, (int) value, min, max);
        else                     tempInfo = new PlayInfo(type, name, (int) value);

        // 자둥 자료형 지원 시 이용.
        /*
        if (msg.args.Count == 5) {
            if (infoType == "int")          tempInfo = new PlayInfo(name, (int)value, min, max);
            else if (infoType == "float")   tempInfo = new PlayInfo(name, (float)value, min, max);
            else if (infoType == "string")  tempInfo = new PlayInfo(name, (string)value, min, max);
        }
        else {
            if (infoType == "int")          tempInfo = new PlayInfo(name, (int)value);
            else if (infoType == "float")   tempInfo = new PlayInfo(name, (float)value);
            else if (infoType == "string")  tempInfo = new PlayInfo(name, (string)value);
        }
        */

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
            // string temp = playInfoList[i].GetInfoName();
            // Debug.Log($"BaseSystem/findInfo : playInfoList[{i}].name = {temp}, infoName = {infoName}.");
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
            msg.returnValue.Add(playInfoList[index].type);              // 사용 분류를 위한 타입.
            msg.returnValue.Add(playInfoList[index].GetRange());        // 최소, 최대값.
            msg.returnValue.Add(playInfoList[index].GetInfoType());     // 언박싱 위한 타입
        }
    }

    public void GetPlayInfoList(Message msg) {
        msg.returnValue.Add(playInfoList);
    }
}

