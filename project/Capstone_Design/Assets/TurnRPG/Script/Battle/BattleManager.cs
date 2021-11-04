using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    //오브젝트계열
    public CommentPanel commentPanel;
    public CommandPanel commandPanel;
    public GameObject readyIndicator;
    public InfoManager infoManager;
    int commentCode;


    //몬스터 정보
    Enemy enemy;
    public string monsterName;
    public string monsterSkill;
    public Hp eHp;

    //플레이어 정보
    public string playerSkillName;
    public int playerStr;
    public int playerDef;
    public Hp pHp;
    float playerAttackChance;
    float playerAttackRate;


    // Start is called before the first frame update
    void Start()
    {
        //플레이어 셋팅
        playerDef = infoManager.GetDef();
        playerStr = infoManager.GetAtk();
        pHp.SetHpSize(infoManager.GetHp());

        //몬스터 셋팅
        enemy = infoManager.GetEnemyInfo("Bear");
        monsterName = enemy.name;
        eHp.SetHpSize(enemy.hp);

        //시작 트리거 테스트
        commentPanel.Contact(monsterName);
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어, 몬스터 HP 테스트
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            pHp.AddHp(-10);
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            eHp.AddHp(-10);
        }
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
                //적 조우
                commentPanel.Waiting();
                break;
            case 1:
                //명령대기
                commandPanel.SetActive(true);
                Debug.Log("커맨드패널 활성화");
                readyIndicator.SetActive(false);
                break;
            case 3:
                //플레이어가 적을때림
                if (RandomSucc(playerAttackChance))
                {
                    eHp.AddHp(-(PlayerAttackCal(playerAttackRate)));
                    commentPanel.AtkSuc();
                }
                else
                {
                    commentPanel.AtkFail();
                }
                break;
            case 5:
                //달아났을떄
                readyIndicator.SetActive(false);
                break;

            case 9:
                //공격에 실패했을때
                commentPanel.Waiting();
                break;
            case 10:
                //공격에 성공했을때
                commentPanel.Waiting();
                break;
            default:
                Debug.Log("코멘트코드 없음");
                break;
        }
    }

    public void InitPanelSelect(int index)
    {
        switch (index)
        {
            case 0:
                commandPanel.SetInitPanel(false);
                commandPanel.SetAttackPanel(true);
                    break;
            case 1:
                break;
            case 2:
                PlayerFlee();
                break;
            default:
                PlayerFlee();
                break;
        }
    }

    public void AttackPanelSelect(int index)
    {
        switch(index)
        {
            case 0:
                Debug.Log("베기 공격!");
                commandPanel.SetActive(false);
                playerAttackChance = 90f;
                playerAttackRate = 1.0f;
                playerSkillName = "베기";
                commentPanel.PlayerAttack(playerSkillName);
                break;
            case 1:
                Debug.Log("찌르기 공격!");
                commandPanel.SetActive(false);
                playerAttackChance = 50f;
                playerAttackRate = 2.0f;
                playerSkillName = "찌르기";
                commentPanel.PlayerAttack(playerSkillName);
                break;
            default:
                commandPanel.SetAttackPanel(false);
                commandPanel.SetInitPanel(true);
                break;
        }
    }

    float PlayerAttackCal(float atkRate)
    {
        float value = 0.0f;
        value = playerStr * atkRate;

        return value;
    }

    void PlayerDefCal(float def, float damage)
    {

    }

    bool RandomSucc(float chance)
    {
        if (Random.Range(1, 100) <= chance)
        {
            return true;
        }
        else
            return false;
    }

    void PlayerFlee()
    {
        commandPanel.SetActive(false);
        commentPanel.PlayerFlee();
    }
}
