using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//마우스 커서 드래그 및 움직임을 통한 핸들러 추가 
public class Control : MonoBehaviour
   , IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    //pad와 stick에 각각 RectTransform 만들어준다. 
    public RectTransform pad;
    public RectTransform stick;

    //플레이어의 회전값
    Vector3 playerRotate;

    float carRotate;


    Car player; // 플레이어 차량 자리

    Animator playerAni; // 플레이어 차량 애니메이션

    bool onMove; // 주행 여부

    //테스트를 위해서 플레이어 속도를 public으로 바꿔준다. 
    public float playerSpeed; // 플레이어 속도 

    bool Accel;

    float dir;

    [Header("MiniMap")]
    public GameObject minimap; // 미니맵 자리
    public Transform minimapCam; // 미니맵 카메라 자리 

    public float maxSpeed;


    //게임이 시작되면 한번 실행하는 부분 => private void Start()
    //일반 시작 함수로 변환 => 수동적으로 실행시켜야 한다.

    public void StartController()
    {
        //RacingGameManager의 플레이어 차량을 player변수에 담는다.
        player = RacingGameManager.instance.player;

        maxSpeed = player.carSpeed;

        //player의 Animator를 가져오기 
        playerAni = player.GetComponent<Animator>();

        //플레이어 차 전용 코르틴 실행
        StartCoroutine("PlayerMove");

        carRotate = player.carRotate;

        if (player.carSpeed == 12.5)
            player.carRotate = 3;
    }
    //드래그하는 동안
    public void OnDrag(PointerEventData eventData)
    {
        //스틱이 손가락을 따라다니게 함.
        stick.position = eventData.position;
        //스틱이 움직일때 원을 벗어나지 않도록 범위 지정
        stick.localPosition = Vector3.ClampMagnitude(eventData.position -
        (Vector2)pad.position, pad.rect.width * 0.5f);

        //스틱이 움직이는 동안, 플레이어의 회전율을 정해준다.
        //y축만 회전 시켜준다. => normalized로 정교화
        playerRotate = new Vector3(0, stick.localPosition.x, 0).normalized;
    }
    //스틱에서 손을 떼면 
    public void OnPointerUp(PointerEventData eventData)
    {
        //스틱을 원래 위치로 되돌려 놓기 
        stick.localPosition = Vector3.zero;
        //플레이어 회전값도 0으로 초기화
        playerRotate = Vector3.zero;
    }
    public void OnMove() // 버튼을 누르면, 
    {
        if (!Accel)
        {
            StartCoroutine("Acceleration"); //엑셀 코르틴 시작
        }
        onMove = true; //주행여부 true
    }
    public void OffMove() // 버튼을 떼면,
    {
        StartCoroutine("Braking"); //감속 코르틴 시작
    }

    public void Rotate()
    {
        float offset = Time.deltaTime * 3;

        if (Input.GetKey(KeyCode.A))
        {
            dir += -1 * offset;
            if (dir < -1)
                dir = -1;
            maxSpeed = player.carSpeed * 0.95f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir += 1 * offset;
            if (dir > 1)
                dir = 1;
            maxSpeed = player.carSpeed * 0.95f;
        }
        else
        {
            if (dir > 0)
                dir += -1 * offset;

            if (dir < 0)
                dir += 1 * offset;
            if (Mathf.Abs(dir) <= 0.1)
                dir = 0;
            maxSpeed = player.carSpeed;
        }

        playerRotate = new Vector3(0, dir, 0).normalized;
    }
    //플레이어 차 전용 코르틴
    IEnumerator PlayerMove()
    {
        minimap.SetActive(true); // 미니맵 켜주기 
        while (true)
        {

            //onMove가 true인 경우에만 플레이어가 이동 
            if (onMove)
            {
                //플레이어의 속도를 RacingGameManager의 curSpeedText에 대입
                RacingGameManager.instance.curSpeedText.text =
                    string.Format("{0:000.00}", playerSpeed * 10);

                //플레이어 차량을 앞으로 playerSpeed 속도만큼 이동시킨다. 
                //* Time.deltaTime를 해주면 어느 기기든지 동일한 속도로 반영 
                player.transform.Translate(Vector3.forward * playerSpeed
                  * Time.deltaTime);
                if (Mathf.Abs(stick.localPosition.x) > pad.rect.width * 0.2f)
                    player.transform.Rotate(playerRotate * 30 * Time.deltaTime);

                //스틱의 가동 범위의 절대값이 반지름보다 작거나 같다면 => 차량 직진
                //if (Mathf.Abs(stick.localPosition.x) <= pad.rect.width * 0.2f)
                if (dir == 0)
                    playerAni.Play("Ani_Forward"); //직진 애니메이션 작동
                //스틱의 가동 범위가 패드넓이의 0.2보다 크다면 => 우회전 
                //if (stick.localPosition.x > pad.rect.width * 0.2f)
                if (dir > 0)
                    playerAni.Play("Ani_Right"); //우회전 애니메이션 작동
                //스틱의 가동 범위가 패드넓이의 0.2보다 작다면 => 좌회전 
                //if (stick.localPosition.x < pad.rect.width * -0.2f)
                if (dir < 0)
                    playerAni.Play("Ani_Left"); //좌회전 애니메이션 작동

                if (Input.GetKey(KeyCode.A))
                    player.transform.Rotate(playerRotate * player.carRotate * Time.deltaTime * Mathf.Abs(dir));


                if (Input.GetKey(KeyCode.D))
                    player.transform.Rotate(playerRotate * player.carRotate * Time.deltaTime * Mathf.Abs(dir));


                player.transform.GetChild(3).gameObject.SetActive(true);
                player.transform.GetChild(4).gameObject.SetActive(false);
            }
            //onMove가 false인 경우에는 
            if (!onMove)
            {
                playerAni.Play("Ani_Idle"); //대기 상태 애니메이션 작동

                player.transform.GetChild(3).gameObject.SetActive(false);
                player.transform.GetChild(4).gameObject.SetActive(true);
            }

            // 미니맵 카메라가 일정 간격을 두고 플레이어를 따라가게 만든다. 
            minimapCam.position = player.transform.position +
                new Vector3(0, 100, 0);
            yield return null;
        }
    }
    //엑셀 기능
    IEnumerator Acceleration()
    {
        Accel = true;
        //다시 엑셀 기능이 시작되면, 감속 코르틴 정지 
        StopCoroutine("Braking");

        while (true)
        {

            // playerSpeed가 증가하다가 차량의 속도보다 커지면
            if (playerSpeed >= maxSpeed)
                playerSpeed -= player.carAccel * Time.deltaTime; //10씩 감소 시킨다.
            else
                playerSpeed += player.carAccel * Time.deltaTime;

            yield return null;
        }
    }
    //감속 기능
    IEnumerator Braking()
    {
        Accel = false;
        //엑셀 버튼에서 손을 뗐으므로, 엑셀 코르틴을 멈춘다.
        StopCoroutine("Acceleration");

        while (true)
        {
            //playerSpeed를 초당 7씩 감속
            playerSpeed -= 7 * Time.deltaTime;

            if (playerSpeed <= 0) //playerSpeed가 0보다 작아지면,
            {
                playerSpeed = 0; //playerSpeed를 0으로 유지(최소 속도)
                onMove = false; //주행 여부 false
                StopCoroutine("Braking"); //감속 코르틴 멈춘다.
            }
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    //키매핑부분 => 테스트 
    //좌회전, 우회전에 따른 함수를 만들고 코르틴 만들어주기 
    private void Update()
    {
        playerAni.SetBool("Forward", false);
        playerAni.SetBool("Left", false);
        playerAni.SetBool("Right", false);
        if (Input.GetKey(KeyCode.W)) //W키를 누르면
            OnMove(); //가속
        if (Input.GetKeyUp(KeyCode.W)) //W키를 떼면 
            OffMove(); //감속  

        if (Input.GetKey(KeyCode.A))
            player.transform.Rotate(playerRotate * 30 * Time.deltaTime);


        if (Input.GetKey(KeyCode.D))
            player.transform.Rotate(playerRotate * 30 * Time.deltaTime);

        Rotate();
    }


}
