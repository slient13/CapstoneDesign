using System.Collections;
using System.Collections.Generic;
using TMPro; //TextMeshProUGUI를 사용하기 위해 필요한 네임스페이스
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 하기 위한 네임스페이스 

public class RacingGameManager : MonoBehaviour
{
    //RacingGameManager를 instance화 하여 접근이 쉽도록 한다.
    public static RacingGameManager instance;

    //플레이어 부분
    [Header("Player")]
    public Car player; // 플레이어 차량 자리 -> Control 스크립트에서 사용

    public float baseSpeed; //랜덤속도에 기준이 될 기본 속도 
    public int lap; //몇바퀴째인지 확인할 변수 
    public int passCount; // 차량들이 지나간 횟수를 체크할 변수.
    public bool check; //체크 포인트 지점을 통과했는지 확인할 bool 변수 


    [Header("GameObj")]
    public Car[] car; //차량들을 담을 자리 

    public Transform[] target; //목적지들을 담을 자리
    public Control controllPad; //컨트롤 패드 스크립트 자리
    public Transform cam; //카메라 자리 

    [Header("Menu")] //메뉴 기능 생성 
    public GameObject startMenu; //시작 메뉴
    public GameObject selectMenu; //선택 메뉴
    public GameObject ui; //ui 자리
    public GameObject finishMenu; //finishMenu 자리 

    [Header("Text")]
    public TextMeshProUGUI bestTimeText; //최고 기록을 나타낼 text
    public TextMeshProUGUI countText; //countText
    public TextMeshProUGUI curTimeText; //현재 시간을 나타낼 text 
    public TextMeshProUGUI curSpeedText; //현재 스피드를 나타낼 text 
    public TextMeshProUGUI[] lapTimeText; //한 바퀴 돌때마다 시간을 나타낼 text
    public TextMeshProUGUI finishText; // 게임 종료 후 등수나 보상 등을 안내할 메시지.

    string carName;



    float curTime; //현재 시간을 담을 변수 
    float bestTime; //최고 기록을 담을 변수 

    //이렇게 하면, 다른 스크립트에서 RacingGameManager를 쉽게 접근할 수 있다.
    private void Awake()
    {
        if (instance == null)
            instance = this;
        this.finishText.text = "";

        SetCarStat();
        BestTimeSet();
    }

    //Let's go를 눌렀을때 실행될 시작 기능 구현
    public void GameStart()
    {
        bool check = (bool)new Message($"SelectMenu/BuyCar : {this.carName}").FunctionCall().returnValue[0];
        this.passCount = -5; // 첫 출발선에서 감지되는 것에 대한 보정.

        if (check == true)
        {
            StartCoroutine("StartCount"); //카운트다운 코르틴 실행 
        }
    }

    public void ChangeSelectedCarName(Message message)
    {
        this.carName = (string)message.args[0];
    }
    //저장된 best 기록을 가져오는 기능
    void BestTimeSet()
    {
        //저장했던 BestLap(최고기록)을 불러와서 넣기 
        bestTime = PlayerPrefs.GetFloat("BestLap");
        //bestTimeText를 text로 바꿔준다. 
        bestTimeText.text = string.Format("Best {0:00}:{1:00.00}",
            (int)(bestTime / 60 % 60), bestTime % 60);

        if (bestTime == 0)
        {
            bestTimeText.text = "Best   -";
        }
    }

    //lapTime 기능
    public void LapTime()
    {
        //lap이 3이라면,
        if (lap == 3)
        {
            {   // 골 처리
                //goal 효과음 넣어주기 
                SE_Manager.instance.PlaySound(SE_Manager.instance.goal);
                cam.parent = null; //차량의 카메라를 밖으로 꺼내기
                StopCoroutine("Timer"); //타이머 기능 종료 
                finishMenu.SetActive(true); //finishMenu 활성화

                //player.player = false; //플레이어로 지정된 차량은 더 이상 플레이어 차량이 x
                //player.StartAI(); //AI로 바뀐 해당 카트에 다시 AI 기능을 부여한다.
                controllPad.gameObject.SetActive(false); //컨트롤  패드 비 활성화
                //player의 3번쨰 요소인 사운드 기능 비 화성화 
                player.transform.GetChild(3).gameObject.SetActive(false);
            }
            {   // 보상 처리 
                this.giveReward();
            }

            // 최고 기록 처리.
            if (curTime < bestTime | bestTime == 0)
            {
                //bestTimeText를 비 활성화 
                bestTimeText.gameObject.SetActive(false);
                //bestTimeText를 현재 시간으로 바꿔준다. 
                bestTimeText.text = string.Format("Best {0:00}:{1:00.00}",
                    (int)(curTime / 60 % 60), curTime % 60);
                //bestTimeText를 다시 활성화 
                bestTimeText.gameObject.SetActive(true);

                //게임을 종료해도 기록을 남기기 위함
                //PlayerPrefs의 BestLap에 curtime대입 
                PlayerPrefs.SetFloat("BestLap", curTime);
            }

        }

        //lapTimeText는 0번째 부터 있으므로 -1, false로 비활성화 
        lapTimeText[lap - 1].gameObject.SetActive(false);
        //lapTimeText를 현재 시간으로 바꿔준다. 
        lapTimeText[lap - 1].text = string.Format("{0:00}:{1:00.00}",
            (int)(curTime / 60 % 60), curTime % 60);
        //바뀐 lapTimeText를 다시 활성화 시키기 
        lapTimeText[lap - 1].gameObject.SetActive(true);
    }

    void giveReward()
    {
        int rank = (this.passCount - 10);
        int reward = 30000 - (rank-1) * 7500;
        this.finishText.text = $"Rank: {rank}!\nreward: {reward}";

        Debug.Log($"RacingGameManager.giveReward : passed Car count = {this.passCount}, reward = {reward}");
        new Message($"ChangeData : Player.Stat.Money, {reward}").FunctionCall();
    }

    //카운트 다운 기능
    IEnumerator StartCount()
    {
        selectMenu.SetActive(false); //차량 선택 메뉴 비활성화
        ui.SetActive(true); //ui 활성화

        //SE_Manager의 3번 인덱스인 3초 효과음을 실행한다.
        SE_Manager.instance.PlaySound(SE_Manager.instance.count[3]);
        //countText의 텍스트를 "3"으로 바꾼다.
        countText.text = "3";
        //"3"으로 바꾼 countText 오브젝트를 활성화시킨다.
        countText.gameObject.SetActive(true);
        //시간사이의 간격은 1초
        yield return new WaitForSecondsRealtime(1);//1초 후에
        //SE_Manager의 2번 인덱스인 2초 효과음을 실행한다.
        SE_Manager.instance.PlaySound(SE_Manager.instance.count[2]);
        //"3"으로 바꾼 countText 오브젝트를 비활성화시킨다.
        countText.gameObject.SetActive(false);
        //countText의 텍스트를 "2"으로 바꾼다.
        countText.text = "2";
        //"2"으로 바꾼 countText 오브젝트를 활성화시킨다.
        countText.gameObject.SetActive(true);
        //시간사이의 간격은 1초
        yield return new WaitForSecondsRealtime(1);//1초 후에
        //SE_Manager의 1번 인덱스인 1초 효과음을 실행한다.
        SE_Manager.instance.PlaySound(SE_Manager.instance.count[1]);
        //"2"으로 바꾼 countText 오브젝트를 비활성화시킨다.
        countText.gameObject.SetActive(false);
        //countText의 텍스트를 "1"으로 바꾼다.
        countText.text = "1";
        //"1"으로 바꾼 countText 오브젝트를 활성화시킨다.
        countText.gameObject.SetActive(true);
        //시간사이의 간격은 1초
        yield return new WaitForSecondsRealtime(1);//1초 후에
        //SE_Manager의 0번 인덱스인 go 효과음을 실행한다.
        SE_Manager.instance.PlaySound(SE_Manager.instance.count[0]);
        //"1"으로 바꾼 countText 오브젝트를 비활성화시킨다.
        countText.gameObject.SetActive(false);
        //countText의 텍스트를 "GO!"으로 바꾼다.
        countText.text = "GO!";
        //"GO!"으로 바꾼 countText 오브젝트를 활성화시킨다.
        countText.gameObject.SetActive(true);
        //시간사이의 간격은 1초
        yield return new WaitForSecondsRealtime(1);//1초 후에
        //"GO!"으로 바꾼 countText 오브젝트를 비활성화시킨다.
        countText.gameObject.SetActive(false);

        //카운트 다운이 끝나면, 컨트롤 패드 활성화
        controllPad.gameObject.SetActive(true);
        player.player = true; //player로 선택된 차량을 true로 만들어준다.
        //player자리에 지정된 차량은 AI 코르틴이 적용 안됨.

        check = true; //게임을 시작할때, check를 true로 해준다. 

        //Control스크립트의 StartController()실행
        controllPad.StartController();

        //Car스크립트의 StartAI()실행
        for (int i = 0; i < car.Length; i++) //차의 개수만큼 for문 실행 
            car[i].StartAI();

        //카운터다운이 끝나면 타이머 실행
        StartCoroutine("Timer");
    }
    //타이머 기능
    IEnumerator Timer()
    {
        while (true)
        {
            curTime += Time.deltaTime; // 시간이 1초씩 증가 

            //시간이 증가할때마다, curTimeText의 글자 형태도 바꿔주기 
            curTimeText.text = string.Format("{0:00}:{1:00.00}",
            (int)(curTime / 60 % 60), curTime % 60); ;
            yield return null;
        }
    }

    //차량 스펙 지정
    void SetCarStat()
    {
        car[0].carSpeed = 12;
        car[1].carSpeed = 12;
        car[2].carSpeed = 12;
        car[3].carSpeed = 12;
        car[4].carSpeed = 13.0f;

        car[0].carRotate = 30;
        car[1].carRotate = 45;
        car[2].carRotate = 30;
        car[3].carRotate = 75;
        car[4].carRotate = 13;

        car[0].carAccel = 10;
        car[1].carAccel = 11;
        car[2].carAccel = 18;
        car[3].carAccel = 10;
        car[4].carAccel = 5;
    }

    //시작 버튼 기능 구현
    public void StartBtn()
    {
        //StartBtn을 누르면 btn효과음 발생
        SE_Manager.instance.PlaySound(SE_Manager.instance.btn);

        startMenu.SetActive(false); //시작 메뉴 비활성화
        selectMenu.SetActive(true); //선택 메뉴 활성화
    }

    //재 시작 버튼 기능 구현
    public void ReStart()
    {
        //버튼 효과음 
        SE_Manager.instance.PlaySound(SE_Manager.instance.btn);
        new Message($"GameProcessManager/ChangeScene : Racing").FunctionCall();
    }

    public void Cancel()
    {
        Debug.Log("RacingGameManager.Cancel : Selected");
        new Message("GameProcessManager/ChangeScene : LodingSecen").FunctionCall();
    }
}
