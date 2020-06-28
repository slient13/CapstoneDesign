using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    
    public int HP;

    public int coin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //피로도 충전 함수(자고 일어 날 때마다)
    void UpHealth()
    {
        HP = HP + 50;
    }
    //피로도 감소 함수(게임을 할 떄 마다)
    void DownHealth()
    {
        HP = HP - 10;
    }
    //돈 모으는 함수
    void Money()
    {
        coin = coin + 50;
    }
}
