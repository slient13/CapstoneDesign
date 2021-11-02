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

        //코멘트라인 설정
        //commentLine[0] = "앗! 야생의" + enemyName + "이(가) 나타났다!";
        //commentLine[1] = "무엇을 하지?";
        //commentLine[2] = itemName + "을 사용하였다.";
        //commentLine[3] = skillName + "로 공격!";
        //commentLine[4] = enemyName + "의 " + enemySkill + "!";
    }

    /*
    /// <summary>
    /// 텍스트 라인 인덱스 선택
    /// </summary>
    public void SetCommentLine(int index)
    {
        mainText.text = commentLine[index];
        lineIndex = index;
    }

    public int GetCommentLineIndex()
    {
        return lineIndex;
    }

    /// <summary>
    /// 텍스트를 가져오기
    /// </summary>
    /// <param name="text"></param>
    void GetText(string text)
    {
        mainText.text = text;
    }

    /// <summary>
    /// 텍스트 업데이트
    /// </summary>
    void UpdateText()
    {
        Text originalText = textPanel.GetComponent<Text>();

        originalText.text = mainText.text;
    }
    */

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
