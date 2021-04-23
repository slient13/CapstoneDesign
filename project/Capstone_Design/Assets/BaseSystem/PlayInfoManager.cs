using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfoManager : MonoBehaviour
{
    void Start()
    {
        new Message("newPlayInfo : hp, int, 100").functionCall();
        new Message("newPlayInfo : money, int, 10000").functionCall();

        MappingInfo mapping = new MappingInfo("PlayInfoManager");
        mapping.addMapping("hpChange : 10", "_ctrlL, arrowU");
        mapping.addMapping("hpChange : -10", "_ctrlL, arrowD");
        mapping.addMapping("moneyChange : 1000", "_altL, arrowU");
        mapping.addMapping("moneyChange : -1000", "_altL, arrowD");
        mapping.enroll();        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void hpChange(Message message) {
        int degree = (int) message.args[0];
        
        Message msg = new Message("getPlayInfo : hp");
        msg.functionCall();
        int beforeHp = (int) msg.returnValue[0];

        int afterHp = beforeHp + degree;
        if (afterHp < 0) afterHp = 0;
        else if (afterHp > 100) afterHp = 100;
        new Message("playInfoSetter : hp, " + afterHp).functionCall();
    }

    public void moneyChange(Message message) {
        int degree = (int) message.args[0];
        
        Message msg = new Message("getPlayInfo : money");
        msg.functionCall();
        int beforeMoney = (int) msg.returnValue[0];

        int afterMoney = beforeMoney + degree;
        if (afterMoney < 0) afterMoney = 0;
        else if (afterMoney > 1000000) afterMoney = 1000000;
        new Message("playInfoSetter : money, "+ afterMoney).functionCall();
    }

    public void getHp(Message message) {
        Message getValue = new Message("getPlayInfo : hp").functionCall();
        int hp = (int) getValue.returnValue[0];
        message.returnValue.Add(hp);
    }
    public void getMoney(Message message) {
        Message getValue = new Message("getPlayInfo : money").functionCall();
        int money = (int) getValue.returnValue[0];
        message.returnValue.Add(money);
    }
}
