using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCommandManager : MonoBehaviour
{
    public GameObject npc;
    public float respawnTime;

    //플레이어 정보를 전달하는 매니저
    public GameObject systemManager;

    bool isLive = true;
    float timer;
    AudioSource hitSound;
    GameObject deadEffect;

    private void Start()
    {
        hitSound = transform.Find("HitSound").GetComponent<AudioSource>();
        deadEffect = transform.Find("ParticleDead").gameObject;
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
        Die();
        Debug.Log("몬스터 사망");
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
