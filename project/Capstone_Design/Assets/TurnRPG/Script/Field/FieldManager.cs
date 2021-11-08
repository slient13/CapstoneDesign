using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public GameObject enemy;

    static string matchedEnemy;

    private void Start()
    {
        //Debug.Log("싸웠던 적 :" + matchedEnemy);
        GameObject enemyManager;

        enemy = GameObject.Find(matchedEnemy);
        if (enemy != null)
        {
            enemyManager = enemy.transform.Find("NpcCommandManager").gameObject;

            enemyManager.GetComponent<NpcCommandManager>().isDie();
        }
    }

    /// <summary>
    /// 싸움을 건 적 이름 설정
    /// </summary>
    /// <param name="value"></param>
    public void SetMatchedEnemy(string value)
    {
        matchedEnemy = value;
        Debug.Log("싸움을 시작한 적 :" + matchedEnemy);
    }
}
