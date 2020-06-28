using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public RectTransform bar;
    public Text moneyText;
    public Text timeText;
    public int hp;
    private float convertScale;
    public int money;
    public int[] timeSet;
    public float time;
    public int gameTime;


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
        timeSet[1] = gameTime / 3600;
        timeSet[2] = gameTime / 60;
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
        
        if (money >= 99999) GameOver();
    }

    public void hpChange(int val){
        hp += val;
    }

    public void moneyChange(int val) {
        money += val;
    }

    public void GameOver() {
        int[] clearTime = new int[3];

        Invoke("ChangeScene", 10);
    }

    void ChangeScene() {
        SceneManager.LoadScene("MainPage");
    }
}
