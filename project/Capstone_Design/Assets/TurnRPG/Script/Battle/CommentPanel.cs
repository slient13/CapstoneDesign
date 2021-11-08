using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentPanel : MonoBehaviour
{
    public GameObject textPanel;
    public GameObject readyIndicator;
    public GameObject battleManagerObj;

    BattleManager battleManager;
    Text mainText;
    string textLine;
    bool _isTextReady = false;

    public float typingSpeed = 0.1f;

    //public string enemyName;
    //public string enemySkill;
    //public string skillName;
    //public string itemName;
    //public string[] commentLine;
    //int lineIndex;

    // Start is called before the first frame update
    void Start()
    {
        textPanel = transform.Find("MainText").gameObject;
        mainText = textPanel.GetComponent<Text>();
        battleManager = battleManagerObj.GetComponent<BattleManager>();
    }

    /// <summary>
    /// 텍스트 직접 설정
    /// </summary>
    /// <param name="value"></param>
    public void SetText(string value)
    {
        //mainText.text= value;
        StartCoroutine(Typing(value));
    }

    /// <summary>
    /// 적과 조우시
    /// </summary>
    /// <param name="monsterName"></param>
    public void Contact(string monsterName)
    {
        textLine = "앗! 야생의 " + monsterName + "이(가) 나타났다!";
        battleManager.SetCommentCode(0);
        StartCoroutine(Typing(textLine));
    }

    /// <summary>
    /// 대기중일시
    /// </summary>
    public void Waiting()
    {
        textLine = "무엇을 하지?";
        battleManager.SetCommentCode(1);
        StartCoroutine(Typing(textLine));
    }
    
    /// <summary>
    /// 아이템 사용시
    /// </summary>
    /// <param name="itemName"></param>
    public void ItemUse(string itemName)
    {
        textLine = itemName + "을(를) 사용하였다.";
        battleManager.SetCommentCode(2);
        StartCoroutine(Typing(textLine));
    }

    public void PlayerAttack(string skillName)
    {
        textLine = "플레이어의 " + skillName + "!";
        battleManager.SetCommentCode(3);
        StartCoroutine(Typing(textLine));
    }

    public void EnemyAttack(string MonsterName, string skillName)
    {
        textLine = MonsterName + "의 " + skillName + "공격!";
        battleManager.SetCommentCode(4);
        StartCoroutine(Typing(textLine));
    }

    public void PlayerFlee()
    {
        textLine = "플레이어는 달아났다!";
        battleManager.SetCommentCode(5);
        StartCoroutine(Typing(textLine));
    }

    public void PlayerLose()
    {
        textLine = "플레이어는 눈앞이 캄캄해졌다!";
        battleManager.SetCommentCode(6);
        StartCoroutine(Typing(textLine));
    }
    
    public void PlayerWin(string monsterName)
    {
        textLine = monsterName + "은(는) 쓰러졌다!";
        battleManager.SetCommentCode(7);
        StartCoroutine(Typing(textLine));
    }

    public void ItemGet(List<string> items)
    {
        string itemLine = "";
        int index = 0;

        foreach (string item in items)
        {
            itemLine += item;
            index++;

            if (index < items.Count)
                itemLine += ", ";
        }
        textLine = "플레이어는 " + itemLine + "을(를) 손에 넣었다!";
        battleManager.SetCommentCode(8);
        StartCoroutine(Typing(textLine));
    }

    public void AtkFail()
    {
        textLine = "공격에 실패했다..";
        battleManager.SetCommentCode(9);
        StartCoroutine(Typing(textLine));
    }

    public void AtkSuc()
    {
        textLine = "공격은 성공적이였다!";
        battleManager.SetCommentCode(10);
        StartCoroutine(Typing(textLine));
    }

    IEnumerator Typing(string text)
    {
        mainText.text = "";

        readyIndicator.SetActive(false);
        foreach (char letter in text.ToCharArray())
        {
            mainText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        readyIndicator.gameObject.SetActive(true);
    }


}
