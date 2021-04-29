using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    InputChecker inputChecker = new InputChecker();
    public GameObject baseSystem;
    public List<GameObject> objectLayer = new List<GameObject>();
    public List<MappingInfo> mappingInfoList = new List<MappingInfo>();
    public List<string> groupList = new List<string>();
    public List<string> enrolledGroupList = new List<string>();
    public string currentMappingGroup;

    void Start() {
        baseSystem = GameObject.Find("BaseSystem");
        enrolledGroupList.Add("general");
        currentMappingGroup = "general";
    }

    void Update() {
        // 현재 입력 키 상태 지속 업데이트
        inputChecker.Update();
        // 키 매핑 확인
        if (objectLayer.Count != 0) mapping();
    }
    
    bool match(string pattern) {
        if (inputChecker.PatternMatch(pattern) == 1) return true;
        else return false;
    }

    // 키 매칭 사용법
    // inputChecker.patterMatch(패턴스트링); -1 혹은 1 반환.
    // a = a down
    // _a = a downStay
    // ^a = a up
    // !a = a upStay
    void mapping() {
        if (match("mouseL")) Debug.Log("mousePos : " + inputChecker.GetMousePos());

        for (int index = 0; index < mappingInfoList.Count; index++) {
            if (groupList[index] == currentMappingGroup) {
                foreach(Info info in mappingInfoList[index].infoList) {
                    if (match(info.keyPattern)) action(info.command, index, info.keyPattern);
                }
            }
        }
    }

    void action(string command, int target, string keyPattern) {
        // 타겟 오브젝트 정보를 뺀 명령어.
        Message msg = new Message(command);
        // 타겟 오브젝트 이름은 별도로 넣어줌.
        msg.targetName = objectLayer[target].name;
        // 첫번째 인수로 현재 마우스의 위치를 전달함. Vector2 형.
        Vector2 mousePos = new Vector2();
        mousePos = inputChecker.GetMousePos();
        msg.args.Add(mousePos);
        // 함수 호출.
        msg.FunctionCall();
        Debug.Log("ControlManager.action : keyPattern = " + keyPattern + ", command = " + command);
    }

    // 매핑정보 추가. 순서 무관하게 반드시 끝에 추가.
    public void AddMapping(Message msg) {
        string name = (string)msg.args[0];  // 등록 요청 오브젝트 이름
        MappingInfo mappingInfo = (MappingInfo)msg.args[1]; // 해당 오브젝트의 매핑 정보.
        string group = (string)msg.args[2]; // 등록 대상 그룹
        GameObject target = GameObject.Find(name);
        if (target != null) {
            int beforeCount = objectLayer.Count;
            // 오브젝트 츠가
            objectLayer.Add(target);
            // 매핑 정보 추가
            mappingInfoList.Add(new MappingInfo());
            mappingInfoList[beforeCount].Copy(mappingInfo);
            Debug.Log("AddMapping.mappingInfoList.Object : " + name);
            // 그룹 정보 추가
            groupList.Add(group);
            // 기 등록 그룹이 아닌 경우 그룹 등록
            if (!isInGroup(group)) enrolledGroupList.Add(group);
        }
        else Debug.Log("ControlManager/AddMapping.error = There is no object which name is " + name);
    }

    // 매핑 정보 제거(오브젝트와 쌍으로 제거)
    public void RemoveMapping(Message msg) {
        Debug.Log("Called : RemoveMapping");
        string name = (string)msg.args[0]; // 등록 요청 오브젝트 이름
        int i;
        for (i = 0; i < objectLayer.Count; ) {
            if (objectLayer[i].name == name) {
                objectLayer.RemoveAt(i);
                mappingInfoList.RemoveAt(i);
                groupList.RemoveAt(i);
                i--;    // 삭제 성공 구분용.
            }
            else i++;
        }
        if (i == objectLayer.Count) Debug.Log("RemoveMapping.error : There is no mappingInfo which objectName is " + name);   
    }
    
    // 변경된 매핑정보 재반영.
    public void UpdateMapping(Message msg) {
        Debug.Log("Called : UpdateMapping");
        string name = (string)msg.args[0]; // 변경 대상 오브젝트의 이름
        MappingInfo newMappingInfo = (MappingInfo)msg.args[1]; // 해당 오브젝트의 새 매핑정보.
        string group = (string)msg.args[2]; // 변경 대상 그룹 이름.
        int i;
        // 관련 그룹이 존재하지 않는다면 매핑 중단.
        if (!isInGroup(group)) {
            Debug.Log("UpdateMapping.error : There is no group which is " + group);   
            return;
        }
        // 일치 그룹 위치 확인. 존재시 값 업데이트.
        for (i = 0; i < objectLayer.Count; ) {
            if (name == objectLayer[i].name) {
                mappingInfoList[i].Copy(newMappingInfo);
                groupList[i] = group;
                break;
            }
            else i++;
        }
        // 일치정보 없는 경우 디버그 로그 출력.
        if (i == objectLayer.Count) Debug.Log("UpdateMapping.error : There is no mappingInfo which objectName is " + name);   
    }

    // 매핑 대상 변경.
    public void LayerChanger(Message msg) {
        string name = (string)msg.args[0]; // 변경 그룹 이름
        // 해당 이름을 가진 그룹이 있는지 검사
        if (isInGroup(name)) {
            currentMappingGroup = name;
        }
        else {
            Debug.Log("LayerChanger.error : There is no group which is " + name);   
        }
    }

    bool isInGroup(string name) {
        foreach(string groupName in enrolledGroupList) {
            if (name == groupName) return true;
        }
        return false;
    }

    public void GetMousePos(Message msg) {
        msg.returnValue.Add(inputChecker.GetMousePos());
    }
}