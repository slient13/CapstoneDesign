using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentPanel : MonoBehaviour
{
    public GameObject textPanel;
    public GameObject readyIndicator;
    public EnemyInfo enemy;
    public string enemyName;
    public string enemySkill;
    public string itemName;
    public string[] commentLine;

    Text mainText;
    int lineIndex;
    bool _isTextReady = false;

    // Start is called before the first frame update
    void Start()
    {
        textPanel = transform.Find("MainText").gameObject;
        readyIndicator = transform.Find("ReadyIndicator").gameObject;

        //코멘트라인 설정
        commentLine[0] = "앗! 야생의" + enemyName + "이(가) 나타났다!";
        commentLine[1] = "무엇을 하지?";
        commentLine[2] = itemName + "을 사용하였다.";
    }

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

        /*
        mainText.text = originalText.text;
        mainText.color = originalText.color;
        mainText.fontSize = originalText.fontSize;
        */
    }

    /// <summary>
    /// 활성화
    /// </summary>
    void Activate()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 비활성화
    /// </summary>
    void DeActivate()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 활성화 설정
    /// </summary>
    /// <param name="boolean"></param>
    void SetActivate(bool boolean)
    {
        this.gameObject.SetActive(boolean);
    }
}
