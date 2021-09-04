using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public string targetName;
    public string functionName;
    public ArrayList args;
    public ArrayList returnValue;

    ArrayList argString;

    public GameObject baseSystem;

    public Message() {
        targetName = "";
        functionName = "";
        args = new ArrayList();
        returnValue = new ArrayList();
        argString = new ArrayList();
        baseSystem = GameObject.Find("BaseSystem");
    }
    public Message(string command) : this() {
        codeSpliter(command);
        if (argString.Count != 0) splitCodeConverter(ref argString, ref args);
        // codeRunner();
    }
    public Message(string command, GameObject baseSystem) : this() {
        codeSpliter(command);
        if (argString.Count != 0) splitCodeConverter(ref argString, ref args);
        this.baseSystem = baseSystem;
    }


    void codeSpliter(string cmd) {
        int index = 0;

        string command = cmd.Trim();

        // 함수명 추출
        getFunctionName(command, ref index);

        // 인수부 분리
        if (index < command.Length) splitArgText(command, ref index, ref argString);            
    }

    // 함수명 추출
    void getFunctionName(string command, ref int index) {
        string temp = "";

        while(command[index] != ':') {
            temp += command[index++];
        }
        // 인수부 진입까지 인덱스 이동
        index++;
        while(index < command.Length && command[index] == ' ' ) {
            index++;
        }

        string[] tempResult = temp.Trim().Split('/');
        if (tempResult.Length == 2) {
            this.targetName = tempResult[0];
            this.functionName = tempResult[1];
        }
        else this.functionName = tempResult[0];
    }

    // 인수부 문자열 분리
    void splitArgText(string command, ref int index, ref ArrayList strList) {
        string temp = "";
        int level;
        // Debug.Log("command = " + command + ". index = " + index + ". command[index] = " + command[index]);

        while(true) {
            // 배열 구분
            if (command[index] == '[') {
                // 배열 파트
                level = 0;
                // 해당 레벨 문자열 분리
                do {
                    if (command[index] == '[') level++;
                    if (command[index] == ']') level--;
                    temp += command[index++];
                } while(level > 0);
                // 추가 객체 생성 후 재귀
                ArrayList subStrList = new ArrayList();
                int subIndex = 0;            
                while(subIndex < temp.Length - 2) {
                    splitArgText(temp.Substring(1, temp.Length - 2).Trim(), ref subIndex, ref subStrList);
                }
                // 처리 완료 컬렉션 연결
                strList.Add(subStrList);
            }     
            // 특수 문자가 섞인 문자열 분리.
            else if (command[index] == '(') {
                // Debug.Log($"Message.debug : command = {command}");
                level = 1;
                ++index;
                do {
                    temp += command[index++];
                    if (command[index] == '(') level++;
                    if (command[index] == ')') level--;
                } while(level > 0);
                ++index;

                strList.Add(temp.Trim());
            }
            else {
                // 일반 인수 처리 파트
                // 콤마 까지 모든 문자열 읽음
                while(command[index] != ',') {
                    // 줄바꿈이 아닌 경우 삽입
                    if (command[index] != '\n') 
                        temp += command[index++];
                    
                    // 문자열 길이를 초과하는 경우 중단.
                    if (index == command.Length) break;
                }
                // 좌우 공백 제거 후 컬렉션에 삽입.
                strList.Add(temp.Trim());
            }
            // 문자열 끝이면 루프 종료
            if (index == command.Length) break;

            // 임시 문자열 버퍼 초기화
            temp = "";
            // 우선 콤마 건너뛰고 공백 아닌 구역까지 스킵
            while(command[++index] == ' ') {
            }
        } 
    }

    // 문자열로 분리된 인수들을 변수로 변환
    void splitCodeConverter(ref ArrayList strList, ref ArrayList valList) {
        for (int i = 0; i < strList.Count; i++) {
            // 인수가 배열인지 여부 확인 (해당 객체의 자료형 검사. String -> 단일 인수)
            if (strList[i].GetType().Name == "String") {
                convertAct(ref valList, (string)strList[i]); 
            }
            else { 
                // 
                ArrayList tempStrList = (ArrayList)strList[i];
                ArrayList tempValList = new ArrayList();
                splitCodeConverter(ref tempStrList, ref tempValList);
                valList.Add(tempValList);
            }
        }
    }

    void convertAct(ref ArrayList list, string command) {
        // 숫자 변환 가능 여부 확인
        int tempInt;
        float tempFloat;
        // Debug.Log("convertAct : command = " + command);
        try {
            tempInt = Convert.ToInt32(command);
            tempFloat = Convert.ToSingle(command);
            if (command.IndexOf('.') == -1) list.Add(tempInt);
            else list.Add(tempFloat);
        }
        catch {
            list.Add(command);  
        }
    }

    // 자기 자신을 인수로 전달하며 함수 중계 요청.
    public Message FunctionCall () {
        // 타겟을 별도로 지정하지 않는 경우 자동으로 'BaseSystem'을 향해 전송.
        if (targetName == "") targetName = "BaseSystem";
        this.baseSystem.SendMessage("FunctionCaller", this);
        return this;
    }

    // 자동으로 baseSystem 을 찾지 못하는 경우 사용.
    public Message FunctionCall (GameObject baseSystem) {
        baseSystem.SendMessage("FunctionCaller", this);
        return this;
    }

    public string GetCommand() {
        string output = "";
        output += targetName + "/" + functionName + " : ";
        for (int i = 0; i < args.Count; i++) {
            try {
                output += args[i];
            }
            catch {
                output += "args" + i;
            }
            if (i < args.Count - 1) output += ", ";
        }

        return output;
    }
}
