using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfoManager : MonoBehaviour
{
    void Start()
    {
        new Message("NewPlayInfo : hp, int, 100").FunctionCall();
        new Message("NewPlayInfo : money, int, 10000").FunctionCall();
        new Message("NewPlayInfo : tired, int, 100").FunctionCall();
        new Message("NewPlayInfo : attack, int, 100").FunctionCall();
        new Message("NewPlayInfo : defense, int, 100").FunctionCall();


        MappingInfo mapping = new MappingInfo("PlayInfoManager");
        mapping.AddMapping("ChangeHp : 10", "_ctrlL, arrowU");
        mapping.AddMapping("ChangeHp : -10", "_ctrlL, arrowD");
        mapping.AddMapping("ChangeMoney : 1000", "_altL, arrowU");
        mapping.AddMapping("ChangeMoney : -1000", "_altL, arrowD");
        mapping.Enroll();        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeHp(Message message) {
        int degree = (int) message.args[0];
        
        Message msg = new Message("GetPlayInfo : hp");
        msg.FunctionCall();
        int beforeHp = (int) msg.returnValue[0];

        int afterHp = beforeHp + degree;
        if (afterHp < 0) afterHp = 0;
        else if (afterHp > 100) afterHp = 100;
        new Message("PlayInfoSetter : hp, " + afterHp).FunctionCall();
    }

    public void ChangeMoney(Message message) {
        int degree = (int) message.args[0];
        
        Message msg = new Message("GetPlayInfo : money");
        msg.FunctionCall();
        int beforeMoney = (int) msg.returnValue[0];

        int afterMoney = beforeMoney + degree;
        if (afterMoney < 0) afterMoney = 0;
        else if (afterMoney > 1000000) afterMoney = 1000000;
        new Message("PlayInfoSetter : money, "+ afterMoney).FunctionCall();
    }

    public void GetHp(Message message) {
        Message getValue = new Message("GetPlayInfo : hp").FunctionCall();
        int hp = (int) getValue.returnValue[0];
        message.returnValue.Add(hp);
    }
    public void GetMoney(Message message) {
        Message getValue = new Message("GetPlayInfo : money").FunctionCall();
        int money = (int) getValue.returnValue[0];
        message.returnValue.Add(money);
    }
}
