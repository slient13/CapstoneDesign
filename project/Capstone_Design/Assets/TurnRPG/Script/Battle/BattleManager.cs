﻿using System.Collections;
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
    public EnemyPanel enemyPanel;
    int commentCode;
    float MAXDEFENSE = 100f;

    //몬스터 정보
    Enemy enemy;
    Enemy.Skill enemySkill;
    public string monsterName;
    public Hp eHp;
    public bool isMonsterTurn;
    List<string> itemDrop;
    

    //플레이어 정보
    public string playerSkillName;
    public Player battlePlayer;
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
        enemy = infoManager.GetEnemyInfo(infoManager.getSceneStartValue());
        enemyPanel.SetEnemy(enemy);
        monsterName = enemy.name;
        eHp.SetHpSize(enemy.hp);
        isMonsterTurn = false;

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
                commandPanel.SetInitPanel(true);
                commandPanel.SetAttackPanel(false);
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
                isMonsterTurn = true;
                break;
            case 4:
                //적이 플레이어를 때림
                if (RandomSucc(infoManager.GetEvasion()))
                {
                    commentPanel.AtkFail();
                }
                else
                {
                    PlayerDefCal();
                    commentPanel.AtkSuc();
                }
                isMonsterTurn = false;
                break;
            case 5:
                //달아났을때
                //여기에 게임종료코드
                readyIndicator.SetActive(false);
                break;
            case 6:
                //플레이어가 패배
                commentPanel.PlayerFlee();
                break;
            case 7:
                //플레이어가 승리
                commentPanel.ItemGet(itemDrop);
                break;
            case 8:
                //전투가 끝나고 아이템 얻었을때
                //여기에 게임종료 코드
                readyIndicator.SetActive(false);
                break;
            case 9:
                //공격에 실패했을때
                if (isMonsterTurn)
                    EnemyAttack();
                else
                    commentPanel.Waiting();
                break;
            case 10:
                //공격에 성공했을때
                if (!MatchCheck())
                {
                    if (isMonsterTurn)
                        EnemyAttack();
                    else
                        commentPanel.Waiting();
                    break;
                }
                else
                    break;
            default:
                Debug.Log("코멘트코드 없음");
                commentPanel.Waiting();
                break;
        }
    }

    /// <summary>
    /// 커맨드패널 메인메뉴
    /// </summary>
    /// <param name="index"></param>
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

    /// <summary>
    /// 커맨드패널 공격메뉴
    /// </summary>
    /// <param name="index"></param>
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

    /// <summary>
    /// 적이 플레이어를 공격하는 알고리즘
    /// </summary>
    void EnemyAttack()
    {
        enemySkill = enemy.GetRandomSkill();
        commentPanel.EnemyAttack(enemy.name, enemySkill.name);
    }

    /// <summary>
    /// 플레이어가 받는 데미지 계산식
    /// </summary>
    /// <param name="def"></param>
    /// <param name="damage"></param>
    void PlayerDefCal()
    {
        float damage = (float)enemySkill.effect * enemy.attack;
        pHp.AddHp(-1f * (damage - damage/((MAXDEFENSE - playerDef)/10)));
    }

    /// <summary>
    /// 플레이어가 주는 데미지 계산식
    /// </summary>
    /// <param name="atkRate"></param>
    /// <returns></returns>
    float PlayerAttackCal(float atkRate)
    {
        float value = 0.0f;
        value = playerStr * atkRate;

        return value;
    }


    /// <summary>
    /// 확률 설정
    /// </summary>
    /// <param name="chance"></param>
    /// <returns></returns>
    bool RandomSucc(float chance)
    {
        if (Random.Range(1, 100) <= chance)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 플레이어가 달아남
    /// </summary>
    void PlayerFlee()
    {
        commandPanel.SetActive(false);
        commentPanel.PlayerFlee();
    }

    /// <summary>
    /// 승부 체크
    /// </summary>
    bool MatchCheck()
    {
        bool isOver = false;
        if (pHp.hpAmount <= 0)
        {
            //플레이어 패배
            isOver = true;
            commentPanel.PlayerLose();
        }
        else if (eHp.hpAmount <= 0)
        {
            //플레이어 승리
            isOver = true;
            itemDrop = enemy.GetDrops();
            commentPanel.PlayerWin(enemy.name);
        }
        else
            isOver = false;

        return isOver;
    }
}
