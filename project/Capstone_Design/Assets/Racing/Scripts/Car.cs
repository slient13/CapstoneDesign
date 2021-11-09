using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Nav 기능을 사용할 수 있게 한다.

public class Car : MonoBehaviour
{
    public float carSpeed; //차량 스피드

    public float carRotate;

    public float carAccel;
    public Transform target; //목적지

    //public float speed;
    int nextTarget; // 목적지 순서를 결정 

    public bool player; // 플레이어 챠량 여부 

    //게임이 시작되면 한번 실행하는 부분 => private void Start()
    //일반 시작 함수로 변환 => 수동적으로 실행시켜야 한다.
    public void StartAI()
    {
        // 플레이어 차량이 아닌 차량들만 AI 기능 부여 
        if(!player)
        {
            //게임시작하면,AI의 목적지를 RacingGameManager의 목적지로 설정
            target=RacingGameManager.instance.target[nextTarget];

            //NavMeshAgent의 speed를 carSpeed와 일치시킨다.
            GetComponent<NavMeshAgent>().speed = carSpeed;
            GetComponent<NavMeshAgent>().acceleration = carAccel;
            // GetComponent<NavMeshAgent>().angularSpeed = carRotate;
            GetComponent<NavMeshAgent>().autoBraking = true;

            //AI코르틴을 실행시킨다. => 그래야 작동이 된다.
            StartCoroutine("AI_Move");
        
            //Animation 코르틴을 실행한다. 
            StartCoroutine("AI_Animation");
        }
    }
    //AI용 코르틴 => 차량들이 스스로 움직이도록 해준다.
    IEnumerator AI_Move()
    {
        //AI 코르틴이 시작하면, 
        //NavMeshAgent를 가져와서 목적지의 위치로 AI를 이동시킨다.
        GetComponent<NavMeshAgent>().SetDestination(target.position);

        while(true)
        {
            //목적지와 현재 거리의 차이를 이용해서 남은 거리를 계산 
            float dis = (target.position-transform.position).magnitude;

            //목적지와의 거리가 1보다 작으면
            if(dis <= 10)
            {
                //다음 타겟으로 목적지 변경
                nextTarget += 1; 

                //만약 마지막 목적지에 다다르면 
                if(nextTarget >= RacingGameManager.instance.target.Length)
                    nextTarget = 0; //목적지를 다시 처음으로 되돌린다.

                target=RacingGameManager.instance.target[nextTarget];
                
                //AI를 다시 다음 목적지로 출발시킨다. 
                GetComponent<NavMeshAgent>().SetDestination(target.position);
            }
            yield return null;
        }
    }
    //애니메이션을 재생시켜줄 코르틴
    //코르틴이 실행되면,
    IEnumerator AI_Animation()
    {
        Vector3 lastPosition; //빈 벡터를 하나 만들어준다.
        while(true)
        {
            lastPosition = transform.position; //벡터에 차량 위치를 넣는다.
            //0.03초 있다가
            yield return new WaitForSecondsRealtime(0.03f);

            //현재 차량 위치와 lastPosition을 비교 
            //차량이 이동을 했다면,
            if((lastPosition - transform.position).magnitude > 0)
            {
                //이동을 어느 방향으로 했는지 파악
                //lastPosition에 대한 상대적 좌표 계산
                Vector3 dir = transform.InverseTransformPoint(lastPosition);

                //차량이 전진했다면, => 범위를 조금 여유를 둬서 잡는다.
                if(dir.x >= -0.01f && dir.x <= 0.01f)
                {
                    //직진 애니메이션 실행
                    GetComponent<Animator>().Play("Ani_Forward");
                }
                if(dir.x < -0.01f)
                {
                    //우회전 애니메이션 실행
                    GetComponent<Animator>().Play("Ani_Right");
                }
                if(dir.x > 0.01f)
                {
                    //좌회전 애니메이션 실행
                    GetComponent<Animator>().Play("Ani_Left");
                }
            }

            //차량이 안 움직였다면,
            if((lastPosition - transform.position).magnitude <= 0)
            {
                //멈춰있는 애니메이션 실행
                GetComponent<Animator>().Play("Ani_Idle");
            }

            yield return null;
        }
    }

     private void Update()
     {
        // if(Input.GetKey(KeyCode.A)){
        //     this.transform.Rotate(0.0f, -90.0f * Time.deltaTime, 0.0f);
        //     GetComponent<Animator>().Play("Ani_Left");
        // }
        // if(Input.GetKeyDown(KeyCode.A)){
        //     GetComponent<Animator>().Play("Ani_Right");
        // }
        // if(Input.GetKey(KeyCode.D)){
        //     this.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);
        //     GetComponent<Animator>().Play("Ani_Right");
        // }
        // if(Input.GetKeyDown(KeyCode.D)){
        //     GetComponent<Animator>().Play("Ani_Left");
        // }
     }
    
    //충돌을 감지하는 트리거
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Finish")
            RacingGameManager.instance.passCount += 1; // 아무 차량이나 지나갈 시 수를 셈.

        //player 차량이 
        if(player)
        {
            //골인지점을 통과했다면, 
            if(other.gameObject.tag == "Finish")
            {
                //checkpoint지점을 통과했다면, LapTime기능 실행
                // => 한바퀴를 안돌고 얍삽하게 거꾸로 골인하는 것 방지 위함
                if(RacingGameManager.instance.check)
                {
                    //checkpoint 지점을 통과하면 다시 false로 바꿔준다. 
                    RacingGameManager.instance.check = false;

                    //lap이 0보다 크다면,
                    if(RacingGameManager.instance.lap > 0)
                    {
                        //한바퀴 돌때마다 효과음 넣어주기 
                        SE_Manager.instance.PlaySound(SE_Manager.instance.lap);
                        //LapTime기능을 작동시킨다 
                        RacingGameManager.instance.LapTime();
                    }

                    //finish라인에 player 차량이 충돌하면 골인했으므로 lap+1
                    RacingGameManager.instance.lap += 1;
                }   
            }
            //checkpoint 지점을 통과했다면,
            if(other.gameObject.tag == "CheckPoint")
            {
                //check 변수를 true로 바꿔준다. 
                RacingGameManager.instance.check = true;
            }
        }
    }

    //키보드 조작 테스트 
    //  void FixedUpdate(){
    //     float hor = Input.GetAxis("Horizontal");
    //     float ver = Input.GetAxis("Vertical");

    //     transform.Translate(Vector3.forward * speed * ver * Time.deltaTime);      //이동
    //     transform.Rotate(Vector3.up * speed * hor);    // 회전
    // }
}
