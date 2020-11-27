using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGame : MonoBehaviour
{
    
    public GameObject fishingManager;
    public GameObject fishingDir;
    public GameObject fishingFloat;
    public Vector3 adjPos;

    GameObject fishingGear;
    Vector3 playerPos;
    Quaternion playerRot;
    Vector3 waterPos;
    Vector3 initPos;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        fishingGear = this.gameObject;
        initPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (fishingManager.GetComponent<FishingManager>().GetFishGame() && fishingDir.GetComponent<FishingDir>().GetWaterHit())
        {
            ShowFishingGear();
            timer += Time.deltaTime;
        }
            
        if (!fishingManager.GetComponent<FishingManager>().GetFishGame())
            HideFishingGear();
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

    public void InitTimer(bool boolean)
    {
        if (boolean)
            timer = 0.0f;
    }
}
