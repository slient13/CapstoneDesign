using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup1;
    public RectTransform uiGroup2;
    public Animator anim;

    public Image[] itemObj; //각 아이템 프리펩
    public int[] itemPrice; // 각 아이템 가격
    public Transform[] itemPos; // 각 아이템 위치
    public string[] talkData;
    public Text talkText; // 금액 부족을 알리기 위한 대사 

    Player enterPlayer;



     void Start()
    {
        Message msg1 = new Message("InventoryManager/addNewItem : health, 체 력, 가, 가 ");
        Message msg2 = new Message("InventoryManager/addNewItem : fish, 물 고 기, 가, 가 ");
        Message msg3 = new Message("InventoryManager/addNewItem : bug, 미 끼, 가, 가 ");

        msg1.functionCall();
        msg2.functionCall();
        msg3.functionCall();

        msg1 = new Message("InventoryManager/modifyItem: health, 0 ");
        msg2 = new Message("InventoryManager/modifyItem: fish, 0 ");
        msg3 = new Message("InventoryManager/modifyItem: bug, 0 ");

        msg1.functionCall();
        msg2.functionCall();
        msg3.functionCall();

        MappingInfo map1 = new MappingInfo("Zone");
        map1.addMapping("Tem :", "o");
        map1.enroll();
    }
   
    public void InventoryChange()
    {
        if (this.tag == "Fish")
        {
            Message msg6 = new Message("InventoryManager/modifyItem : fish, 1");
            msg6.functionCall();
            Message msg7 = new Message("playInfoChanger: coin, -700");
            msg7.functionCall();
        }
        else if (this.tag == "Bug")
        {
            Message msg8 = new Message("InventoryManager/modifyItem : bug, 1");
            msg8.functionCall();
            Message msg9 = new Message("playInfoChanger: coin, -100");
            msg9.functionCall();
        }

    }

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup1.anchoredPosition = Vector3.up * 250;
        uiGroup2.anchoredPosition = Vector3.up * -250;

    }

    public void Tem()
    {
        Debug.Log("취소");
        anim.SetTrigger("doHello");
        uiGroup1.anchoredPosition = Vector3.down * 1000;
        uiGroup2.anchoredPosition = Vector3.down * 1500;
    }

    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup1.anchoredPosition = Vector3.down * 1000;
        uiGroup2.anchoredPosition = Vector3.down * 1500;

    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if(price > enterPlayer.coin)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
