using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour // 레이스게임의 전반적인 매니저 스크립트 
{
    //RaceManager 스크립트에 대한 접근이 쉽도록 instance화 시킨다.
    public static RaceManager instance;

    [Header("Player")]
    public AIMove player; // 플레이어로 선택될 차를 담을 자리 


    public float baseSpeed; // 랜덤 속도의 기준 속도 

    [Header("GameObj")]
    public AIMove[] car; // 차량들을 담을자리 => AIMove 스크립트 이용 
    public Transform[] target; // 목적지들을 담을 자리 

    private void Awake()
    {
        if(instance == null)
            instance = this;
        
        SpeedSet(); // 기능을 실행

        player.player = true; // 플레이어의 플레이어체크를 true로 
    }

    // 차량의 속도를 랜덤으로 부여하는 함수 
    void SpeedSet()
    {
        for(int i = 0; i < car.Length; i++)
        {
            // 차량들은 1이내에서 랜덤으로 속도가 부여된다. 
            car[i].carSpeed = Random.Range(baseSpeed,baseSpeed+2f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
