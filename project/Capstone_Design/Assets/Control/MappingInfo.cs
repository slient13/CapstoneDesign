using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 오브젝트마다 키 매핑 정보를 가질 수 있도록 하기 위함.
// ControlManager 에서 이 정보를 확인해서 매핑 사이클을 작성함.
[System.Serializable]
public class MappingInfo {
    public List<Info> infoList;
    public string objectName;
    public string group;
    public bool isDebug;
    public bool isNeedMousePos;

    public MappingInfo(bool isNeedMousePos = false, bool isDebug = false) {        
        infoList = new List<Info>();
        objectName = "";
        group = "general";
        this.isDebug = isDebug;
        this.isNeedMousePos = isNeedMousePos;
    }

    public MappingInfo(string name, bool isNeedMousePos = false, bool isDebug = false) : this(isNeedMousePos, isDebug) {
        objectName = name;
    }

    public void AddMapping(string command, string keyPattern) {
        // Message 용 command
        // InputChecker 용 keyPattern
        infoList.Add(new Info(command, keyPattern));
    }
    public void RemoveMapping(int index) {
        infoList.RemoveAt(index);
    }

    public List<string> GetMappingInfo() {
        List<string> output = new List<string>();
        foreach(Info info in infoList) {
            output.Add(info.command + " | " + info.keyPattern);
        }
        return output;
    }

    public void Reset() {   // 정보 리스트 리셋
        while(infoList.Count != 0) infoList.RemoveAt(0);
    }
    public MappingInfo Add(MappingInfo other) { // 정보 추가. 기존값 보존.
        foreach(Info info in other.infoList) {
            this.infoList.Add(new Info(info.command, info.keyPattern));
        }

        return this;
    }

    public MappingInfo Copy(MappingInfo other) { // 정보 복사. 기존값 초기화.
        this.Reset();
        this.Add(other);

        return this;
    }


    // 현재 매핑 정보를 general 그룹으로 등록하는 함수.
    public void Enroll() {
        Message msg = new Message("ControlManager/AddMapping : ");
        msg.args.Add(objectName);
        msg.args.Add(this);
        msg.args.Add(group);
        msg.FunctionCall();
    }
    
    // 등록 그룹을 지정
    public void Enroll(string groupName) {
        this.group = groupName;

        Message msg = new Message("ControlManager/AddMapping : ");
        msg.args.Add(objectName);
        msg.args.Add(this);
        msg.args.Add(group);
        msg.FunctionCall();
    }

    // 매핑 정보 변경 반영.
    public void MappingUpdate() {
        Message msg = new Message("ControlManager/UpdateMapping : ");
        msg.args.Add(objectName);
        msg.args.Add(this);
        msg.args.Add(group);
        msg.FunctionCall();
    }
}

