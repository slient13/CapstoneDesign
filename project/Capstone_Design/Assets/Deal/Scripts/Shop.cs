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
        // 사용법 간략화.
        new Message("InventoryManager/addNewItem : Health, 체 력, 체력이다, - ").functionCall();
        new Message("InventoryManager/addNewItem : Fish, 물 고 기, 평범한 물고기다, - ").functionCall();
        new Message("InventoryManager/addNewItem : Bug, 미 끼, 물고기를 잡기위한 미끼다, - ").functionCall();

        // 효과 없는 불필요한 코드
        // Message msg4 = new Message("InventoryManager/modifyItem: Health, 0 ");
        // Message msg5 = new Message("InventoryManager/modifyItem: Fish, 0 ");
        // Message msg6 = new Message("InventoryManager/modifyItem: Bug, 0 ");

        // msg4.functionCall();
        // msg5.functionCall();
        // msg6.functionCall();

        MappingInfo map1 = new MappingInfo("Zone");
        map1.addMapping("Tem :", "o");
        map1.enroll();
    }
   
    /*
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
    */

    public void Interaction(Player player) {
        Enter(player);
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

    /*
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
    */

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
