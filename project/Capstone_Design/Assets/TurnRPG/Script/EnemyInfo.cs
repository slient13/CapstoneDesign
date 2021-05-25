using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public struct Enemy
    {
        public string code;
        string name;
        string[] skills;
        string[] itemdrop;
        float hp;
        float damage;
    };

    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Enemy GetEnemyInfo()
    {
        return enemy;
    }

    void OnDie()
    {

    }
}
