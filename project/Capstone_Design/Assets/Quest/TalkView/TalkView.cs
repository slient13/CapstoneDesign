using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TalkView : MonoBehaviour
{
    const int NO_SELECTION = -1;
    const int TALK_END = -2;
    private string npcName;
    public List<Talk> talkList;
    public Talk currentTalk;

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
        mapping.addMapping("answerSelect_1 : ", "n1");
        mapping.addMapping("answerSelect_2 : ", "n2");
        mapping.addMapping("answerSelect_3 : ", "n3");
        mapping.addMapping("answerSelect_4 : ", "n4");
        mapping.enroll("talkView");

        // 테스트용 시작 코드
        // MappingInfo startMap = new MappingInfo(talkUIName);
        // startMap.addMapping("StartTalkByKey : npc", "space");
        // startMap.enroll();


        background.SetActive(false);
        talkView.SetActive(false);
        
    }

    void Update() {
    }

    public void StartTalkByKey(Message message) {
        new Message("ControlManager/layerChanger : talkView").functionCall();        
        string name = (string)message.args[0];  // 대화 대상 이름
        int startId;
        if (message.args.Count == 1) startId = 0;
        else startId = (int) message.args[1];
        StartTalk(name, startId);
    }

    public void StartTalk(string npcName, int startId) {
        // Debug.Log("토크뷰 받았쩡");
        background.SetActive(true);
        talkView.SetActive(true);
        this.npcName = npcName;
        string filename = "Talk/" + npcName + "TalkScript";
        npcNameText.text = npcName;
        loadTalkScript(npcName, filename);
        changeTalk(startId);
    }

    void loadTalkScript(string npcName, string fileName) {
        this.npcName = npcName;
        talkList = new List<Talk>();

        TextAsset data = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);
        Talk talk = new Talk();
        int answerNum = 0;
        
        string line;
        string[] splitedLine = new string[3];        
        line = sr.ReadLine();
        string mode;
        string value;
        string exVal = "";
        while(line != null)
        {
            splitedLine = line.Split(',');
            mode = splitedLine[0];
            value = splitedLine[1];
            if (mode == "message" || mode == "answer") 
                exVal = splitedLine[2];

            if (mode == "id") talk.id = int.Parse(value);
            else if (mode == "text") talk.text = value;
            else if (mode == "message") {
                talk.message[0] = value;
                talk.message[1] = exVal;
            }
            else if (mode == "answer") {
                if (answerNum >= 4) continue;
                talk.answers[answerNum].text = value;
                talk.answers[answerNum].targetId = int.Parse(exVal);
                answerNum += 1;
            }
            else if (mode == "") {
                for (int i = answerNum; i < 4; i++) {
                    talk.answers[answerNum].text = "";
                    talk.answers[answerNum].targetId = NO_SELECTION;
                    answerNum += 1;
                }
                answerNum = 0;
                talkList.Add(talk);
                talk = new Talk();
            }
            line = sr.ReadLine();
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

    public void changeTalk(int targetId) {  // 선택에 따른 다음 분기로 대화 이동.
        // 대상 분기가 종료인 경우 대화 종료.
        if (targetId == TALK_END) closeTalk();
        // 아닌 경우 talkList 를 순회하며 일치하는 타겟을 확인, 현재 대화를 그것으로 변경
        else foreach (Talk talk in talkList) {
            if (talk.id == targetId){
                // npc.sendMessage(currentTalk.message[0], currentTalk.message[1]);
                currentTalk = talk;
                this.setNpcTalk();
                this.setPlayerAnswer();
                // Debug.Log(currentTalk.answers[0].targetId);
                // Debug.Log(currentTalk.answers[1].targetId);
                // Debug.Log(currentTalk.answers[2].targetId);
                // Debug.Log(currentTalk.answers[3].targetId);
                if (currentTalk.message[0] != null) sendMessage();
                break;
            }                            
        }
    }

    void sendMessage() {
        string functionName = currentTalk.message[0];
        string arg = currentTalk.message[1];
        Message msg = new Message(functionName + ":" + arg);
        msg.targetName = "QuestManager";
        msg.functionCall();
        Debug.Log("TalkView.sendMessage.msg.getCommand() = " + msg.getCommand());
    }
    
    void closeTalk() {
        talkList = null;
        background.SetActive(false);
        talkView.SetActive(false);
        new Message("ControlManager/layerChanger : general").functionCall();
    }

    public void answerSelect_1() {
        changeTalk(currentTalk.answers[0].targetId);
        // Debug.Log("clicked : answerSelect_1");
    }
    public void answerSelect_2() {
        changeTalk(currentTalk.answers[1].targetId);
        // Debug.Log("clicked : answerSelect_2");
    }
    public void answerSelect_3() {
        changeTalk(currentTalk.answers[2].targetId);
        // Debug.Log("clicked : answerSelect_3");
    }
    public void answerSelect_4() {
        changeTalk(currentTalk.answers[3].targetId);
        // Debug.Log("clicked : answerSelect_4");
    }
}

[System.Serializable]
public class Talk {
    const int NO_SELECTION = -1;
    public int id;
    public string text;
    public string[] message;    // 첫번째는 함수명, 두번째는 인수.
    public Answer[] answers;

    public Talk() {
        this.id = NO_SELECTION;
        this.text = "";
        this.message = new string[2];
        this.answers = new Answer[4];
        for (int i = 0 ; i < 4; i++) {
            this.answers[i] = new Answer();
        }
    }
    public Talk(int id, string text, string[] message, Answer[] answers) {
        this.id = id;
        this.text = text;
        this.message = message;    
        this.answers = answers;
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