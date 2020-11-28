using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{   
    GameObject hpBar;   // 체력바
    GameObject timer;   // 경과 시간
    GameObject money;   // 소지한 돈.

    const int HP_BAR_WEIGHT = 250;
    public int MAX_HP;
    int START_HP;

    // Start is called before the first frame update
    void Start()
    {
        MAX_HP = 100;
        START_HP = MAX_HP / 10;
        hpBar = transform.Find("hpBar").GetChild(0).gameObject;
        money = transform.Find("money").GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Message msgHp = new Message("getPlayInfo : hp");
        msgHp.functionCall();
        setHpBar((int) msgHp.returnValue[0]);

        Message msgMoney = new Message("getPlayInfo : money");
        msgMoney.functionCall();
        setMoney((int) msgMoney.returnValue[0]);
    }

    void setHpBar(int degree) {       
        // 가로 길이 변경 
        RectTransform rectTran = hpBar.GetComponent<RectTransform>();
        float width = degree * HP_BAR_WEIGHT / MAX_HP;
        rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        hpBar.transform.localPosition = new Vector3(width / 2 - 125, 0, 0);
    }    
    void setMoney(int degree) {
        money.GetComponent<Text>().text = degree + " 원";
    }
}
