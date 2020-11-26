using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    InputChecker inputChecker = new InputChecker();
    public GameObject baseSystem;
    public List<GameObject> objectLayer = new List<GameObject>();
    public List<MappingInfo> mappingInfoList = new List<MappingInfo>();
    public int target = 0;

    void Start() {
        baseSystem = GameObject.Find("BaseSystem");
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
            Debug.Log("mapping.targetChange");
        }

        if (match("mouseL")) Debug.Log("mousePos : " + inputChecker.getMousePos());

        foreach(Info info in mappingInfoList[target].infoList) {
            if (match(info.keyPattern)) action(info.command);
        }
    }

    void action(string command) {
        // 타겟 오브젝트 정보를 뺀 명령어.
        Message msg = new Message(command);
        // 타겟 오브젝트 이름은 별도로 넣어줌.
        msg.targetName = objectLayer[target].name;
        // 첫번째 인수로 현재 마우스의 위치를 전달함. Vector2 형.
        Vector2 mousePos = new Vector2();
        mousePos = inputChecker.getMousePos();
        msg.args.Add(mousePos);
        // 함수 호출.
        msg.functionCall();
    }

    // 매핑정보 추가. 순서 무관하게 반드시 끝에 추가.
    public void addMapping(Message msg) {
        string name = (string)msg.args[0]; // 등록 요청 오브젝트 이름
        MappingInfo mappingInfo = (MappingInfo)msg.args[1]; // 해당 오브젝트의 매핑 정보.
        GameObject target = GameObject.Find(name);
        if (target != null) {
            int beforeCount = objectLayer.Count;
            objectLayer.Add(target);
            mappingInfoList.Add(new MappingInfo());
            mappingInfoList[beforeCount].copy(mappingInfo);
            Debug.Log("addMapping.mappingInfoList.Count : " + mappingInfoList.Count);
        }
        else Debug.Log("ControlManager/addMapping.error = There is no object which name is " + name);
    }

    // 매핑 정보 제거(오브젝트와 쌍으로 제거)
    public void removeMapping(Message msg) {
        Debug.Log("Called : removeMapping");
        string name = (string)msg.args[0]; // 등록 요청 오브젝트 이름
        int i;
        for (i = 0; i < objectLayer.Count; ) {
            if (objectLayer[i].name == name) {
                objectLayer.RemoveAt(i);
                mappingInfoList.RemoveAt(i);
            }
            else i++;
        }
        if (i == objectLayer.Count) Debug.Log("removeMapping.error : There is no mappingInfo which objectName is " + name);   
    }
    
    // 변경된 매핑정보 재반영.
    public void updateMapping(Message msg) {
        Debug.Log("Called : updateMapping");
        string name = (string)msg.args[0]; // 변경 대상 오브젝트의 이름
        MappingInfo newMappingInfo = (MappingInfo)msg.args[1]; // 해당 오브젝트의 새 매핑정보.
        int i;
        for (i = 0; i < objectLayer.Count; ) {
            if (name == objectLayer[i].name) {
                mappingInfoList[i].copy(newMappingInfo);
                break;
            }
            else i++;
        }
        if (i == objectLayer.Count) Debug.Log("updateMapping.error : There is no mappingInfo which objectName is " + name);   
    }

    // 매핑 대상 변경.
    public void layerChanger(Message msg) {
        string name = (string)msg.args[0]; // 변경 타겟 오브젝트의 이름
        int i;
        for (i = 0; i < objectLayer.Count; ) {
            if (name == objectLayer[i].name) {
                target = i;
                break;
            }
            else i++;
        }
        if (i == objectLayer.Count) Debug.Log("layerChanger.error : There is no mappingInfo which objectName is " + name);   
    }

}