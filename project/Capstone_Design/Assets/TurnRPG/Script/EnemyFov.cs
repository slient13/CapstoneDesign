using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFov : MonoBehaviour
{
    public float trigTime;
    public float chasingTime;
    public GameObject npcCommandManager;

    float timer;
    bool isFound;
    bool finding;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        finding = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFound && !finding)
        {
            timer += Time.deltaTime;
            npcCommandManager.GetComponent<NpcCommandManager>().SendMessage("ChaseTarget",target);

            if(timer >= chasingTime)
            {
                npcCommandManager.GetComponent<NpcCommandManager>().SendMessage("InitPos");
                isFound = false;
                finding = true;
                timer = .0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (other.gameObject.tag == "Player" && finding)
        {
            timer += Time.deltaTime;
            if (timer >= trigTime)
            {
                Debug.Log("플레이어가 적을 자극했습니다!");
                npcCommandManager.GetComponent<NpcCommandManager>().SendMessage("CancelReturning");
                target = other.gameObject;
                isFound = true;
                finding = false;
                timer = .0f;
            }
        }
    }
}
