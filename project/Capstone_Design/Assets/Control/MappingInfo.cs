using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 오브젝트마다 키 매핑 정보를 가질 수 있도록 하기 위함.
// ControlManager 에서 이 정보를 확인해서 매핑 사이클을 작성함.
public class MappingInfo {
    public List<Info> infoList;
    public string objectName;

    public MappingInfo() {        
        infoList = new List<Info>();
        objectName = "";
    }

    public MappingInfo(string name) : this() {
        objectName = name;
    }

    public void addMapping(string command, string keyPattern) {
        // Message 용 command
        // InputChecker 용 keyPattern
        infoList.Add(new Info(command, keyPattern));
    }
    public void removeMapping(int index) {
        infoList.RemoveAt(index);
    }

    public List<string> getMappingInfo() {
        List<string> output = new List<string>();
        foreach(Info info in infoList) {
            output.Add(info.command + " | " + info.keyPattern);
        }
        return output;
    }

    public void reset() {   // 정보 리스트 리셋
        while(infoList.Count != 0) infoList.RemoveAt(0);
    }

    public MappingInfo copy(MappingInfo other) { // 값 복사. 기존값 무시.
        this.reset();
        foreach(Info info in other.infoList) {
            this.infoList.Add(new Info(info.command, info.keyPattern));
        }

        return this;
    }

    // 현재 오브젝트의 키 매핑 정보를 ControlManger 에 등록하는 함수.
    public void enroll() {
        Message msg = new Message("ControlManager/addMapping : ");
        msg.args.Add(objectName);
        msg.args.Add(this);
        msg.functionCall();
    }
    // 사전에 객체 이름을 할당하지 않은 경우
    public void enroll(string objectName) {
        Message msg = new Message("ControlManager/addMapping : ");
        msg.args.Add(objectName);
        msg.args.Add(this);
        msg.functionCall();
    }
}

