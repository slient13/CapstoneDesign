using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleNpc : MonoBehaviour
{
    public GameObject npcCommandManager;

    // 콜리전 발생시 턴제게임이 시작된다
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            npcCommandManager.GetComponent<NpcCommandManager>().SendMessage("BattleStart", true);

            Debug.Log(collision.gameObject.name + "과 전투가 시작되었습니다");
        }
    }

    void Respawn()
    {
        this.gameObject.SetActive(true);
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
