using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LUIManager : MonoBehaviour
{
    public RectTransform bar;
    public Text moneyText;
    public Text timeText;
    public GameObject talkView;
    public int hp;
    private float convertScale;
    public int money;
    public int[] timeSet;
    public float time;
    public int gameTime;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        hp = 100;
        convertScale = 2.5f;
        money = 0;
        timeSet = new int[3];
        timeSet[0] = 0;    // 일
        timeSet[1] = 0;    // 시
        timeSet[2] = 0;    // 분
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        gameTime = (int)time * 60;

        timeSet[0] = gameTime / 3600 / 24;
        timeSet[1] = gameTime / 3600 % 24;
        timeSet[2] = gameTime / 60 % 60;
        // 체력바 업데이트
        bar.anchoredPosition = new Vector3(hp * convertScale /2, 0, 0);
        bar.sizeDelta = new Vector2 (hp * convertScale, 50);

        // 돈 텍스트 업데이트
        moneyText.text = money + " 원";

        // 시간 텍스트 업데이트
        timeText.text = 
            timeSet[0] + "일 "
            + timeSet[1] + "시 "
            + timeSet[2] + "분";
        
       //if (money >= 99999999) GameOver();
    }

    public void StartTalk(string npcName) {
        Debug.Log("매니저 받았쩡");
        talkView.SendMessage("StartTalk", npcName);
    }
    public void hpChange(int val){
        hp += val;
        if (hp > 100) hp = 100;
        else if (hp < 0) hp = 0;
    }

    public void moneyChange(int val) {
        money += val;
    }

    /*
    public void GameOver() {
        int[] clearTime = new int[3];

        Invoke("ChangeScene", 10);
    }
    */

    void ChangeScene() {
        SceneManager.LoadScene("MainPage");
    }
}
