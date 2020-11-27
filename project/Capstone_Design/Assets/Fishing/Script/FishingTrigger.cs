using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingTrigger : MonoBehaviour
{ 

    public Vector3 startUIScale;
    public GameObject fishStartUI;
    public GameObject fishingPos;
    public GameObject fishingDir;
    public GameObject fishingGame;
    public GameObject fishingManager;
    

    bool isFishable;
    Component fishingManage;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        isFishable = false;

        //게임 시작시 안보이게 하기
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isFishable == false && fishStartUI != null && fishingManager.GetComponent<FishingManager>().GetFishGame() == false)
            {
                //UI 생성
                Instantiate(fishStartUI, new Vector3(0, 0, 0), Quaternion.identity);
                GameObject.FindGameObjectWithTag("FSUI").GetComponent<FishingStartUI>().GetUIScale(startUIScale);
                    //SendMessage("GetUIScale", startUIScale, SendMessageOptions.DontRequireReceiver);
            }

            fishingGame.GetComponent<FishingGame>().SetFishingDir(fishingDir);

            isFishable = true;

            print("낚시트리거 활성화!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isFishable = false;

            if (fishStartUI != null)
                Destroy(GameObject.FindGameObjectWithTag("FSUI"));

            fishingManager.GetComponent<FishingManager>().SetFishGame(false);
            print("낚시트리거 비활성화!");
        }
    }

    private void Update()
    {
        
        if(isFishable == true && Input.GetKeyDown(KeyCode.E) == true)
        {
            //낚시매니저에게 게임시작 전달
            fishingManager.GetComponent<FishingManager>().SetFishGame(true);

            player.transform.position = fishingPos.transform.position;
            player.transform.LookAt(new Vector3(fishingDir.transform.position.x, player.transform.position.y, fishingDir.transform.position.z));

            print("게임시작!");
        }
        if (fishingManager.GetComponent<FishingManager>().GetFishGame() == true)
            Destroy(GameObject.FindGameObjectWithTag("FSUI"));
    }
}
