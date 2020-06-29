using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{    
    public GameObject obj;

    private Vector3 dir;
    private Vector3 initPos;

    private GameObject uiManager;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.visible = false;

        initPos = GameObject.FindWithTag("Player").transform.position;
        Debug.Log(initPos);

        uiManager = GameObject.Find("UIManager");
    }

    public void PlayerPosInit()
    {
        GameObject.FindWithTag("Player").transform.position = initPos;
    } 

    //트리거를 활용한 오브젝트 이름 얻어오기
    /// <summary>
    /// GetGameObjName("오브젝트이름")
    /// </summary>
    /// <param name="objName"></param>
    void GetGameObjName(string objName)
    {
        //Debug.Log(obj.name);
        obj = GameObject.Find(objName);
    }

    //오프젝트 파괴
    /// <summary>
    /// ObjectDestroy(오브젝트 이름)
    /// </summary>
    /// <param name="objName"></param>
    void ObjectDestroy(string objName)
    {
        Destroy(GameObject.Find(objName));
    }

    /// <summary>
    /// NPC 트리거안에 플레이어가 들어왔을때 발생
    /// </summary>
    /// <param name="npc"></param>
    void NpcPlayerNotice(GameObject npc)
    { 
        Debug.Log(npc.name + " NPC가 플레이어를 알아차렸습니다");
        uiManager.SendMessage("StartTalk", "npc");
    }

    /// <summary>
    /// NPC 트리거안에 플레이어가 나갔을때 발생
    /// </summary>
    /// <param name="npc"></param>
    void NpcPlayerIgnore(GameObject npc)
    {
        Debug.Log(npc.name + " NPC가 플레이어를 무시합니다");
    }


    // Update is called once per frame
    void Update()
    {
        //스페이스 입력시 회전
        if (Input.GetKeyDown(KeyCode.Space) && obj != null)
        {
            if(obj.tag == "Object")
            {
                obj.SendMessage("ObjRotation");
                Debug.Log("스페이스바 입력");
            }
        }
        //방향키 입력시 이동
        if(Input.GetKey(KeyCode.LeftArrow) && obj != null)
        {
            if(obj.tag == "Object")
            {
                dir.Set(-1, 0, 0);
                obj.SendMessage("ObjMove", dir);
            }

        }
        if (Input.GetKey(KeyCode.RightArrow) && obj != null)
        {
            if (obj.tag == "Object")
            {
                dir.Set(1, 0, 0);
                obj.SendMessage("ObjMove", dir);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow) && obj != null)
        {
            if (obj.tag == "Object")
            {
                dir.Set(0, 0, 1);
                obj.SendMessage("ObjMove", dir);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) && obj != null)
        {
            if (obj.tag == "Object")
            {
                dir.Set(0, 0, -1);
                obj.SendMessage("ObjMove", dir);
            }
        }
    }
}
