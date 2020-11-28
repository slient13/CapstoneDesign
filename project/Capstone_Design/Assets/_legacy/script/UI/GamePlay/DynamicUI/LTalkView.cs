using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LTalkView : MonoBehaviour
{
    public GameObject ui;
    const int NO_SELECTION = -1;
    const int TALK_END = -2;
    private string npcName;
    private List<LTalk> talks;
    private LTalk currentTalk;

    public Text npcNameText;
    public Text npcTalkText;
    public Text[] playerAnswersText;

    public GameObject talkview;

    void Start() {
        // startTalk("test");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) answerSelect_1();
        if (Input.GetKeyDown(KeyCode.Alpha2)) answerSelect_2();
        if (Input.GetKeyDown(KeyCode.Alpha3)) answerSelect_3();
        if (Input.GetKeyDown(KeyCode.Alpha4)) answerSelect_4();
    }

    public void StartTalk(string npcName) {
        Debug.Log("토크뷰 받았쩡");
        talkview.gameObject.SetActive(true);
        this.npcName = npcName;
        string filename = npcName + "TalkScript";
        loadTalkScript(npcName, filename);
    }

    void loadTalkScript(string npcName, string fileName) {
        this.npcName = npcName;
        talks = new List<LTalk>();

        TextAsset data = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);
        LTalk talk = new LTalk();
        int answerNum = 0;
        
        string line;
        string[] splitedLine = new string[3];        
        line = sr.ReadLine();
        while(line != null)
        {
            splitedLine = line.Split(',');

            if (splitedLine[0] == "id") talk.id = int.Parse(splitedLine[1]);
            else if (splitedLine[0] == "text") talk.text = splitedLine[1];
            else if (splitedLine[0] == "message") {
                talk.message[0] = splitedLine[1];
                talk.message[1] = splitedLine[2];
            }
            else if (splitedLine[0] == "answer") {
                if (answerNum >= 4) continue;
                talk.answers[answerNum].text = splitedLine[1];
                talk.answers[answerNum].targetId = int.Parse(splitedLine[2]);
                answerNum += 1;
            }
            else if (splitedLine[0] == "") {
                for (int i = answerNum; i < 4; i++) {
                    talk.answers[answerNum].text = "";
                    talk.answers[answerNum].targetId = NO_SELECTION;
                    answerNum += 1;
                }
                answerNum = 0;
                talks.Add(talk);
                talk = new LTalk();
            }
            line = sr.ReadLine();
        }

        changeTalk(0);
    }

    void setNpcTalk() {
        npcTalkText.text = currentTalk.text;
    }

    void setPlayerAnswer() {
        for (int i = 0; i < 4; i++) {
            playerAnswersText[i].text = currentTalk.answers[i].text;            
        }
    }

    public void changeTalk(int targetId) {
        if (targetId == TALK_END) closeTalk();
        else foreach (LTalk talk in talks)
        {
            if (talk.id == targetId){
                // npc.sendMessage(currentTalk.message[0], currentTalk.message[1]);
                currentTalk = talk;
                this.setNpcTalk();
                this.setPlayerAnswer();
                // Debug.Log(currentTalk.answers[0].targetId);
                // Debug.Log(currentTalk.answers[1].targetId);
                // Debug.Log(currentTalk.answers[2].targetId);
                // Debug.Log(currentTalk.answers[3].targetId);
                if (currentTalk.message[0] != "") sendMessage();
                break;
            }                            
        }
    }

    void sendMessage() {        
        int intParameter;
        if (int.TryParse(currentTalk.message[1], out intParameter) == true) {
            this.ui.SendMessage(
                currentTalk.message[0],
                intParameter
                );
        }
        else this.ui.SendMessage(
            currentTalk.message[0],
            currentTalk.message[1]
            );
    }
    
    void closeTalk() {
        talks = null;
        talkview.gameObject.SetActive(false);
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

class LTalk {
    const int NO_SELECTION = -1;
    public int id;
    public string text;
    public string[] message;    // 첫번째는 함수명, 두번째는 인수.
    public LAnswer[] answers;

    public LTalk() {
        this.id = NO_SELECTION;
        this.text = "";
        this.message = new string[2];
        this.answers = new LAnswer[4];
        for (int i = 0 ; i < 4; i++) {
            this.answers[i] = new LAnswer();
        }
    }
    public LTalk(int id, string text, string[] message, LAnswer[] answers) {
        this.id = id;
        this.text = text;
        this.message = message;    
        this.answers = answers;
    }
}

class LAnswer {
    public string text;
    public int targetId;

    public LAnswer(){
        this.text = "";
        this.targetId = 0;
    }
    public LAnswer(string text, int targetId) {
        this.text = text;
        this.targetId = targetId;
    }
}