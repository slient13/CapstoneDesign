using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public GameObject enemyArea;
    public GameObject rallyPoint;
    public float minTime;
    public float maxTime;
    public float speed;


    GameObject moveArea;
    Rigidbody npcBody;
    float timer = 0f;
    float randomTime = 0f;
    bool moving;
    public bool isRightArea;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        isRightArea = false;

        moveArea = transform.Find("MoveArea").gameObject;
        //moveArea.SetActive(false);
        npcBody = GetComponent<Rigidbody>();

        randomTime = Random.Range(minTime, maxTime);

        //이동범위조절 콜리더 작동중지
        //moveArea.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!moving)
        {
            timer += Time.deltaTime;
        }

        if(!moving && timer>=randomTime)
        {
            int i = 0;

            while(true)
            {
                SetPosition();
                IsRightArea();
                i++;
                if (isRightArea)
                {
                    moving = true;
                    break;
                }
                else if (i > 10)
                {
                    EndMove();
                    Debug.Log("Failed To Move.. " + this.name);
                    break;
                }
            }
        }

        if(moving && timer>=randomTime)
        {
            MoveToTarget();
        }
    }

    void SetPosition()
    {
        float xRange = moveArea.transform.localScale.x;
        float zRange = moveArea.transform.localScale.z;

        float moveX = Random.Range(0, xRange) - xRange / 2;
        float moveZ = Random.Range(0, zRange) - zRange / 2;

        rallyPoint.transform.position = new Vector3(transform.position.x - moveX, transform.position.y, transform.position.z - moveZ);
        Debug.Log(rallyPoint.transform.position);
    }

    void MoveToTarget()
    {
        transform.LookAt(rallyPoint.transform);
        transform.position = Vector3.MoveTowards(transform.position, rallyPoint.transform.position, speed * Time.deltaTime);
        //npcBody.AddForce(speed * transform.forward);
    }

    void EndMove()
    {
        randomTime = Random.Range(minTime, maxTime);
        timer = 0.0f;
        isRightArea = false;
        moving = false;
    }

    //몬스터가 활동영역 내에 있는지 확인
    void IsRightArea()
    {
        float xRange = enemyArea.transform.localScale.x;
        float zRange = enemyArea.transform.localScale.z;
        float localX = rallyPoint.transform.localPosition.x;
        float localZ = rallyPoint.transform.localPosition.z;

        if ((localX + xRange/2) <= xRange && (localX + xRange / 2) >= 0 && (localZ + zRange/2) <= zRange && (localZ + zRange / 2) >=0)
            isRightArea = true;
        else
            isRightArea = false;
    }
}
