using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener {
    static EventListener singletone;
    List<BindingInfo> bindingList;
    EventListener() {
        bindingList = new List<BindingInfo>();
    }

    public static EventListener GetEventListener() {
        if (singletone == null) singletone = new EventListener();
        return singletone;
    }
    // 이벤트 발생 시 기존에 바인딩 된 정보가 있는지 확인하고, 있다면 인수를 포함하여 바인딩한 곳으로 넘김.
    public void EventCall(Message message) {
        foreach(BindingInfo bindingInfo in bindingList) {
            if (bindingInfo.objectName == message.targetName 
                && bindingInfo.functionName == message.functionName) {
                Message temp = new Message(bindingInfo.eventCommand);
                temp.args.Add(message.args);
                temp.FunctionCall();
            }
        }
    }

    public void Binding(string objectName, string functionName, string eventCommand) {
        BindingInfo temp = new BindingInfo(objectName, functionName, eventCommand);
        bindingList.Add(temp);
    }

    public void Binding(BindingInfo bindingInfo) {
        bindingList.Add(bindingInfo);
    }

    // 입력과 일치하는 바인딩 제거.
    public void CancleBinding(string objectName, string functionName, string eventCommand) {
        for (int index = 0; index < bindingList.Count; index++) {
            if (bindingList[index].objectName == objectName
                && bindingList[index].functionName == functionName
                && bindingList[index].eventCommand == eventCommand) {
                    bindingList.RemoveAt(index);
                    break;
                }
        }
    }
}

public class BindingInfo {
    public string objectName;
    public string functionName;
    public string eventCommand;
    public BindingInfo(string objectName, string functionName, string eventCommand) {
        this.objectName = objectName;
        if (objectName == "") this.objectName = "BaseSystem";
        this.functionName = functionName;
        this.eventCommand = eventCommand;
    }
}