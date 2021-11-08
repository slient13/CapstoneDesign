using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public GameObject enemyArea;
    public GameObject rallyPoint;
    public GameObject returnPoint;
    public float minTime;
    public float maxTime;
    public float speed;

    GameObject moveArea;
    Rigidbody npcBody;
    float timer = 0f;
    float randomTime = 0f;
    bool moving;
    bool isRightArea;
    bool enable;
    bool returning;


    // Start is called before the first frame update
    void Start()
    {
        enable = true;
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
        if(enable)
        {
            //활성화시
            if (!moving)
            {
                timer += Time.deltaTime;
            }

            if (!moving && timer >= randomTime)
            {
                int i = 0;

                while (true)
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

            if (moving && timer >= randomTime)
            {
                MoveToTarget(rallyPoint);
            }
        }
        else
        {
            if(returning)
            {
                MoveToTarget(returnPoint);
            }
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

    void MoveToTarget(GameObject rally)
    {
        transform.LookAt(new Vector3(rally.transform.position.x, 1, rally.transform.position.z));
        transform.position = Vector3.MoveTowards(transform.position, rally.transform.position, speed * Time.deltaTime);
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
    
    /// <summary>
    /// 배회 알고리즘 활성화 설정
    /// </summary>
    /// <param name="boolean"></param>
    //활성화 설정
    void SetEnable(bool boolean)
    {
        enable = boolean;
    }


    /// <summary>
    /// 초기화지점으로 돌아가게한다
    /// </summary>
    void SetReturning(bool boolean)
    {
        returning = boolean;
    }


}
