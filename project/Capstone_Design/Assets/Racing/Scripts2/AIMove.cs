using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // AI기능을 사용할 수 있도록 하는 라이브러리

public class AIMove : MonoBehaviour
{
    public float carSpeed; //차 속력
    public Transform target; // 목적지 

    int nextTarget; // 목적지 순서를 체크할 변수 
    public bool player; // 플레이어 차량인지 확인할 bool 

    
    // 시작할때 한번 실행되는 함수
    void Start()
    {
        // 플레이어 차량이 아닌 경우만 
        if(!player)
        {
            // 게임 실행 후, 자동으로 목적지를 넣는다.
            target = RaceManager.instance.target[nextTarget];

            // 차 속력과 AI 차량의 속력을 같게한다. 
            GetComponent<NavMeshAgent>().speed = carSpeed; // NavMeshAgent를 가져와 실행시킨다.

            //AI 코르틴을 실행시킨다.
            StartCoroutine("AI_Move"); // 이 부분이 없으면, AI 차량들이 움직이지 않는다.
            StartCoroutine("AI_Animation"); // AI 차량의 움직임에 애니메이션을 더한다.
        }
    }


    // AI 차량용 코르틴 => AI 차량들이 움직이도록  
    IEnumerator AI_Move()
    {
        // AI 코르틴이 실행되면, 목적지로 AI 차량들을 이동시킨다.  
        GetComponent<NavMeshAgent>().SetDestination(target.position);
        while(true)
        {
            // (목적지 위치 - 차량 위치)를 통해 목적지와의 거리를 계산한다.
            float dis = (target.position - transform.position).magnitude;

            // 목적지와의 거리가 가까워지면,
            if(dis <= 1)
            {
                nextTarget += 1; // 다음 목적지로 이동

                // nextTarget의 값이 목적지의 개수보다 커지면,
                if(nextTarget >= RaceManager.instance.target.Length)
                    nextTarget = 0; //다시 nextTarget으로 초기화 => 차량들이 계속 달리도록

                target = RaceManager.instance.target[nextTarget]; // 다음 목적지 설정

                // 다시 다음 목적지로 AI 차량들을 이동시킨다.  
                GetComponent<NavMeshAgent>().SetDestination(target.position);
            }
            yield return null;
        }
    }

    // AI 애니메이션 재생 전용 코르틴 -> AI 차량의 움직임에 따른 애니메이션을 실행시켜준다.
    IEnumerator AI_Animation()
    {
        Vector3 lastPosition; // 코르틴이 실행되면 빈 벡터 하나 생성 
        while(true)
        {
            // 빈 벡터에 차량 위치를 넣어준다.
            lastPosition = transform.position;
            // 0.03초 정도 있다가,
            yield return new WaitForSecondsRealtime(0.03f);
            
            //차량이 움직였다면,
            if((lastPosition - transform.position).magnitude > 0)
            {
                //어디로 움직였는지 구한다음,
                Vector3 dir = transform.InverseTransformPoint(lastPosition);
                if(dir.x >= -0.01f && dir.x <= 0.01f) // 상대적인 x좌표가 if문의 범위에 해당한다면,
                    GetComponent<Animator>().Play("Ani_Forward"); // 직진 애니메이션 실행 
                if(dir.x < -0.01f) // 상대적인 x좌표가 if문의 범위에 해당한다면,
                    GetComponent<Animator>().Play("Ani_Right"); // 우회전 애니메이션 실행
                if(dir.x > 0.01f) // 상대적인 x좌표가 if문의 범위에 해당한다면,
                    GetComponent<Animator>().Play("Ani_Left"); // 좌회전 애니메이션 실행
            }else if((lastPosition - transform.position).magnitude <= 0) // 차량이 움직이지 않았다면,
                GetComponent<Animator>().Play("Ani_Idle"); // 정지 애니메이션 실행

            yield return null;
        }
    }

    // 매 프레임마다  실행되는 함수 
    // void Update()
    // {
    //     if(Input.GetKey(KeyCode.W)){
    //         this.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);
    //     }
    //     if(Input.GetKey(KeyCode.S)){
    //         this.transform.Translate(Vector3.back * carSpeed * Time.deltaTime);
    //     }
    //     // if(Input.GetKey(KeyCode.A)){
    //     //     this.transform.Translate(Vector3.left * 5.0f * Time.deltaTime);
    //     // }
    //     // if(Input.GetKey(KeyCode.D)){
    //     //     this.transform.Translate(Vector3.right * 5.0f * Time.deltaTime);
    //     // }
    //     if(Input.GetKey(KeyCode.A)){
    //         this.transform.Rotate(0.0f, -90.0f * Time.deltaTime, 0.0f);
    //     }
    //     if(Input.GetKey(KeyCode.D)){
    //         this.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);
    //     }
    // }
}
