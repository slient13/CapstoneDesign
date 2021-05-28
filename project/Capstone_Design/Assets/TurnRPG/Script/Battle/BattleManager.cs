using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public string[] mainComment;
    public string monsterName;

    // Start is called before the first frame update
    void Start()
    {
        mainComment = new string[] {"앗! 야생의 " + monsterName + "이 나타났다!",};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
