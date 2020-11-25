using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    InputChecker inputChecker = new InputChecker();
    public GameObject baseSystem;
    public List<GameObject> objectLayer;
    [SerializeField]
    public List<MappingInfo> mappingInfoList;
    public int target = 0;

    void Start() {
        baseSystem = GameObject.Find("BaseSystem");
        objectLayer = new List<GameObject>();
        mappingInfoList = new List<MappingInfo>();
    }

    void Update() {
        // 현재 입력 키 상태 지속 업데이트
        inputChecker.Update();
        // 키 매핑 확인
        if (objectLayer.Count != 0) mapping();
    }
    
    bool match(string pattern) {
        if (inputChecker.patternMatch(pattern) == 1) return true;
        else return false;
    }

    // 키 매칭 사용법
    // inputChecker.patterMatch(패턴스트링); -1 혹은 1 반환.
    // a = a down
    // _a = a downStay
    // ^a = a up
    // !a = a upStay
    void mapping() {
        if (match("t")) {
            if (target == 0) target = 1;
            else target = 0;
        }

        foreach(Info info in mappingInfoList[target].infoList) {
            if (match(info.keyPattern)) action(info.command);
        }
    }

    void action(string command) {
        // 타겟 오브젝트 정보를 뺀 명령어.
        Message msg = new Message(command);
        // 타겟 오브젝트 이름은 별도로 넣어줌.
        msg.targetName = objectLayer[target].name;
        // 함수 호출.
        msg.functionCall();
    }

    public void addMapping(Message msg) {
        string name = (string)msg.args[0]; // 등록 요청 오브젝트 이름
        MappingInfo mappingInfo = (MappingInfo)msg.args[1]; // 해당 오브젝트의 매핑 정보.
        GameObject target = GameObject.Find(name);
        if (target != null) {
            int beforeCount = objectLayer.Count;
            objectLayer.Add(target);
            Debug.Log("addMapping.mappingInfoList.Count : " + mappingInfoList.Count);
            mappingInfoList.Add(new MappingInfo());
            mappingInfoList[beforeCount].copy(mappingInfo);
        }
        else Debug.Log("ControlManager/addMapping.error = There is no object which name is " + name);
    }

    public void removeMapping(Message msg) {
        string name = (string)msg.args[0]; // 등록 요청 오브젝트 이름
        for (int i = 0; i < objectLayer.Count; i++) {
            if (objectLayer[i].name == name) {
                objectLayer.RemoveAt(i);
                mappingInfoList.RemoveAt(i);
            }
        }
    }
}