using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro; //TextMeshProUGUI를 사용하기 위해 필요한 네임스페이스

public class SelectMenu : MonoBehaviour, IPointerDownHandler
{
    public Camera cam; // 카메라 자리
    public GameObject finalCheckMenu; //최종 확인 메뉴  
    RaycastHit hit;
    bool checking;


    int money;

    [Header("Text")]
    public TextMeshProUGUI MoneyText; //플레이어의 돈을 나타낼 text

    public TextMeshProUGUI PriceText;

    int[] carPriceList = { 0, 15000, 17000, 20000, 23000 };
    string[] carNameList = { "CarA", "CarB", "CarC", "CarD", "CarE" };
    List<bool> unlock_list;
    RacingDataManager racingDataManager = new RacingDataManager();

    void Start()
    {
        syncMoneyText();
        //money = 10000;
        Debug.Log(money);
        unlock_list = this.racingDataManager.GetUnlockList();
    }

    public void SaveUnlockData()
    {
        this.racingDataManager.SaveUnlockList(this.unlock_list);
    }

    //IPointDownHandler 인터페이스 
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!checking) //checking이 false라면 
        {
            //차량을 선택하면 btn효과음 발생
            SE_Manager.instance.PlaySound(SE_Manager.instance.btn);

            //터치한 지점에 Ray광선을 만든다.  
            Ray ray = cam.ScreenPointToRay(eventData.position);
            //만든 Ray광선을 발사하고, 광선을 맞은 오브젝트를 hit에 저장 
            Physics.Raycast(ray, out hit);

            //터치한 곳에 오브젝트가 있는 경우에만,
            //차량 선택에서 아무것도 없는 곳을 눌렀을때 발생하는 오류 해결 방안
            if (hit.transform != null)
            {
                //광선에 맞은 오브젝트의 태그가 Car라면,
                if (hit.transform.gameObject.tag == "Car")
                {
                    checking = true; //checking을 true로 바꿔준다.
                    //카메라를 선택한 오브젝트안으로 위치시킨다.
                    cam.transform.SetParent(hit.transform);
                    //먼저, 줌 아웃 코르틴을 중지 시킨후에,
                    StopCoroutine("Cam_ZoomOut");
                    //차량을 선택했을때, Cam_Zoomin 코르틴을 활성화한다. 
                    //이렇게 하면, 여러 코르틴들의 중복 실행 방지가 가능하다. 
                    StartCoroutine("Cam_ZoomIn");
                    //finalCheckMenu를 활성화 시킨다.
                    finalCheckMenu.SetActive(true);

                    //선택한 Car가 플레이어가 되게 한다.
                    RacingGameManager.instance.player = hit.transform.
                        GetComponent<Car>();

                    int carPrice = -1;
                    string carName = "";
                    for (int i = 0; i < carNameList.Length; ++i)
                    {
                        if (hit.transform.gameObject.name == this.carNameList[i])
                        {
                            if (this.unlock_list[i] == false)
                                carPrice = this.carPriceList[i];
                            else
                                carPrice = -1;
                            carName = this.carNameList[i];
                            break;
                        }
                    }

                    new Message($"GameManager/ChangeSelectedCarName : {carName}").FunctionCall();
                    syncPriceText(carPrice);
                    syncMoneyText();
                } // end : hit.transform.gameObject.tag == "Car"
            }   // end : hit.transform != null
        }   // end : !checking
    }

    public void BuyCar(Message message)
    {
        string carName = (string)message.args[0];
        bool output = false;

        for (int i = 0; i < carNameList.Length; ++i)
        {
            if (carName == this.carNameList[i])
            {
                output = deal(i);
                break;
            }
        }

        if (output == true)
        {
            MoneyText.text = "";
            PriceText.text = "";
        }

        message.returnValue.Add(output);
    }



    bool deal(int index)
    {
        this.money = getMoney();
        int needMoney = this.carPriceList[index];
        bool unlock = this.unlock_list[index];

        if (unlock == true)
        {
            return true;
        }
        else if (this.money >= needMoney)
        {
            changeMoney(-needMoney);
            this.unlock_list[index] = true;
            this.SaveUnlockData();
            return true;
        }
        else
        {
            return false;
        }

    }

    void syncMoneyText()
    {
        this.money = getMoney();
        MoneyText.text = $"Money : {this.money}";
    }

    void syncPriceText(int needMoney)
    {
        if (needMoney < 0) PriceText.text = $"Unlocked";
        else PriceText.text = $"Price : {needMoney}";
    }

    int getMoney()
    {
        return (int)new Message($"GetPlayInfoValue : Player.Stat.Money").FunctionCall().returnValue[0];
    }

    void changeMoney(int degree)
    {
        new Message($"ChangeData : Player.Stat.Money, {degree}").FunctionCall();
    }


    //카메라의 줌 인 기능
    IEnumerator Cam_ZoomIn()
    {
        while (true)
        {
            //카메라의 localPosition을 Vector3.Slerp를 이용해 부드럽게 줌인
            //카메라의 형제 localPosition에서 new Vector3(0,2,-3.5f)의 위치로
            //카메라가 이동하고, 20*Time.deltaTime의 속도로 줌인된다.
            cam.transform.localPosition =
              Vector3.Slerp(cam.transform.localPosition,
              new Vector3(0, 2, -3.5f), 20 * Time.deltaTime);

            //카메라가 이동하다가 localPosition.z이 -3.5f보다 크거나 같아지면,
            //줌인 코르틴 종료 
            if (cam.transform.localPosition.z >= -3.5f)
                StopCoroutine("Cam_ZoomIn");
            yield return null;
        }
    }
    //취소버튼 기능
    public void CancelBtn()
    {
        //CancelBtn을 누르면 btn효과음 발생
        SE_Manager.instance.PlaySound(SE_Manager.instance.btn);

        //취소버튼을 누르면, 줌 인 코르틴을 중지시킨후에
        StopCoroutine("Cam_ZoomIn");
        //줌 아웃 코르틴 실행 => 코르틴들의 겹침을 방지하기 위함
        StartCoroutine("Cam_ZoomOut");
        //finalCheckMenu 비활성화
        finalCheckMenu.SetActive(false);
        checking = false; //checking false로 바뀜
    }

    //카메라 줌 아웃 기능
    IEnumerator Cam_ZoomOut()
    {
        while (true)
        {
            //카메라의 localPosition을 Vector3.Slerp를 이용해 부드럽게 줌아웃
            //카메라의 형제 localPosition에서 new Vector3(0,3,-5f)의 위치로
            //카메라가 이동하고, 20*Time.deltaTime의 속도로 줌인된다.
            cam.transform.localPosition =
              Vector3.Slerp(cam.transform.localPosition,
              new Vector3(0, 3, -5f), 20 * Time.deltaTime);

            //카메라가 이동하다가 localPosition.z이 -5f보다 작거나 같아지면,
            //줌인 코르틴 종료 
            if (cam.transform.localPosition.z <= -5f)
                StopCoroutine("Cam_ZoomOut");

            yield return null;
        }
    }
}
