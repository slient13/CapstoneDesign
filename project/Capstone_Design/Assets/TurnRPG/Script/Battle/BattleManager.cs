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
    public EnemyPanel enemyPanel;
    public BattleAudioPack audioPack;
    public EnemyImage enemyImage;
    public ItemSelect itemSelect;
    int commentCode;
    InfoManager infoManager = new InfoManager();
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
    public Hp pHp;
    public int playerStr;
    public int playerDef;
    public float playerMaxHp;
    float playerAttackChance;
    float playerAttackRate;
    float damageHistory;
    bool isAnimPlaying = false;
    bool isSoundPlaying = false;


    // Start is called before the first frame update
    void Start()
    {
        //플레이어 셋팅
        playerDef = infoManager.GetDef();
        playerStr = infoManager.GetAtk();
        pHp.SetHpSize(infoManager.GetHp(), playerMaxHp);
        damageHistory = 0f;

        //몬스터 셋팅
        enemy = infoManager.GetEnemyInfo(infoManager.GetSceneStartValue());
        enemyPanel.SetEnemy(enemy);
        monsterName = enemy.name;
        eHp.SetHpSize(enemy.hp, enemy.hp);
        isMonsterTurn = false;

        //게임 셋팅
        readyIndicator.gameObject.SetActive(false);

        //시작 트리거 테스트
        //commentPanel.Contact(monsterName);
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

        //애니메이션, 효과음 실행 여부 확인
        isAnimPlaying = enemyImage.IsPlaying();
        isSoundPlaying = audioPack.IsPlaying();
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
        audioPack.PlayNextText();

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
               //Debug.Log("커맨드패널 활성화");
                readyIndicator.SetActive(false);
                break;
            case 2:
                isMonsterTurn = true;

                if (isMonsterTurn)
                    EnemyAttack();
                else
                    commentPanel.Waiting();
                break;
            case 3:
                //플레이어가 적을때림
                if (RandomSucc(playerAttackChance))
                {
                    eHp.AddHp(-(PlayerAttackCal(playerAttackRate)));
                    //애니메이션, 사운드 재생
                    enemyImage.PlayHitAnim();
                    audioPack.PlayEnemyHit();

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

                //애니메이션 재생
                enemyImage.PlayAttackAnim();

                if (RandomSucc(infoManager.GetEvasion()))
                {
                    commentPanel.AtkFail();
                }
                else
                {
                    //소리 재생
                    audioPack.PlayPlayerHit();

                    PlayerDefCal();
                    commentPanel.AtkSuc();
                }
                isMonsterTurn = false;
                break;
            case 5:
                //달아났을때
                readyIndicator.SetActive(false);
                //여기에 게임종료코드
                EndGame("Lose");
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
                readyIndicator.SetActive(false);
                //여기에 게임종료 코드
                EndGame("Win");
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
                //Debug.Log("코멘트코드 없음");
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
                audioPack.PlayConfirmSound();
                commandPanel.SetInitPanel(false);
                commandPanel.SetAttackPanel(true);
                    break;
            case 1:
                //audioPack.PlayErrorSound();
                audioPack.PlayConfirmSound();
                itemSelect.StartSelect();
                commandPanel.SetActive(false);
                break;
            case 2:
                audioPack.PlayConfirmSound();
                PlayerFlee();
                break;
            default:
                audioPack.PlayConfirmSound();
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
                //Debug.Log("베기 공격!");
                audioPack.PlayConfirmSound();
                commandPanel.SetActive(false);
                playerAttackChance = 90f;
                playerAttackRate = 1.0f;
                playerSkillName = "베기";
                commentPanel.PlayerAttack(playerSkillName);
                break;
            case 1:
                //Debug.Log("찌르기 공격!");
                audioPack.PlayConfirmSound();
                commandPanel.SetActive(false);
                playerAttackChance = 50f;
                playerAttackRate = 2.0f;
                playerSkillName = "찌르기";
                commentPanel.PlayerAttack(playerSkillName);
                break;
            default:
                audioPack.PlayConfirmSound();
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
        AddPlayerDamage(-1f * (damage - damage / ((MAXDEFENSE - playerDef) / 10)));
        //damageHistory += -1f * (damage - damage / ((MAXDEFENSE - playerDef) / 10));
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

            //효과음, 애니메이션 재생
            enemyImage.PlayDeadAnim();
            audioPack.PlayDeadSound();
        }
        else
            isOver = false;

        return isOver;
    }

    /// <summary>
    /// 플레이어 HP 더하기
    /// </summary>
    public void AddPlayerHp(float value)
    {
        pHp.AddHp(value);
        AddPlayerDamage(-1f * value);
    }

    /// <summary>
    /// 플레이어에게 가해진 데미지 더하기
    /// </summary>
    /// <param name="value"></param>
    void AddPlayerDamage(float value)
    {
        damageHistory += value;
        if (damageHistory < 0f)
            damageHistory = 0f;
        else if (damageHistory > playerMaxHp)
            damageHistory = 100f;
    }

    /// <summary>
    /// 커맨드패널 사용설정
    /// </summary>
    /// <param name="value"></param>
    public void SetCommandPanel(bool value)
    {
        commandPanel.SetActive(value);
    }

    /// <summary>
    /// 게임시작
    /// </summary>
    public void StartGame()
    {
        commentPanel.Contact(monsterName);
    }
    
    /// <summary>
    /// 게임종료
    /// </summary>
    void EndGame(string value)
    {
        infoManager.ChangeHp((int)damageHistory);
       //Debug.Log("플레이어의 남은 체력은 : " + pHp.hpAmount);
        Message msg = new Message("GameProcessManager/ChangeScene : HuntingField, " + value).FunctionCall();
    }
}
