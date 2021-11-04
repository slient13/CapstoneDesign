using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    AIMove player; // 플레이어 차량자리 
    bool onMove; // 차량이 이동중인지 확인 
    public float playerSpeed; // 플레이어 차량의 속도 

    // 게임이 실행되면 한번 실행되는 함수 
    private void Start()
    {
        // 게임이 시작하면 RaceManage에서 play로 선택된 차량을 player에 넣는다.
        player = RaceManager.instance.player; 
        StartCoroutine("PlayerMove");
    }
     
    // 플레이어 차량 전용 코르틴
    IEnumerator PlayerMove()
    {
        while(true)
        {
            player.transform.Translate(Vector3.forward * playerSpeed
                * Time.deltaTime);
            
            yield return null;
        }
    }
}
