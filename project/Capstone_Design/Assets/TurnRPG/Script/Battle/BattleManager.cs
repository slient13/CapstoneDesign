using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    //몬스터 정보
    public string monsterName;
    public string monsterSkill;

    //플레이어 정보
    public string playerSkill;

    public GameObject commentPanelObj;
    CommentPanel commentPanel;
    int commentCode;

    // Start is called before the first frame update
    void Start()
    {
        commentPanel = commentPanelObj.GetComponent<CommentPanel>();
        commentPanel.Contact(monsterName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 코멘트의 id값을 설정한다.
    /// </summary>
    /// <param name="value"></param>
    public void SetCommentCode(int value)
    {
        commentCode = value;
    }

    /// <summary>
    /// 코멘트 패널에서 다음 텍스트로 넘어갈때
    /// </summary>
    public void NextState()
    {
        switch(commentCode)
        {
            case 0:
                commentPanel.Waiting();
                break;
            case 1:

                break;
            default:
                Debug.LogWarning("없는 코멘트코드");
                break;
        }
    }
}
