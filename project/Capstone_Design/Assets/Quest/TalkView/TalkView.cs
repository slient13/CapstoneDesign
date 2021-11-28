using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TalkView : MonoBehaviour
{
    const int NO_SELECTION = -1;        // 아무것도 아님. 선택 불가.
    const int TALK_END = -2;            // 대화 종료.
    private string npcName;             // 현재 대화 중인 NPC 이름.
    public List<Talk> talkList;         // 전체 대화 리스트.
    public Talk currentTalk;            // 현재 대화 리스트.

    public Text npcNameText;            // npc 이름 표시 텍스트 UI
    public Text npcTalkText;            // npc 대화 표시 텍스트 UI
    public List<Text> playerAnswersText;    // 플레이어 선택지 Text 리스트

    public GameObject background;
    public GameObject talkView;
    string talkUIName;

    /*
    구조 : 
        TalkUI : 
            Background
            TalkView : 
                NpcName
                NpcTalk : Text
                SelectTalk : 
                    Button : Text
                    Button : Text
                    Button : Text
                    Button : Text
    */
    void Start() {
        npcNameText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        npcTalkText = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();
        for (int i = 0; i < 4; i++) {
            Text tempText = transform.GetChild(1).GetChild(2).GetChild(i).GetChild(0).GetComponent<Text>();
            playerAnswersText.Add(tempText);
        }
        talkView = transform.Find("TalkView").gameObject;   // TalkView
        background = transform.Find("Background").gameObject;

        talkUIName = gameObject.name;     // TalkUI

        MappingInfo mapping = new MappingInfo(talkUIName);  
        mapping.AddMapping("AnswerSelect : 0", "n1");
        mapping.AddMapping("AnswerSelect : 1", "n2");
        mapping.AddMapping("AnswerSelect : 2", "n3");
        mapping.AddMapping("AnswerSelect : 3", "n4");
        mapping.Enroll("talkView");

        // 테스트용 시작 코드
        // MappingInfo startMap = new MappingInfo(talkUIName);
        // startMap.AddMapping("StartTalkByKey : npc", "space");
        // startMap.Enroll();

        background.SetActive(false);
        talkView.SetActive(false);
        
    }

    void Update() {
    }

    public void StartTalkByKey(Message message) {
        string name = (string)message.args[0];  // 대화 대상 이름.
        int startId;                            // 시작 대화 위치. 필요에 따라 다른 포지션에서 시작해야 하는 경우 이용.
        if (message.args.Count == 1) startId = 0;
        else startId = (int) message.args[1];
        StartTalk(name, startId);
        new Message("ControlManager/LayerChanger : talkView").FunctionCall();
    }

    public void StartTalk(string npcName, int startId) {
        Debug.Log($"TalkView/StartTalk.Notice : npcName = {npcName}, startId = {startId}");
        background.SetActive(true);     // 배경 활성화.
        talkView.SetActive(true);       // talkView 활성화.
        this.npcName = npcName;         // 대화 NPC 이름 변경.
        loadTalkScript(npcName);        // 해당 NPC의 대화 목록 불러옴.
        ChangeTalk(startId);            // 지정된 첫 시작 위치로 변경.
    }

    void loadTalkScript(string npcName) {
        talkList = new List<Talk>();
        // 문자열 리스트 로드.
        List<string> lineList = ExternalFileSystem.SingleTon().GetTalkInfo(npcName);        
        string[] splitedLine = new string[10];        
        {   // NPC의 이름 추출
            string tag, value;
            string[] temp = lineList[0].Split(',');
            tag = temp[0].Trim();

            if (tag == "name")
            {
                value = temp[1].Trim();
                this.npcName = value;
            }
            else 
            {
                this.npcName = npcName;
            }
        }
        npcNameText.text = this.npcName;

        // 기타 분류 값.
        string mode;                                // 해당 줄이 담은 정보 구분.
        int id = 0;                                 // 해당 대화 단위의 번호.
        string text = "";                           // NPC의 대화 내용.
        bool isBranch = false;                      // 분기 노드 여부.
        Answer[] answer = new Answer[4];            // 선택지.
        for (int i = 0; i < 4; i++) answer[i] = new Answer();
        int answerCount = 0;                        // 현재 입력된 선택지 개수.
        List<string> message = new List<string>();  // 해당 시점에 실행될 이벤트 목록.
        // 대화 입력.
        foreach(string line in lineList)
        {
            // 디버그용.
            // Debug.Log("TalkView/loadTalkScript : line = " + line);
            // 문자열 분리.
            splitedLine = line.Split(',');
            // 존재하는 좌우 공백 제거.
            if (splitedLine.Length >= 2) for (int i = 0; i < splitedLine.Length; i++) splitedLine[i].Trim();
            // mode 체크.
            mode = splitedLine[0];
            // 아이디 기록.
            if (mode == "id") id = Convert.ToInt32(splitedLine[1]);
            // 브랜치 여부 입력.
            else if (mode == "branch") isBranch = true;
            // 대답 입력.
            else if (mode == "text") text = splitedLine[1];
            // 선택지 입력.
            else if (mode == "answer") {
                // 이미 선택지가 4개 기록된 경우 추가 기록을 차단함.
                if (answerCount == 4) continue;
                answer[answerCount].text = splitedLine[1];
                answer[answerCount].targetId = Convert.ToInt32(splitedLine[2]);
                answerCount += 1;
            }
            // 실행 함수 입력.
            else if (mode == "message") {
                string integratedMessageString = "";
                for (int i = 1; i < splitedLine.Length; i++) {
                    if (i != 1) integratedMessageString += ", ";
                    integratedMessageString += splitedLine[i];
                }
                message.Add(integratedMessageString);
            }
            // 대화 정보 입력 종료.            
            else if (mode == "end") {
                // 입력되지 않은 대화 디폴드 값으로 초기화.
                while (answerCount < 4) {
                    answer[answerCount].text = "";
                    answer[answerCount].targetId = NO_SELECTION;
                    answerCount += 1;
                }
                talkList.Add(new Talk(id, text, message, answer, isBranch));
                // 다음 대화 목록을 받기 위한 초기화
                id = Talk.NO_TALK;
                text = "";
                isBranch = false;
                answer = new Answer[4];
                for (int i = 0; i < 4; i++) answer[i] = new Answer();
                answerCount = 0;
                message = new List<string>();
            }
            else if (mode == "name") {
                // do nothing.
            }
            else { 
                Debug.Log($"TalkView.loadTalkScript.error : no match mode. input = {mode}.");
            }
        }
    }

    void setNpcTalk() {     // npc 의 텍스트를 현재 대화의 텍스트로 설정.
        npcTalkText.text = currentTalk.text;
    }

    void setPlayerAnswer() {    // 플레이어의 대화 선택지를 현재 대화의 선택지로 설정.
        for (int i = 0; i < 4; i++) {
            if (currentTalk.answers[i].text.Length != 0)
                playerAnswersText[i].text = (i + 1) + ". " + currentTalk.answers[i].text;
            else
                playerAnswersText[i].text = "";
        }
    }

    public void ChangeTalk(int targetId) {  // 선택에 따른 다음 분기로 대화 이동.
        // debug
        int currentId = this.currentTalk.id;
        Debug.Log($"TalkView.ChangeTalk.debug : currentId = {currentId}, targetId = {targetId}");
        // 대상 분기가 종료인 경우 대화 종료.
        if (targetId == TALK_END) closeTalk();        
        // 아닌 경우 talkList 를 순회하며 일치하는 타겟을 확인, 현재 대화를 그것으로 변경
        else foreach (Talk talk in talkList) {
            if (talk.id == targetId){
                // npc.eventCall(currentTalk.message[0], currentTalk.message[1]);
                currentTalk = talk;
                this.setNpcTalk();
                this.setPlayerAnswer();
                // 이동한 대화가 'branch'면 자동으로 처리.
                if (currentTalk.isBranch == true) passBranch();
                else if (currentTalk.message.Count != 0) eventCall();
                break;
            }                            
        }
    }

    // 브랜치 노트에서 자동으로 다음 노드로 건너가도록 해주는 함수.
    void passBranch() {
        int nextIndex = 0;

        // 선택 대상 판별. message 가 여러개라면 결과값의 합산으로 결정.
        // 
        foreach(string command in currentTalk.message) {
            Message checker = new Message(command).FunctionCall();
            try { nextIndex += (int) checker.returnValue[0]; }
            catch (System.Exception) { Debug.Log($"TalkView/passBranch.error : Next command action's return is not integer type. [{command}]"); }
        }

        // 선택지가 단 1개 뿐이면 'nextIndex' 값에 무관하게 0번 선택지로 이동.
        if (currentTalk.GetAnswerCount() == 1) {
            new Message($"TalkView/AnswerSelect : {nextIndex}").FunctionCall();
            return;
        }

        // 결과값이 0 ~ 3 범위 내가 아니라면 자동으로 넘기지 않음.
        if (!(nextIndex >= 0 && nextIndex < 4)) {
            Debug.Log($"TalkView/passBranch.error : Out of index. nextIndex = {nextIndex}");
            return;
        }

        // 그 외에는 대상 대화 노드로 바로 이동.
        new Message($"TalkUI/AnswerSelect : {nextIndex}").FunctionCall();
    }

    void eventCall() {        
        string cmd;
        foreach(string messageString in currentTalk.message) {
            Debug.Log("TalkView.eventCall.messageString : " + messageString);
            cmd = messageString;
            Message msg = new Message(cmd).FunctionCall();
        } 
    }
    
    void closeTalk() {
        talkList = null;
        background.SetActive(false);
        talkView.SetActive(false);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void AnswerSelect(Message message) {
        int index = (int) message.args[0];
        int targetId = currentTalk.answers[index].targetId;        
        Debug.Log($"TalkView/AnswerSelect.Notice : currentId = {currentTalk.id}, index = {index}, targetId = {targetId}");
        ChangeTalk(targetId);
    }
}

[System.Serializable]
public class Talk {
    public const int NO_TALK = -2;
    public const int NO_SELECTION = -1;
    public int id;
    public string text;
    public bool isBranch;   // 이것이 'true'면 대답을 기다리지 않고 자체적으로 Message 실행 결과를 이용해 분기.
    public List<string> message;    
    public Answer[] answers;

    public Talk() {
        this.id = NO_SELECTION;
        this.text = "";
        this.message = new List<string>();
        this.answers = new Answer[4];
        for (int i = 0 ; i < 4; i++) {
            this.answers[i] = new Answer();
        }
        this.isBranch = false;
    }
    public Talk(int id, string text, List<string> message, Answer[] answers, bool isBranch = false) {
        this.id = id;
        this.text = text;
        this.message = message;    
        this.answers = answers;
        this.isBranch = isBranch;
    }
    // 선택 가능한 'answer'의 개수 반환.
    public int GetAnswerCount() {
        int index = 0;
        foreach(Answer answer in answers) {
            if (answer.targetId != NO_SELECTION) index += 1;
        }
        return index;
    }
}

[System.Serializable]
public class Answer {
    public string text;
    public int targetId;

    public Answer(){
        this.text = "";
        this.targetId = 0;
    }
    public Answer(string text, int targetId) {
        this.text = text;
        this.targetId = targetId;
    }
}