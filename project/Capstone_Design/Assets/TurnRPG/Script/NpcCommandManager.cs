using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCommandManager : MonoBehaviour
{
    public GameObject npc;
    public float respawnTime;
    
    //플레이어 정보를 전달하는 매니저
    public GameObject systemManager;

    bool isLive;

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
        npc.GetComponent<BattleNpc>().SendMessage("Respawn");
    }


    /// <summary>
    /// 죽으면 사라지는것
    /// </summary>
    void Die()
    {
        npc.SetActive(false);
    }

}
