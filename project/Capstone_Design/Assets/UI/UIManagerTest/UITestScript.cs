using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        new Message("newPlayInfo : hp, int, 80").functionCall();
        new Message("newPlayInfo : money, int, 10000").functionCall();

        MappingInfo mapping = new MappingInfo("TestObject");
        mapping.addMapping("hpChange : 10", "_a, arrowU");
        mapping.addMapping("hpChange : -10", "_a, arrowD");
        mapping.addMapping("moneyChange : 1000", "_s, arrowU");
        mapping.addMapping("moneyChange : -1000", "_s, arrowD");
        mapping.enroll();

        new Message("TestObject/hpChange : 10").functionCall();

        new Message("InventoryManager/addNewItem : heart, 하트, 툴팁, 효과").functionCall();
        new Message("InventoryManager/addNewItem : num1, 숫자1, 툴팁, 효과").functionCall();
        new Message("InventoryManager/addNewItem : num2, 숫자2, 툴팁, 효과").functionCall();
        new Message("InventoryManager/addNewItem : num3, 숫자3, 툴팁, 효과").functionCall();
        
        new Message("InventoryManager/modifyItem : heart, 1").functionCall();
        new Message("InventoryManager/modifyItem : num1, 2").functionCall();
        new Message("InventoryManager/modifyItem : num2, 3").functionCall();
        new Message("InventoryManager/modifyItem : num3, 4").functionCall();
        
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
}
