using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public GameObject enemyArea;
    public GameObject rallyPoint;
    public Rigidbody npcBody;
    public float minTime;
    public float maxTime;


    float timer = 0.0f;
    float randomTime = 0.0f;
    bool moving = false;
    Vector3 rallyPos;
    public GameObject moveArea;

    // Start is called before the first frame update
    void Start()
    {
        moveArea = transform.Find("MoveArea").gameObject;
        //moveArea.SetActive(false);
        npcBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(!moving)
        {
            randomTime = Random.Range(minTime, maxTime);
            moving = true;
        }

        if(moving && timer>=randomTime)
        {
            SetPosition();
            MoveToTarget();
        }
    }

    void SetPosition()
    {
        float xRange = moveArea.transform.localScale.x;
        float zRange = moveArea.transform.localScale.z;

        float moveX = Random.Range(0, xRange) - xRange / 2;
        float moveZ = Random.Range(0, zRange) - zRange / 2;

        rallyPos = new Vector3(transform.position.x - moveX, transform.position.y, transform.position.z - moveZ);
        Debug.Log(rallyPos);
    }

    void MoveToTarget()
    {
        transform.LookAt(rallyPos);
        
    }

    /*
    void MoveToTarget()
    {
        if (target.transform.position != Vector3.MoveTowards(transform.position, target.transform.position, 1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1f); // 현위치, 도착점, 속도

            transform.LookAt(target.transform);
            //Debug.Log(Vector3.MoveTowards(transform.position, target.transform.position, 1f));

            farmer_animator.IsMove = true;
        }
        else
            farmer_animator.IsMove = false;
    }
    */
}
