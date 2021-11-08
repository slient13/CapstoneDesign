using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcCommandManager : MonoBehaviour
{
    public GameObject npc;
    public float respawnTime;
    public GameProcessManager gpManager;
    public string monsterName;
    public GameObject player;
    public FieldManager fieldManager;
    InfoManager infoManager = new InfoManager();


    //플레이어 정보를 전달하는 매니저
    public GameObject systemManager;

    bool isLive = true;
    float timer;
    AudioSource hitSound;
    GameObject deadEffect;
    static string matchedNpcCode;
    static Vector3 lastEnemyPosition;

    private void Start()
    {
        fieldManager = GameObject.Find("FieldManager").GetComponent<FieldManager>();
        hitSound = transform.Find("HitSound").GetComponent<AudioSource>();
        deadEffect = transform.Find("ParticleDead").gameObject;
        gpManager = GameObject.Find("GameProcessManager").GetComponent<GameProcessManager>();
    }

    private void Update()
    {
        if (!isLive)
        {
            timer += Time.deltaTime;
            if (timer > respawnTime)
            {
                Respawn();
                timer = 0f;
            }
        }
    }

    void ChaseTarget(GameObject target)
    {
        if(target != null)
        {
            npc.GetComponent<RandomMover>().SendMessage("SetEnable", false);
            npc.GetComponent<RandomMover>().SendMessage("MoveToTarget", target);
        }
    }

    void InitPos()
    {
        npc.GetComponent<RandomMover>().SendMessage("SetReturning", true);
    }

    void CancelReturning()
    {
        npc.GetComponent<RandomMover>().SendMessage("SetReturning", false);
    }

    /// <summary>
    /// 리스폰
    /// </summary>
    void Respawn()
    {
        npc.GetComponent<BattleNpc>().Respawn();
        //npc.GetComponent<BattleNpc>().SendMessage("Respawn");

        isLive = true;
    }

    /// <summary>
    /// 전투 시작
    /// </summary>
    /// <param name="boolean"></param>
    void BattleStart(bool boolean)
    {
        infoManager.SavePlayerPos();
        matchedNpcCode = gameObject.transform.parent.name;
        lastEnemyPosition = npc.transform.position;
        fieldManager.SetMatchedEnemy(matchedNpcCode);
        //Debug.Log(matchedNpcCode + "의 위치" + lastEnemyPosition);
        //Debug.Log("싸움을 시작한 몬스터의 코드 : " + matchedNpcCode);
        //Debug.Log("플레이어의 위치저장:" + infoManager.GetPlayerLastPos());
        Message msg = new Message("GameProcessManager/ChangeScene : Rpg_Intro, " + monsterName).FunctionCall();

        //SceneManager.LoadScene("Rpg_Intro");

        //Die();
        //Debug.Log("몬스터 사망");
    }

    /// <summary>
    /// 죽었나 확인하는것
    /// </summary>
    public void isDie()
    {
        InfoManager infomanager = new InfoManager();
        Debug.Log("싸움을 걸었던 적의 코드 : " + matchedNpcCode);

        if (infomanager.GetSceneStartValue() == "Win" && gameObject.transform.parent.name == matchedNpcCode)
        {
            //Debug.Log(matchedNpcCode + "를 이겼다!");
            //Debug.Log(matchedNpcCode + "의 마지막 위치" + lastEnemyPosition);
            npc.transform.position = lastEnemyPosition;
            Die();
            // 플레이어 싸우던 위치로 이동
            player.transform.position = infoManager.GetPlayerLastPos();
        }
        else if(infomanager.GetSceneStartValue() == "Lose" && gameObject.transform.parent.name == matchedNpcCode)
        {
            //Debug.Log(matchedNpcCode + "에게 졌다..");
            //Debug.Log(matchedNpcCode + "의 마지막 위치" + lastEnemyPosition);
            player.transform.position = infoManager.GetPlayerLastPos();
        }
        matchedNpcCode = "";
        
        
    }

    /// <summary>
    /// 죽으면 사라지는것
    /// </summary>
    void Die()
    {
        deadEffect.transform.position = npc.transform.position;
        deadEffect.GetComponent<ParticleSystem>().Play();

        hitSound.Play();

        isLive = false;
        npc.SetActive(false);
    }
}
