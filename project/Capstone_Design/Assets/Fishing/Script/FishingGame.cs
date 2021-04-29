using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGame : MonoBehaviour
{

    public AudioClip floatSplashSound;
    public AudioClip successSound;
    public GameObject fishingManager;
    public GameObject fishingDir;
    public GameObject fishingFloat;
    public GameObject fishingWarnUI;
    public Vector3 adjPos;
    public float minTime;
    public float maxTime;
    public float baitTime;
    public int sucPercentage;

    GameObject fishingGear;
    Vector3 playerPos;
    Quaternion playerRot;
    Vector3 waterPos;
    Vector3 initPos;
    bool rayChecker;
    bool baitChecker;
    float timer;
    float baitTimer;
    float randomTime;
    Message msg;

    // Start is called before the first frame update
    void Start()
    {
        fishingGear = this.gameObject;
        initPos = this.transform.position;
        rayChecker = false;

        //물고기 아이템코드 추가
        msg = new Message("InventoryManager/AddNewItem : Fish, 물고기, 평범한 물고기다, 맛있다");
        msg.FunctionCall();
        msg = new Message("InventoryManager/AddNewItem : Boots, 부츠, 우-야, 독일산");
        msg.FunctionCall();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rayChecker && fishingManager.GetComponent<FishingManager>().GetFishGame() && fishingDir.GetComponent<FishingDir>().GetWaterHit())
        {
            ShowFishingGear();
            randomTime = Random.Range(minTime, maxTime);

            rayChecker = true;
        }

        if(fishingManager.GetComponent<FishingManager>().GetFishGame())
        {
            timer += Time.deltaTime;

            if(!baitChecker && timer >= randomTime)
            {
                Baited();
                baitChecker = true;
            }

            if (timer >= randomTime)
            {
                baitTimer += Time.deltaTime;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Success();
                }

                if(baitTimer > baitTime)
                {
                    Failed();
                }
            }
        }
        else
        {
            HideFishingGear();
            InitTimer();

            rayChecker = false;
            baitChecker = false;
        }
            
    }

    public void ShowFishingGear()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        playerRot = GameObject.FindGameObjectWithTag("Player").transform.rotation;
        fishingGear.transform.position = playerPos + adjPos;
        fishingGear.transform.rotation = playerRot;

        if (fishingDir.GetComponent<FishingDir>().GetWaterHit())
        {
            waterPos = fishingDir.GetComponent<FishingDir>().GetWaterPos();
            fishingFloat.GetComponent<FishingFloat>().SetPos(waterPos);

            //소리재생
            this.GetComponent<AudioSource>().clip = floatSplashSound;
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void HideFishingGear()
    {
        fishingGear.transform.position = initPos;
        fishingFloat.GetComponent<FishingFloat>().InitPos();
    }

    public void SetFishingDir(GameObject dir)
    {
        fishingDir = dir;
    }

    public void InitTimer()
    {
        timer = 0.0f;
        baitTimer = 0.0f;
    }

    public void Baited()
    {
        fishingFloat.GetComponent<FishingFloat>().InitPos();
        print("낚였스!");
        fishingWarnUI.GetComponent<FishingWarnUI>().SetEnabled(true);
    }

    public void Failed()
    {
        fishingWarnUI.GetComponent<FishingWarnUI>().SetEnabled(false);
        fishingManager.GetComponent<FishingManager>().SetFishGame(false);
        print("물고기를 놓쳤다..");
    }

    public void Success()
    {
        fishingWarnUI.GetComponent<FishingWarnUI>().SetEnabled(false);
        fishingManager.GetComponent<FishingManager>().SetFishGame(false);

        //소리재생
        this.GetComponent<AudioSource>().clip = successSound;
        this.GetComponent<AudioSource>().Play();

        if (sucPercentage > 100)
            sucPercentage = 100;
        else if (sucPercentage < 0)
            sucPercentage = 0;

        if (Random.Range(0, 100) <= sucPercentage)
        {
            print("물고기를 잡았다!");
            //인벤토리 물고기 추가코드
            msg = new Message("InventoryManager/ModifyItem : Fish, 1");
            msg.FunctionCall();
        }
        else
        {
            print("물고기가 아닌걸 잡았다..");
            msg = new Message("InventoryManager/ModifyItem : Boots, 1");
            msg.FunctionCall();
        }
    }
}
